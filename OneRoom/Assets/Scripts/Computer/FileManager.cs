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
    [BoxGroup("Default Settings")]
    public Transform applicationParent;

    [BoxGroup("File Prefabs")]
    public GameObject folderPrefab;
    [BoxGroup("File Prefabs")]
    public GameObject textFilePrefab;
    [BoxGroup("File Prefabs")]
    public GameObject imageFilePrefab;

    [BoxGroup("File Opening")]
    public GameObject textFileApplicationPrefab;
    [BoxGroup("File Opening")]
    public GameObject imageFileApplicationPrefab;
    [BoxGroup("File Opening")]
    public GameObject fileExplorer;

    private void Start() {
        instance = this;
    }

    public void NewTextFile(Transform parent, string fullPath, TextFileInfo file) {
        GameObject textFile = Instantiate(textFilePrefab, parent);

        textFile.GetComponentInChildren<TextMeshProUGUI>().text = file.name;

        textFile.GetComponent<Button>().onClick.AddListener(() => {
            OpenTextFile(file);
        });
    }

    public void NewImageFile(Transform parent, string fullPath, ImageFileInfo file) {
        GameObject imageFile = Instantiate(imageFilePrefab, parent);

        imageFile.GetComponentInChildren<Image>().sprite = file.sprite;
        imageFile.GetComponentInChildren<TextMeshProUGUI>().text = file.name;

        imageFile.GetComponent<Button>().onClick.AddListener(() => {
            OpenImageFile(file);
        });
    }

    public void NewFolderFile(Transform parent, string folderName, string fullPath, UnityAction action) {
        GameObject newFolderFile = Instantiate(folderPrefab, parent);

        newFolderFile.GetComponentInChildren<TextMeshProUGUI>().text = folderName;

        newFolderFile.GetComponent<Button>().onClick.AddListener(() => action.Invoke());
    }

    public void OpenTextFile(TextFileInfo file) {
        GameObject textViewer = Instantiate(textFileApplicationPrefab, applicationParent);

        TextMeshProUGUI[] texts = textViewer.GetComponentsInChildren<TextMeshProUGUI>();

        texts[0].text = file.name;
        texts[1].text = file.content;
    }

    public void OpenImageFile(ImageFileInfo file) {
        GameObject imageViewer = Instantiate(imageFileApplicationPrefab, applicationParent);

        Image image = imageViewer.GetComponentInChildren<ScrollSize>().GetComponent<Image>();

        imageViewer.GetComponentInChildren<TextMeshProUGUI>().text = file.name;

        image.sprite = file.sprite;
    }

    public void OpenFileExplorer() {
        Instantiate(fileExplorer, applicationParent);
    }
}
