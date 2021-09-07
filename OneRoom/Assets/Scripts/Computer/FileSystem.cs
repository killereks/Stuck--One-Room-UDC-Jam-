using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileSystem : MonoBehaviour {

    public FileTree masterNode = new FileTree("Master","Master");

    public static FileSystem instance;

    private void Start() {
        instance = this;

        masterNode.CreateDirectory("C/Desktop/some files");
        masterNode.CreateDirectory("C/Desktop/some other files");
        masterNode.CreateDirectory("C/Documents");
        masterNode.CreateDirectory("C/Downloads");
        masterNode.CreateDirectory("C/Music");
        masterNode.CreateDirectory("C/Pictures");
        masterNode.CreateDirectory("C/Videos");

        masterNode.CreateDirectory("C/KiRoX/System Files/addins");
        masterNode.CreateDirectory("C/KiRoX/System Files/debug");
        masterNode.CreateDirectory("C/KiRoX/System Files/en-GB");
        masterNode.CreateDirectory("C/KiRoX/System Files/System");
        masterNode.CreateDirectory("C/KiRoX/System Files/System32");
        masterNode.CreateDirectory("C/KiRoX/System Files/DLLs/Assembly");

        for (int i = 0; i < 10; i++) {
            masterNode.AddFile("C/KiRoX/System Files/DLLs", new TextFileInfo("somerandom.dll", "This is content but different"));
        }
        for (int i = 0; i < 6; i++) {
            masterNode.AddFile("C/KiRoX/System Files/DLLs/Assembly", new TextFileInfo("somefile.dll", "Hello, this is content"));
        }

        masterNode.CreateDirectory("C/Documents/Important Files");

        //masterNode.AddFile("C/KiRoX/System Files", new ImageFileInfo("my image.png", ContextMenu.instance.backgroundIcon));
        masterNode.AddFile("C/Documents", new TextFileInfo("Hello", "Hello"));

        //masterNode.AddFile("C/Documents", new ImageFileInfo("CoolPic", testSprite));
    }
}