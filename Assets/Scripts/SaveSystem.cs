﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public static class SaveSystem {

    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/JSON/";
    private const string SAVE_EXTENSION = "txt";

    public static void Init() {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER)) {
            // Create Save Folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString) {
        // Make sure the Save Number is unique so it doesnt overwrite a previous save file
        int saveNumber = 1;
        while (File.Exists(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION)) {
            saveNumber++;
        }
        // saveNumber is unique
        File.WriteAllText(SAVE_FOLDER + "save_" + saveNumber + "." + SAVE_EXTENSION, saveString);
    }

    public static void SaveXML(XmlDocument saveString)
    {
        // Make sure the Save Number is unique so it doesnt overwrite a previous save file
        int saveNumber = 1;
        while (File.Exists(Application.dataPath + "/Saves/XML/" + "saveXML_" + saveNumber + "." + SAVE_EXTENSION))
        {
            saveNumber++;
        }
        // saveNumber is unique
        saveString.Save(Application.dataPath + "/Saves/XML/" + "saveXML_"  + "." + SAVE_EXTENSION);
    }


    public static string Load() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        // Get all save files
        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
        // Cycle through all save files and identify the most recent one
        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null) {
                mostRecentFile = fileInfo;
            } else {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
                    mostRecentFile = fileInfo;
                }
            }
        }

        // If theres a save file, load it, if not return null
        if (mostRecentFile != null) {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        } else {
            return null;
        }
    }

}
