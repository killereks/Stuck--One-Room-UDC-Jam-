using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;
using System;
using UnityEngine.Events;

public class FileManager : MonoBehaviour {

    public static FileManager instance;

    [BoxGroup("Default Settings")]
    public Transform desktopFiles;

    [BoxGroup("File Prefabs")]
    public GameObject folderPrefab;
    [BoxGroup("File Prefabs")]
    public GameObject textFilePrefab;
    [BoxGroup("File Prefabs")]
    public GameObject imageFilePrefab;

    private void Start() {
        instance = this;
    }

    public void NewTextFile(Transform parent, string fullPath, TextFileInfo file) {
        GameObject textFile = Instantiate(textFilePrefab, parent);

        textFile.GetComponentInChildren<TextMeshProUGUI>().text = file.name;

        /*TextFile textFileObj = textFile.GetComponent<TextFile>();

        textFileObj.fileInfo.SetName(file.GetName());
        textFileObj.SetTextData(file.GetData());*/
    }

    public void NewImageFile(Transform parent, string fullPath, ImageFileInfo file) {
        GameObject imageFile = Instantiate(imageFilePrefab, parent);

        imageFile.GetComponentInChildren<TextMeshProUGUI>().text = file.name;

        /*ImageFile imageFileObj = imageFile.GetComponent<ImageFile>();

        imageFileObj.fileInfo.SetName(file.GetName());
        imageFileObj.SetImageData(file.GetData());*/
    }

    public void NewFolderFile(Transform parent, string folderName, string fullPath, UnityAction action) {
        GameObject newFolderFile = Instantiate(folderPrefab, parent);

        newFolderFile.GetComponentInChildren<TextMeshProUGUI>().text = folderName;

        newFolderFile.GetComponent<Button>().onClick.AddListener(() => action.Invoke());

        /*Tools.FindDeepChild<TextMeshProUGUI>(newFolderFile, "Name").text = folderName;*/
    }
}
