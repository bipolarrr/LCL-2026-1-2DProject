### 에이전트 설치 방법

### 설치 메소드
- Windows 사용자는 **무조건 `Scoop`**
- Mac 사용자는 **`Brew`**

### 먼저 알아둘 것

### `터미널`

- 컴퓨터에게 글자로 명령하는 창

### `Scoop`

- Windows에서 프로그램 설치를 도와주는 도구

### `Brew`

- Mac에서 프로그램 설치를 도와주는 도구

### OpenCode와 Claude Code 차이

- `OpenCode`: 여러 AI 서비스를 연결해서 쓰는 오픈소스 코딩 에이전트
- `Claude Code`: Claude 공식 코딩 에이전트, 유료결제 필요.


> `OpenCode`는 GitHub Copilot 사용의 유일한 방법.
> `Claude Code`는 Claude 유료 결제가 필요한 방법.

### 공통 준비물

- 학생 인증이 된 Github 계정 또는 Claude Code를 사용할 수 있는 유료 구독 계정.

### Windows 사용자 2명: OpenCode 설치

### 설치 명령
1. Scoop 설치

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
Invoke-RestMethod -Uri https://get.scoop.sh | Invoke-Expression
```
2. Opencode 설치

```powershell
scoop install opencode
```
3. Opencode 실행

```powershell
opencode
```

### 처음 실행 후 할 일

OpenCode:

```text
/connect
```
1. `GitHub Copilot` 선택
2. 화면에 뜨는 코드를 확인
3. 브라우저에서 `https://github.com/login/device` 열기
4. 코드를 입력하고 GitHub 계정으로 승인
5. 다시 OpenCode로 돌아오기

사용가능한 모델을 확인해서 선택:

```text
/models
```

### OpenCode가 하는 일

> OpenCode는 AI 그 자체가 아님.  
> "어떤 AI 모델을 연결해서 쓸지 정하는 오픈소스 코딩 에이전트"임.

### Windows Claude Code 설치

Windows는 `Scoop`으로 설치.

### 1. 필요한 것 설치

```powershell
scoop install git
```

### 2. Claude Code 설치

```powershell
scoop install claude-code
```

### 3. 실행

```powershell
claude
```

### 4. 점검(이유는 모르겠지만 잘 안될 때)

```powershell
claude --version
claude doctor
```

### Mac: Brew로 Claude Code 설치

### 설치

```bash
brew install --cask claude-code
```

### 실행

```bash
claude
```

### 점검(이유는 모르겠지만 잘 안될 때)

```bash
claude --version
claude doctor
```
