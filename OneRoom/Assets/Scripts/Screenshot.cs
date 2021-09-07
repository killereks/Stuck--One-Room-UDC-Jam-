using System.IO;
using UnityEditor;
using UnityEngine;

public class Screenshot : MonoBehaviour {

    public Vector2Int imageSize;
    public string fileName;

    public void TakePicture() {
        string path = Path.Combine(Application.dataPath, "Screenshots");

        Directory.CreateDirectory(path);

        var oldRT = RenderTexture.active;

        RenderTexture renderTexture = new RenderTexture(imageSize.x, imageSize.y, 16, RenderTextureFormat.ARGB32);

        Camera cam = GetComponent<Camera>();

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture.active = renderTexture;

        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        Color colorToRemove = new Color(1f, 0f, 1f);

        for (int y = 0; y < image.height; y++) {
            for (int x = 0; x < image.width; x++) {
                if (image.GetPixel(x, y) == colorToRemove) {
                    image.SetPixel(x, y, new Color(0f, 0f, 0f, 0f));
                }
            }
        }

        byte[] bytes = image.EncodeToPNG();
        DestroyImmediate(image);

        File.WriteAllBytes(Path.Combine(path, $"{fileName}.png"), bytes);
        RenderTexture.active = oldRT;
    }

}

[CustomEditor(typeof(Screenshot))]
public class ScreenshotEditor : Editor {

    public override void OnInspectorGUI() {
        Screenshot screenshot = (Screenshot) target;

        screenshot.imageSize = EditorGUILayout.Vector2IntField("Image Dimensions", screenshot.imageSize);
        screenshot.fileName = EditorGUILayout.TextField("File name", screenshot.fileName);

        if (GUILayout.Button("Take Screenshot")) {
            screenshot.TakePicture();
        }
    }

}