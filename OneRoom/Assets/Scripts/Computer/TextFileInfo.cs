using UnityEngine;

public class TextFileInfo : FileInfo {
    public string content { get; private set; }

    public TextFileInfo(string name, string content) {
        this.name = name;
        this.content = content;
    }
}
