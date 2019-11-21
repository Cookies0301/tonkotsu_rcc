using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistantSingleton<T,D> : Singleton<T> where D : new() where T : Singleton<T>
{
    private D savedData;
    private string savePath;

    public D SavedData { get => savedData; set => savedData = value; }

    protected override void Awake() 
    {
        savePath = Application.persistentDataPath + "/gamesave_" + this.GetType().Name;

        base.Awake();

        savedData = LoadData();
    }

    protected override void OnDestroy()
    {
        if(this == PersistantSingleton<T,D>.Instance)
        {
            SaveData();
        }

        base.OnDestroy();        
    }

    protected virtual D LoadData()
    {
        if(File.Exists(savePath))
        {
            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                var data = binaryFormatter.Deserialize(fileStream);
                return (D) data;
            }
        }
        else
        {
            Debug.LogError("No Save File in " + this.name + "Filepath: " + savePath);
            savedData = new D();
            SaveData();

            return savedData;
        }
    }

    protected virtual void SaveData()
    {
        var binaryFormatter = new BinaryFormatter();
        using(var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, this.savedData);
        }

        Debug.Log("Data Saved in " + this.name);
    }
}
