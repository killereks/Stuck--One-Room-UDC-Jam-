using UnityEngine;

public class ImageFileInfo : FileInfo {

    public Sprite sprite { get; private set; }

    public ImageFileInfo(string name, Sprite sprite) {
        this.name = name;
        this.sprite = sprite;
    }

}
