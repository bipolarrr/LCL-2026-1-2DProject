param(
    [string[]]$ChangedPath,
    [string]$TaskFile,
    [switch]$DisallowCSharp,
    [switch]$DisallowUnityArtifacts
)

$ErrorActionPreference = "Stop"

function Convert-ToRepoPath
{
    param([string]$Path)

    return ($Path -replace "\\", "/").TrimStart("./")
}

function Get-ChangedPaths
{
    if ($ChangedPath -and $ChangedPath.Count -gt 0)
    {
        return $ChangedPath | ForEach-Object { Convert-ToRepoPath $_ }
    }

    if (Test-Path ".git")
    {
        $paths = git diff --name-only
        if ($paths)
        {
            return $paths | ForEach-Object { Convert-ToRepoPath $_ }
        }
    }

    return @()
}

function Read-AllowedPaths
{
    param([string]$Path)

    if (-not $Path)
    {
        return @()
    }

    if (-not (Test-Path $Path))
    {
        throw "Task file not found: $Path"
    }

    $lines = Get-Content $Path
    $allowed = New-Object System.Collections.Generic.List[string]
    $inAllowed = $false

    foreach ($line in $lines)
    {
        if ($line -match "^##\s+Allowed Write Paths")
        {
            $inAllowed = $true
            continue
        }

        if ($inAllowed -and $line -match "^##\s+")
        {
            break
        }

        if ($inAllowed -and $line -match "^\s*-\s+(.+?)\s*$")
        {
            $allowed.Add((Convert-ToRepoPath $Matches[1]))
        }
    }

    return $allowed.ToArray()
}

function Test-GlobMatch
{
    param(
        [string]$Path,
        [string]$Pattern
    )

    $regex = [regex]::Escape($Pattern)
    $regex = $regex -replace "\\\*\\\*", ".*"
    $regex = $regex -replace "\\\*", "[^/]*"
    return $Path -match "^$regex$"
}

$paths = @(Get-ChangedPaths)
$allowedPaths = @(Read-AllowedPaths $TaskFile)
$violations = New-Object System.Collections.Generic.List[string]

foreach ($path in $paths)
{
    if ($DisallowCSharp -and $path -match "^Assets/.+\.cs$")
    {
        $violations.Add("Runtime or editor C# changed: $path")
    }

    if ($DisallowUnityArtifacts -and $path -match "^Assets/.+\.(unity|prefab|asset|asmdef)$")
    {
        $violations.Add("Unity asset, scene, prefab, or asmdef changed: $path")
    }

    if ($path -match "^(Packages|ProjectSettings|UserSettings|Library|Temp)/")
    {
        $violations.Add("Protected Unity project path changed: $path")
    }

    if ($allowedPaths.Count -gt 0)
    {
        $isAllowed = $false
        foreach ($allowed in $allowedPaths)
        {
            if ((Test-GlobMatch $path $allowed) -or $path.StartsWith($allowed.TrimEnd("/") + "/"))
            {
                $isAllowed = $true
                break
            }
        }

        if (-not $isAllowed)
        {
            $violations.Add("Changed path is outside task allowed paths: $path")
        }
    }
}

if ($violations.Count -gt 0)
{
    Write-Host "Boundary check failed:"
    foreach ($violation in $violations)
    {
        Write-Host "- $violation"
    }

    exit 1
}

if ($paths.Count -eq 0)
{
    Write-Host "Boundary check completed. No changed paths were detected. Pass -ChangedPath or run inside a Git worktree to check a diff."
}
else
{
    Write-Host "Boundary check passed for $($paths.Count) changed path(s)."
}
