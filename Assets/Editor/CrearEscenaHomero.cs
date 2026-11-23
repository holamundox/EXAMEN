using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public static class CrearEscenaHomero
{
    [MenuItem("Tools/Crear escena de Homero")]
    public static void Crear()
    {
        Directory.CreateDirectory("Assets/Sprites");
        string ruta = "Assets/Sprites/cuadrado.png";
        if (!File.Exists(ruta))
        {
            var tex = new Texture2D(32, 32);
            var px = new Color[32 * 32];
            for (int i = 0; i < px.Length; i++) px[i] = Color.white;
            tex.SetPixels(px);
            File.WriteAllBytes(ruta, tex.EncodeToPNG());
            AssetDatabase.ImportAsset(ruta);
            var imp = (TextureImporter)AssetImporter.GetAtPath(ruta);
            imp.textureType = TextureImporterType.Sprite;
            imp.spritePixelsPerUnit = 32;
            imp.filterMode = FilterMode.Point;
            imp.SaveAndReimport();
        }
        Sprite cuadrado = AssetDatabase.LoadAssetAtPath<Sprite>(ruta);
        AudioClip sonido = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/bloque.wav");

        var escena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        var camGO = new GameObject("Main Camera");
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true; cam.orthographicSize = 5;
        cam.backgroundColor = new Color(0.45f, 0.75f, 1f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        camGO.tag = "MainCamera";
        camGO.AddComponent<AudioListener>();
        camGO.transform.position = new Vector3(0, 0, -10);

        var suelo = Cuadrado("Suelo", cuadrado, new Vector3(0, -3, 0), new Vector3(40, 1, 1), new Color(0.3f, 0.6f, 0.2f));
        suelo.AddComponent<BoxCollider2D>();

        var obst = Cuadrado("Obstaculo", cuadrado, new Vector3(3, -2, 0), Vector3.one, new Color(0.6f, 0.3f, 0.2f));
        obst.AddComponent<BoxCollider2D>();

        var bloque = Cuadrado("BloqueSonido", cuadrado, new Vector3(-3, 0.5f, 0), Vector3.one, new Color(1f, 0.8f, 0.1f));
        bloque.AddComponent<BoxCollider2D>();
        var qb = bloque.AddComponent<QuestionBlock>();
        qb.sonido = sonido;

        var homero = Cuadrado("Homero", cuadrado, new Vector3(-3, -2, 0), Vector3.one, new Color(1f, 0.9f, 0.2f));
        homero.GetComponent<SpriteRenderer>().sortingOrder = 5;
        homero.AddComponent<Rigidbody2D>();
        homero.AddComponent<BoxCollider2D>();
        homero.AddComponent<PlayerController>();

        camGO.AddComponent<CameraFollow>().objetivo = homero.transform;

        Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(escena, "Assets/Scenes/Homero.unity");
        Debug.Log("Escena creada en Assets/Scenes/Homero.unity. Dale Play!");
    }

    static GameObject Cuadrado(string nombre, Sprite s, Vector3 pos, Vector3 escala, Color color)
    {
        var go = new GameObject(nombre);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = s; sr.color = color;
        go.transform.position = pos;
        go.transform.localScale = escala;
        return go;
    }
}
