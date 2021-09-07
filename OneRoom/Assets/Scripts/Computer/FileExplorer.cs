using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileExplorer : MonoBehaviour {

    string currentPath;

    public Transform filesParent;
    public GameObject directoryPrefab;
    public Transform directoryParent;

    List<string> history = new List<string>();

    private void Start() {
        LoadFiles();
    }

    public void UpdatePath(string path) {
        currentPath = path;

        AddToHistory(path);

        UpdateUI();
    }

    void AddToHistory(string elem) {
        if (history.Count > 0) {
            if (history[history.Count - 1] == elem) return;
        }

        history.Add(elem);
        if (history.Count > 30) {
            history.RemoveAt(0);
        }
    }

    public void HistoryPrevious() {
        if (history.Count > 1) {
            // remove twice (to remove the recently added one)
            history.RemoveAt(history.Count - 1);
            UpdatePath(history[history.Count - 1]);
        }
    }

    public void UpdateUI() {
        LoadFiles();

        foreach (Transform child in directoryParent) {
            Destroy(child.gameObject);
        }

        string path = "";
        string[] pathSplit = currentPath.Split('/');
        foreach (string pathSegment in pathSplit) {
            path += pathSegment;

            string curPath = path;

            GameObject newDirectory = Instantiate(directoryPrefab, directoryParent);
            newDirectory.GetComponent<Button>().onClick.AddListener(() => UpdatePath(curPath));
            newDirectory.GetComponent<TMPro.TextMeshProUGUI>().text = pathSegment;

            //Instantiate(directoryBreadcrumbPrefab, directoryParent);

            path += "/";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)directoryParent);
    }

    public void LoadFiles() {
        List<FileInfo> files = FileSystem.instance.masterNode.GetFilesInDirectory(currentPath);
        List<FileTree> folders = FileSystem.instance.masterNode.GetFoldersInDirectory(currentPath);

        foreach (Transform child in filesParent) {
            Destroy(child.gameObject);
        }

        foreach (FileTree folder in folders) {
            FileManager.instance.NewFolderFile(filesParent, folder.path, folder.fullPath, () => UpdatePath(folder.fullPath));
        }

        foreach (FileInfo file in files) {
            if (file is TextFileInfo) {
                FileManager.instance.NewTextFile(filesParent, currentPath, file as TextFileInfo);
            } else if (file is ImageFileInfo) {
                FileManager.instance.NewImageFile(filesParent, currentPath, file as ImageFileInfo);
            }
        }

    }
}
