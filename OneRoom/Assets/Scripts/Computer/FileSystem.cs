using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            string code = GenerateRandomCode(Random.Range(100, 300));
            masterNode.AddFile("C/KiRoX/System Files/DLLs", new TextFileInfo($"{RandomLetters(8)}.dll", code));
        }
        for (int i = 0; i < 6; i++) {
            string code = GenerateRandomCode(Random.Range(100, 300));
            masterNode.AddFile("C/KiRoX/System Files/DLLs/Assembly", new TextFileInfo($"{RandomLetters(8)}.dll", code));
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

    string GenerateRandomCode(int lines) {

        StringBuilder outcome = new StringBuilder();

        bool isLabel = false;

        string[] labelNames = new string[] {"loop","start","interrupt","state","output",
            "setupw1","setupw2","setupwdt","mainloop","userinput","user","input",
            "counter","sum","again","lamp","loop_amp","betaop","random","code","master","directory","file","stream"};

        void NewLineNumber(int lineNumber) {
            outcome.Append(lineNumber).Append("\t").Append("<color=#3498db>|</color>").Append("\t");
        }

        string GetMemory() {
            if (Random.value <= 0.7f) {
                if (Random.value <= 0.8f) {
                    return "e" + RandomLetters(1) + "x";
                }
                return "r" + Random.Range(1, 17);
            }
            return $"#{Random.Range(1, 1001)}";
        }

        for (int i = 0; i < lines; i++) {
            int number = Random.Range(0, 8);

            NewLineNumber(i);

            if (isLabel) {
                outcome.Append("\t");
            }

            switch (number) {
                case 0:
                    outcome.AppendLine($"<color=#4caf50>ADD</color>\t{GetMemory()},\t{GetMemory()},\t{GetMemory()}");
                    break;
                case 1:
                    outcome.AppendLine($"<color=#4caf50>SUB</color>\t{GetMemory()},\t{GetMemory()},\t{GetMemory()}");
                    break;
                case 2:
                    outcome.AppendLine($"<color=#3498DB>CMP</color>\t{GetMemory()},\t{GetMemory()}");
                    NewLineNumber(i);
                    outcome.AppendLine($"<color=#3498DB>BLT</color>\t{labelNames[Random.Range(0, labelNames.Length)]}");
                    break;
                case 3:
                    outcome.AppendLine($"<color=#4caf50>ORR</color>\t{GetMemory()},\t{GetMemory()},\t{ GetMemory()}");
                    break;
                case 4:
                    outcome.AppendLine($"<color=#9B59B6>LSL</color>\t{GetMemory()},\t{GetMemory()},\t{ GetMemory()}");
                    break;
                case 5:
                    outcome.AppendLine($"<color=#9B59B6>LSR</color>\t{GetMemory()},\t{GetMemory()},\t{ GetMemory()}");
                    break;
                case 6:
                    outcome.AppendLine($"<color=#4caf50>STR</color>\t{GetMemory()},\t{GetMemory()}");
                    break;
                case 7:
                    if (isLabel) {
                        isLabel = false;
                        outcome.AppendLine();
                        break;
                    }
                    outcome.AppendLine($"<color=#E67E22>{labelNames[Random.Range(0, labelNames.Length)]}:</color>");
                    isLabel = true;
                    break;
            }
        }
        NewLineNumber(lines);
        outcome.AppendLine("HLT");

        return outcome.ToString();
    }

    string RandomLetters(int count) {
        char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < count; i++) {
            stringBuilder.Append(alphabet[Random.Range(0, alphabet.Length)]);
        }

        return stringBuilder.ToString();
    }
}