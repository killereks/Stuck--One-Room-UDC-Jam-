using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileTree {
    public List<FileTree> subdirectories = new List<FileTree>();
    public List<FileInfo> files = new List<FileInfo>();
    public string path;
    public string fullPath;

    FileTree parentDirectory = null;
    //public int depth = 0;

    public FileTree(string path, string fullPath) {
        this.path = path;
        this.fullPath = fullPath;
    }

    public bool DirectoryExists(string path) {
        string[] splitPath = path.Split('/');

        FileTree curDirectory = this;

        foreach (string curPath in splitPath) {
            bool found = false;
            foreach (FileTree directory in curDirectory.subdirectories) {
                if (directory.path == curPath) {
                    curDirectory = directory;
                    found = true;
                    break;
                }
            }
            if (!found) {
                return false;
            }
        }
        return true;


    }

    public FileTree GetDirectory(string path) {
        if (string.IsNullOrEmpty(path)) {
            return this;
        }

        if (path.Contains("/")) {
            string[] splitPath = path.Split(new char[]{'/'}, 2);

            FileTree node = GetInCurrentDirectory(splitPath[0]);
            if (node != null) {
                return node.GetDirectory(splitPath[1]);
            }
        }

        return GetInCurrentDirectory(path);
    }

    public void MoveDirectory(string from, string to) {
        FileTree fromDir = GetDirectory(from);
        FileTree toDir = GetDirectory(to);

        if (from == null || to == null) {
            throw new System.Exception("MoveDirectory: one of the directories is null path1: " + from + ", path2: " + to);
        }

        FileTree copy = fromDir;
        FileSystem.instance.masterNode.DeleteDirectory(from);

        toDir.subdirectories.Add(copy);
    }

    public void CreateDirectory(string path) {
        if (string.IsNullOrEmpty(path)) return;
        string fullPath = "";

        if (path.Contains("/")) {
            string[] splitPath = path.Split('/');

            FileTree currentDirectory = this;
            foreach (string pathName in splitPath) {
                fullPath += pathName;
                // does the directory exist
                FileTree foundDirectory = currentDirectory.subdirectories.Find(x => x.path == pathName);
                // if not, let's create it and set it as our current directory
                if (foundDirectory == null) {
                    FileTree newDirectory = new FileTree(pathName, fullPath);
                    //newDirectory.depth = currentDirectory.depth + 1;
                    newDirectory.parentDirectory = this;

                    currentDirectory.subdirectories.Add(newDirectory);
                    foundDirectory = newDirectory;
                }
                currentDirectory = foundDirectory;
                fullPath += "/";
            }
        }
    }

    public void AddFile(string path, FileInfo file) {
        FileTree directory = GetDirectory(path);

        if (directory != null) {
            directory.files.Add(file);
        }
    }

    public List<FileInfo> GetFilesInDirectory(string path) {
        FileTree directory = GetDirectory(path);

        if (directory != null) {
            return directory.files;
        }
        return new List<FileInfo>();
    }

    public List<FileTree> GetFoldersInDirectory(string path) {
        FileTree directory = GetDirectory(path);
        if (directory != null) {
            return directory.subdirectories;
        }
        return new List<FileTree>();
    }

    /// <summary>
    /// Gets a specific folder by name inside current directory instance
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public FileTree GetInCurrentDirectory(string name) {
        foreach (FileTree dir in subdirectories) {
            if (dir.path == name) {
                return dir;
            }
        }
        return null;
    }

    public void DeleteDirectory(string path) {
        string[] splitPath = path.Split('/');
        if (splitPath.Length == 0) return;

        string dirName = splitPath[splitPath.Length-1];

        FileTree curDirectory = this;
        for (int i = 0; i < splitPath.Length - 1; i++) {
            if (curDirectory == null) return;

            curDirectory = curDirectory.GetInCurrentDirectory(splitPath[i]);
        }

        if (curDirectory != null) {
            FileTree target = curDirectory.subdirectories.Find(x => x.path == dirName);
            if (target != null) {
                curDirectory.subdirectories.Remove(target);
            }
        }
    }
}
