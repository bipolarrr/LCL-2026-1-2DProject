using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

/// <summary>
/// 메뉴 Tools > Build Demo Scene 클릭 한 번으로
/// Ground, Platform, Player, Enemy, Coin, GameManager를 자동 배치한다.
/// Undo 지원 — Ctrl+Z로 전부 되돌릴 수 있다.
/// </summary>
public static class DemoSceneBuilder
{
    private const string MenuPath = "Tools/Build Demo Scene";

    [MenuItem(MenuPath)]
    public static void BuildDemoScene()
    {
        if (!EditorUtility.DisplayDialog(
            "Demo Scene Builder",
            "현재 씬에 데모 오브젝트를 배치합니다.\n동명 오브젝트가 있으면 삭제 후 재생성합니다.",
            "배치", "취소"))
        {
            return;
        }

        Undo.SetCurrentGroupName("Build Demo Scene");
        int undoGroup = Undo.GetCurrentGroup();

        // --- Layer ---
        int groundLayer = GetOrCreateLayer("Ground");
        if (groundLayer < 0)
        {
            Debug.LogError("[DemoSceneBuilder] Ground 레이어를 만들 수 없습니다. 빈 User Layer가 없습니다.");
            return;
        }

        // --- Sprites ---
        Sprite square = FindSprite("Square");
        Sprite circle = FindSprite("Circle");

        if (square == null || circle == null)
        {
            Debug.LogError("[DemoSceneBuilder] 기본 Square/Circle 스프라이트를 찾을 수 없습니다.");
            return;
        }

        // --- Clean existing ---
        DestroyIfExists("Ground");
        DestroyIfExists("Platform");
        DestroyIfExists("Player");
        DestroyIfExists("Enemy");
        DestroyIfExists("EnemyLaser");
        DestroyIfExists("Coin");
        DestroyIfExists("GameManager");
        DestroyIfExists("BouncePad");
        DestroyIfExists("WallLeft");
        DestroyIfExists("WallRight");

        // --- Build ---
        CreateGround(square, groundLayer);
        CreateWalls(square, groundLayer);
        CreatePlatform(square, groundLayer);
        CreateBouncePad(square, groundLayer);
        CreatePlayer(square, groundLayer);
        CreateEnemy(circle);
        CreateEnemyLaser(circle, groundLayer);
        CreateCoin(square);
        CreateGameManager();

        Undo.CollapseUndoOperations(undoGroup);
        Debug.Log("[DemoSceneBuilder] Demo Scene 배치 완료.");
    }

    // ================================================================
    //  Ground  — (0, -3)  20×1  진회색
    // ================================================================
    private static void CreateGround(Sprite sprite, int layer)
    {
        GameObject go = CreateSpriteObject("Ground", sprite, new Vector3(0f, -3f, 0f));
        go.transform.localScale = new Vector3(20f, 1f, 1f);
        go.layer = layer;

        SetColor(go, HexColor("555555"));
        go.AddComponent<BoxCollider2D>();
    }

    // ================================================================
    //  Platform  — (3, 0)  4×0.5  밝회색
    // ================================================================
    private static void CreatePlatform(Sprite sprite, int layer)
    {
        GameObject go = CreateSpriteObject("Platform", sprite, new Vector3(3f, 0f, 0f));
        go.transform.localScale = new Vector3(4f, 0.5f, 1f);
        go.layer = layer;

        SetColor(go, HexColor("888888"));
        go.AddComponent<BoxCollider2D>();
    }

    // ================================================================
    //  BouncePad  — (-2, -2.5)  1.5×0.3  초록색, Trigger
    // ================================================================
    private static void CreateBouncePad(Sprite sprite, int groundLayer)
    {
        GameObject go = CreateSpriteObject("BouncePad", sprite, new Vector3(-2f, -2.5f, 0f));
        go.transform.localScale = new Vector3(1.5f, 0.3f, 1f);
        go.layer = groundLayer;

        SetColor(go, HexColor("00CC44"));

        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        go.AddComponent<BouncePad>();
    }

    // ================================================================
    //  Player  — (-5, -1.5)  1×1  흰색
    //  + Rigidbody2D, BoxCollider2D
    //  + PlayerInputReader, PlayerMotor, PlayerHealth, PlayerController
    //  + 자식: GroundCheck
    // ================================================================
    private static void CreatePlayer(Sprite sprite, int groundLayer)
    {
        GameObject go = CreateSpriteObject("Player", sprite, new Vector3(-5f, -1.5f, 0f));
        go.tag = "Player";

        // Rigidbody2D
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Collider
        go.AddComponent<BoxCollider2D>();

        // Scripts — 순서 중요: Motor가 먼저 있어야 Health/Controller가 참조 가능
        go.AddComponent<PlayerInputReader>();
        go.AddComponent<PlayerMotor>();
        go.AddComponent<PlayerHealth>();
        go.AddComponent<PlayerController>();

        // GroundCheck 자식
        GameObject groundCheck = new GameObject("GroundCheck");
        Undo.RegisterCreatedObjectUndo(groundCheck, "Create GroundCheck");
        groundCheck.transform.SetParent(go.transform);
        groundCheck.transform.localPosition = new Vector3(0f, -0.55f, 0f);

        // PlayerMotor에 GroundCheck, GroundLayer 연결
        SerializedObject motorSo = new SerializedObject(go.GetComponent<PlayerMotor>());
        motorSo.FindProperty("groundCheck").objectReferenceValue = groundCheck.transform;
        motorSo.FindProperty("groundLayer").intValue = 1 << groundLayer;
        motorSo.ApplyModifiedProperties();

        // PlayerInputReader에 InputAction 연결
        WireInputActions(go);
    }

    /// <summary>
    /// InputSystem_Actions 에셋에서 Player/Move, Player/Jump 액션 레퍼런스를 찾아
    /// PlayerInputReader에 자동 연결한다.
    /// </summary>
    private static void WireInputActions(GameObject playerGo)
    {
        const string assetPath = "Assets/InputSystem_Actions.inputactions";
        Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        InputActionReference moveRef = null;
        InputActionReference jumpRef = null;

        foreach (Object sub in subAssets)
        {
            if (sub is InputActionReference actionRef && actionRef.action != null)
            {
                if (actionRef.action.actionMap?.name == "Player")
                {
                    if (actionRef.action.name == "Move") moveRef = actionRef;
                    else if (actionRef.action.name == "Jump") jumpRef = actionRef;
                }
            }
        }

        if (moveRef == null || jumpRef == null)
        {
            Debug.LogWarning("[DemoSceneBuilder] InputSystem_Actions에서 Player/Move 또는 Player/Jump를 찾을 수 없습니다. Inspector에서 수동 연결이 필요합니다.");
            return;
        }

        SerializedObject inputSo = new SerializedObject(playerGo.GetComponent<PlayerInputReader>());
        inputSo.FindProperty("moveActionRef").objectReferenceValue = moveRef;
        inputSo.FindProperty("jumpActionRef").objectReferenceValue = jumpRef;
        inputSo.ApplyModifiedProperties();
    }

    // ================================================================
    //  Enemy  — (4, -1.5)  1×1  빨간 원
    //  + Rigidbody2D, CircleCollider2D, EnemyPatrol, EnemyContactDamage
    // ================================================================
    private static void CreateEnemy(Sprite sprite)
    {
        GameObject go = CreateSpriteObject("Enemy", sprite, new Vector3(4f, -1.5f, 0f));

        SetColor(go, Color.red);

        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CircleCollider2D>();
        go.AddComponent<EnemyPatrol>();
        go.AddComponent<EnemyContactDamage>();
    }

    // ================================================================
    //  Walls  — 좌(-10, 0) 우(10, 0)  1×6  회색
    //  Ground 레이어 사용 → EnemyLaserBeam이 wallLayers로 감지
    // ================================================================
    private static void CreateWalls(Sprite sprite, int layer)
    {
        foreach (float x in new[] { -10f, 10f })
        {
            string name = x < 0 ? "WallLeft" : "WallRight";
            GameObject go = CreateSpriteObject(name, sprite, new Vector3(x, 0f, 0f));
            go.transform.localScale = new Vector3(1f, 6f, 1f);
            go.layer = layer;
            SetColor(go, HexColor("555555"));
            go.AddComponent<BoxCollider2D>();
        }
    }

    // ================================================================
    //  EnemyLaser  — (5, -1.5)  1×1  빨간 원
    //  + Rigidbody2D, CircleCollider2D, LineRenderer
    //  + EnemyLaserBeam, EnemyLaserController
    //  transform.right(+X) 방향으로 발사 → 오른쪽 벽에 도달
    // ================================================================
    private static void CreateEnemyLaser(Sprite sprite, int groundLayer)
    {
        GameObject go = CreateSpriteObject("EnemyLaser", sprite, new Vector3(5f, -1.5f, 0f));

        SetColor(go, Color.red);

        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CircleCollider2D>();
        go.AddComponent<EnemyPatrol>();
        go.AddComponent<EnemyFaceMovementDirection>();

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        go.AddComponent<EnemyLaserBeam>();
        go.AddComponent<EnemyLaserController>();

        // wallLayers를 Ground 레이어로 연결
        SerializedObject beamSo = new SerializedObject(go.GetComponent<EnemyLaserBeam>());
        beamSo.FindProperty("wallLayers").intValue = 1 << groundLayer;
        beamSo.ApplyModifiedProperties();
    }

    // ================================================================
    //  Coin  — (3, 1)  0.5×0.5  금색, Trigger
    // ================================================================
    private static void CreateCoin(Sprite sprite)
    {
        GameObject go = CreateSpriteObject("Coin", sprite, new Vector3(3f, 1f, 0f));
        go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        SetColor(go, HexColor("FFD700"));

        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        go.AddComponent<Coin>();
    }

    // ================================================================
    //  GameManager  — GameStateController
    // ================================================================
    private static void CreateGameManager()
    {
        GameObject go = new GameObject("GameManager");
        Undo.RegisterCreatedObjectUndo(go, "Create GameManager");
        go.AddComponent<GameStateController>();
    }

    // ================================================================
    //  Utility
    // ================================================================

    private static GameObject CreateSpriteObject(string name, Sprite sprite, Vector3 position)
    {
        GameObject go = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(go, $"Create {name}");

        go.transform.position = position;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return go;
    }

    private static void SetColor(GameObject go, Color color)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = color;
    }

    private static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString($"#{hex}", out Color color);
        return color;
    }

    private static void DestroyIfExists(string name)
    {
        GameObject existing = GameObject.Find(name);
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing);
        }
    }

    /// <summary>
    /// Ground 레이어가 없으면 빈 User Layer에 추가한다.
    /// </summary>
    private static int GetOrCreateLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");

        // 이미 있는지 확인
        for (int i = 0; i < layers.arraySize; i++)
        {
            if (layers.GetArrayElementAtIndex(i).stringValue == layerName)
                return i;
        }

        // 빈 User Layer 찾아서 할당 (8~31)
        for (int i = 8; i < layers.arraySize; i++)
        {
            if (string.IsNullOrEmpty(layers.GetArrayElementAtIndex(i).stringValue))
            {
                layers.GetArrayElementAtIndex(i).stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Unity 2D 패키지의 기본 스프라이트(Square, Circle 등)를 찾는다.
    /// </summary>
    private static Sprite FindSprite(string spriteName)
    {
        // Unity 2D Sprite 패키지 경로 (버전별로 다를 수 있음)
        string[] searchPaths =
        {
            $"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/{spriteName}.png",
            $"Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/{spriteName}.png",
        };

        foreach (string path in searchPaths)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null) return sprite;
        }

        // 폴백: 프로젝트 전체에서 검색
        string[] guids = AssetDatabase.FindAssets($"t:Sprite {spriteName}");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null && sprite.name == spriteName) return sprite;
        }

        return null;
    }
}
