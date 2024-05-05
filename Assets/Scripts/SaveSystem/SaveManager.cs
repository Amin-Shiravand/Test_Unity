using System.Collections;
using System.Collections.Generic;
using System.IO;
using SaveSystem;
using UnityEngine;
using Utils;

public class SaveManager : Singleton<SaveManager>
{
    private const string FILE_NAME = "BOARD_DATA.json";
    private string filePath;

    public override void Init()
    {
        if( IsInit )
        {
            return;
        }

        base.Init();
        filePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
    }

    public bool IsSaveFileExist()
    {
        Debug.Assert(IsInit, "Firstly,Initialize the file system");
        return File.Exists(filePath);
    }

    public void DeleteSaveData()
    {
        Debug.Assert(IsInit, "Firstly,Initialize the file system");

        if( IsSaveFileExist() )
        {
            File.Delete(filePath);
        }
    }

    public void SaveData( SaveData saveData )
    {
        Debug.Assert(IsInit, "Firstly,Initialize the file system");
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, jsonData);
    }

    public SaveData LoadGameData()
    {
        Debug.Assert(IsInit, "Firstly,Initialize the file system");
        if( !IsSaveFileExist() )
        {
            return null;
        }

        string jsonData = File.ReadAllText(filePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);
        return saveData;
    }
}