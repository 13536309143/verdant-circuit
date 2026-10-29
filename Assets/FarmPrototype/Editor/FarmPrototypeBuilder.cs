using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TinyFarm;

public static class FarmPrototypeBuilder
{
    private const string Root = "Assets/FarmPrototype";
    private static Material grass, soil, soilLight, furrow, wood, cream, charcoal, teal, gold, leaf;
    private static TMP_FontAsset font;

    [MenuItem("Tools/Tiny Farm/Create Farm Prototype")]
    public static void Build()
    {
        if (Application.isPlaying) throw new System.InvalidOperationException("Stop Play mode before creating the farm.");
        if (SceneManager.GetActiveScene().isDirty)
            throw new System.InvalidOperationException("Save the current scene before creating the farm.");
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(Root + "/FarmPrototype.unity") != null)
            throw new System.InvalidOperationException("FarmPrototype already exists; open the saved scene instead.");
        EnsureFolder(Root);
        EnsureFolder(Root + "/Materials");
        font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        if (font == null) throw new System.InvalidOperationException("Import TMP essential resources first.");
        Shader shader = Shader.Find("Standard");
        if (shader == null) throw new System.InvalidOperationException("The Built-in Standard shader is unavailable.");
        grass = Mat("Meadow", "71926B", shader);
        soil = Mat("Soil", "78523B", shader);
        soilLight = Mat("SoilLight", "866048", shader);
        furrow = Mat("Furrow", "60432F", shader);
        wood = Mat("Cedar", "C69C69", shader);
        cream = Mat("Porcelain", "F3EBD7", shader);
        charcoal = Mat("Graphite", "263B3A", shader);
        teal = Mat("Mint", "6CC9B2", shader);
        gold = Mat("Marigold", "EAB957", shader);
        leaf = Mat("Leaf", "3F694D", shader);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "FarmPrototype";
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.76f, 0.80f, 0.78f);
        RenderSettings.fog = false;
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1, true);
        QualitySettings.shadowDistance = 40;

        var farm = new GameObject("Farm Environment").transform;
        Shape("Backdrop", PrimitiveType.Cube, new Vector3(0, -0.75f, 0), new Vector3(200, 0.2f, 200), Mat("Backdrop", "EAE9DE", shader), farm);
        Shape("Island Base", PrimitiveType.Cube, new Vector3(0, -0.39f, 0), new Vector3(7.5f, 0.5f, 7.1f), soil, farm);
        Shape("Meadow", PrimitiveType.Cube, new Vector3(0, -0.1f, 0), new Vector3(7.7f, 0.16f, 7.3f), grass, farm);
        var tiles = new GameObject("Field - 4 x 4").transform;
        tiles.SetParent(farm);
        for (int y = 0; y < 4; y++)
        for (int x = 0; x < 4; x++)
        {
            Vector3 p = new Vector3(x * 1.3f - 1.95f, 0.105f, y * 1.3f - 1.95f);
            var tile = Shape("Plot_" + x + "_" + y, PrimitiveType.Cube, p, new Vector3(1.18f, 0.15f, 1.18f), (x + y) % 2 == 0 ? soil : soilLight, tiles);
            for (int row = 0; row < 4; row++)
                Shape("Furrow_" + x + "_" + y + "_" + row, PrimitiveType.Cube,
                    p + new Vector3(0, 0.079f, (row - 1.5f) * 0.24f), new Vector3(0.94f, 0.012f, 0.027f), furrow, tiles);
        }
        Shape("Field Rim North", PrimitiveType.Cube, new Vector3(0, 0.085f, 2.66f), new Vector3(5.48f, 0.2f, 0.12f), wood, farm);
        Shape("Field Rim South", PrimitiveType.Cube, new Vector3(0, 0.085f, -2.66f), new Vector3(5.48f, 0.2f, 0.12f), wood, farm);
        Shape("Field Rim West", PrimitiveType.Cube, new Vector3(-2.66f, 0.085f, 0), new Vector3(0.12f, 0.2f, 5.22f), wood, farm);
        Shape("Field Rim East", PrimitiveType.Cube, new Vector3(2.66f, 0.085f, 0), new Vector3(0.12f, 0.2f, 5.22f), wood, farm);
        for (int i = 0; i < 6; i++)
        {
            float x = -3.35f + i * 1.34f;
            Shape("Fence Post " + i, PrimitiveType.Cube, new Vector3(x, 0.26f, 3.13f), new Vector3(0.12f, 0.62f, 0.12f), cream, farm);
        }
        Shape("Fence Lower", PrimitiveType.Cube, new Vector3(0, 0.15f, 3.13f), new Vector3(6.8f, 0.08f, 0.08f), cream, farm);
        Shape("Fence Upper", PrimitiveType.Cube, new Vector3(0, 0.42f, 3.13f), new Vector3(6.8f, 0.08f, 0.08f), cream, farm);
        for (int i = 0; i < 10; i++)
        {
            float x = i % 2 == 0 ? -3.3f : 3.3f;
            float z = -2.7f + (i / 2) * 1.12f;
            Tuft(new Vector3(x, 0, z), farm);
        }
        var crate = new GameObject("Supply Crates").transform;
        crate.SetParent(farm);
        Shape("Crate", PrimitiveType.Cube, new Vector3(2.95f, 0.18f, -3.02f), new Vector3(0.55f, 0.4f, 0.48f), wood, crate);
        for (int i = 0; i < 3; i++)
            Shape("Crate Slat " + i, PrimitiveType.Cube, new Vector3(2.95f, 0.39f, -3.18f + i * 0.16f), new Vector3(0.57f, 0.025f, 0.055f), soilLight, crate);

        var marker = new GameObject("Current Cell Outline").transform;
        for (int i = 0; i < 4; i++)
        {
            bool horizontal = i < 2;
            Shape("Outline " + i, PrimitiveType.Cube, Vector3.zero,
                horizontal ? new Vector3(1.13f, 0.015f, 0.035f) : new Vector3(0.035f, 0.015f, 1.13f), gold, marker).localPosition =
                horizontal ? new Vector3(0, 0, i == 0 ? -0.55f : 0.55f) : new Vector3(i == 2 ? -0.55f : 0.55f, 0, 0);
        }
        var robot = new GameObject("Farm Robot");
        var visual = new GameObject("Robot Visual").transform;
        visual.SetParent(robot.transform, false);
        Shape("Left Track", PrimitiveType.Cube, new Vector3(-0.27f, 0.12f, 0), new Vector3(0.16f, 0.22f, 0.65f), charcoal, visual);
        Shape("Right Track", PrimitiveType.Cube, new Vector3(0.27f, 0.12f, 0), new Vector3(0.16f, 0.22f, 0.65f), charcoal, visual);
        Shape("Chassis", PrimitiveType.Cube, new Vector3(0, 0.24f, 0), new Vector3(0.47f, 0.28f, 0.55f), cream, visual);
        Shape("Head", PrimitiveType.Cube, new Vector3(0, 0.49f, 0.02f), new Vector3(0.48f, 0.28f, 0.4f), cream, visual);
        Shape("Face", PrimitiveType.Cube, new Vector3(0, 0.49f, 0.226f), new Vector3(0.36f, 0.17f, 0.025f), charcoal, visual);
        Shape("Eye Left", PrimitiveType.Cube, new Vector3(-0.085f, 0.50f, 0.244f), new Vector3(0.065f, 0.055f, 0.015f), teal, visual);
        Shape("Eye Right", PrimitiveType.Cube, new Vector3(0.085f, 0.50f, 0.244f), new Vector3(0.065f, 0.055f, 0.015f), teal, visual);
        Shape("Antenna", PrimitiveType.Cylinder, new Vector3(0.13f, 0.7f, 0), new Vector3(0.025f, 0.075f, 0.025f), charcoal, visual);
        Shape("Beacon", PrimitiveType.Sphere, new Vector3(0.13f, 0.79f, 0), Vector3.one * 0.095f, gold, visual);
        Shape("Top Badge", PrimitiveType.Cube, new Vector3(0, 0.637f, 0), new Vector3(0.18f, 0.016f, 0.16f), teal, visual);

        var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
        cameraObject.tag = "MainCamera";
        var camera = cameraObject.GetComponent<Camera>();
        cameraObject.transform.position = new Vector3(0, 10, -7.8f);
        cameraObject.transform.LookAt(new Vector3(0, 0, 0.12f));
        camera.orthographic = true;
        camera.orthographicSize = 4.8f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Hex("EAE9DE");
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 80;
        camera.allowHDR = true;
        var sun = new GameObject("Directional Light", typeof(Light)).GetComponent<Light>();
        sun.type = LightType.Directional;
        sun.transform.rotation = Quaternion.Euler(48, -32, 0);
        sun.color = new Color(1, 0.94f, 0.82f);
        sun.intensity = 1.15f;
        sun.shadows = LightShadows.Soft;
        sun.shadowStrength = 0.45f;
        sun.shadowBias = 0.03f;

        TMP_Text coordinates, steps, status;
        BuildHud(out coordinates, out steps, out status);
        robot.AddComponent<FarmRobotController>().Configure(visual, marker, coordinates, steps, status);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, Root + "/FarmPrototype.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(Root + "/FarmPrototype.unity", true) };
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = robot;
        if (SceneView.lastActiveSceneView != null)
            SceneView.lastActiveSceneView.LookAt(new Vector3(0, 0, 0), camera.transform.rotation, 7);
        Debug.Log("Farm prototype ready: 4 x 4 field. Arrow keys / WASD move, R resets.");
    }

    private static void BuildHud(out TMP_Text coordinates, out TMP_Text steps, out TMP_Text status)
    {
        var canvasObject = new GameObject("Farm HUD", typeof(Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
        canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObject.GetComponent<UnityEngine.UI.CanvasScaler>();
        scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1440, 900);
        scaler.matchWidthOrHeight = 0.5f;
        var parent = canvasObject.transform;
        Color ink = Hex("263B3A");
        Color muted = Hex("5C7063");
        Label("Eyebrow", "YOUR FIRST FIELD  /  01", parent, new Vector2(0, 1), new Vector2(54, -42), new Vector2(500, 28), 17, muted);
        var title = Label("Title", "Little Farm", parent, new Vector2(0, 1), new Vector2(52, -73), new Vector2(510, 72), 58, ink);
        title.fontStyle = FontStyles.Bold;
        Label("Subtitle", "A small field. A little robot. A fresh start.", parent, new Vector2(0, 1), new Vector2(56, -149), new Vector2(540, 34), 21, muted);
        var info = Panel("Field Card", parent, new Vector2(1, 1), new Vector2(-48, -46), new Vector2(250, 114), Hex("F6F3E9"));
        Label("Field Size", "4 x 4  FIELD", info, new Vector2(0, 1), new Vector2(22, -17), new Vector2(208, 27), 22, ink).fontStyle = FontStyles.Bold;
        coordinates = Label("Coordinates", "CELL  02 / 02", info, new Vector2(0, 1), new Vector2(22, -51), new Vector2(208, 26), 19, muted);
        steps = Label("Steps", "000  STEPS", info, new Vector2(0, 1), new Vector2(22, -81), new Vector2(208, 22), 15, muted);
        var footer = Panel("Controls Card", parent, new Vector2(0.5f, 0), new Vector2(0, 30), new Vector2(640, 84), Hex("263B3A"));
        Label("Controls", "ARROW KEYS / WASD   Move     |     R   Reset", footer, new Vector2(0.5f, 0.5f), new Vector2(0, 12), new Vector2(610, 30), 20, Hex("F3EBD7"), TextAlignmentOptions.Center);
        status = Label("Status", "READY TO EXPLORE", footer, new Vector2(0.5f, 0.5f), new Vector2(0, -16), new Vector2(610, 22), 13, Hex("9BC4A8"), TextAlignmentOptions.Center);
    }

    private static RectTransform Panel(string name, Transform parent, Vector2 anchor, Vector2 position, Vector2 size, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(UnityEngine.UI.Image));
        var rect = go.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        go.GetComponent<UnityEngine.UI.Image>().color = color;
        go.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
        return rect;
    }

    private static TMP_Text Label(string name, string text, Transform parent, Vector2 anchor, Vector2 position,
        Vector2 size, float fontSize, Color color, TextAlignmentOptions alignment = TextAlignmentOptions.Left)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        var rect = go.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        var label = go.GetComponent<TextMeshProUGUI>();
        label.font = font;
        label.text = text;
        label.fontSize = fontSize;
        label.color = color;
        label.alignment = alignment;
        label.enableWordWrapping = false;
        label.raycastTarget = false;
        return label;
    }

    private static void Tuft(Vector3 p, Transform parent)
    {
        var a = Shape("Grass Tuft", PrimitiveType.Cube, p + Vector3.up * 0.09f, new Vector3(0.045f, 0.22f, 0.09f), leaf, parent);
        a.localRotation = Quaternion.Euler(0, 25, -24);
        var b = Shape("Grass Tuft", PrimitiveType.Cube, p + new Vector3(0.07f, 0.06f, 0.02f), new Vector3(0.035f, 0.18f, 0.07f), leaf, parent);
        b.localRotation = Quaternion.Euler(15, -15, 30);
    }

    private static Transform Shape(string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material, Transform parent)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        var collider = go.GetComponent<Collider>();
        if (collider != null) Object.DestroyImmediate(collider);
        return go.transform;
    }

    private static Material Mat(string name, string hex, Shader shader)
    {
        var material = new Material(shader);
        material.name = name;
        material.color = Hex(hex);
        material.SetFloat("_Glossiness", 0.05f);
        AssetDatabase.CreateAsset(material, Root + "/Materials/" + name + ".mat");
        return material;
    }

    private static Color Hex(string hex)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
    }
}
