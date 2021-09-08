using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileSystem : MonoBehaviour {

    public FileTree masterNode = new FileTree("Master","Master");

    public static FileSystem instance;

    public Sprite[] programmingMemes;
    public Sprite[] mathMemes;

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

        masterNode.CreateDirectory("C/Documents/Memes/Programming Memes");
        masterNode.CreateDirectory("C/Documents/Memes/Math Memes");

        for (int i = 0; i < programmingMemes.Length; i++) {
            Sprite sprite = programmingMemes[i];
            masterNode.AddFile("C/Documents/Memes/Programming Memes", new ImageFileInfo($"meme{i+1}.png", sprite));
        }
        for (int i = 0; i < mathMemes.Length; i++) {
            Sprite sprite = mathMemes[i];
            masterNode.AddFile("C/Documents/Memes/Math Memes", new ImageFileInfo($"funnymath{i + 1}.png", sprite));
        }
        masterNode.AddFile("C/Documents", new TextFileInfo("Hello", "Hello"));

        //masterNode.AddFile("C/Documents", new ImageFileInfo("CoolPic", testSprite));
    }
}