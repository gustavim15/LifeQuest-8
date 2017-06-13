using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;

    void Awake()
    {
        instance = this;
    }

    public void Save(List<Filho> _listFilhos, List<Quest> _listQuests, string _userID)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + _userID + ".dat");
        SceneStatus data = new SceneStatus();

        data.listFilhos = _listFilhos;
        data.listQuests = _listQuests;

        bf.Serialize(file, data);
        file.Close();
    }

    public ListLoaded Load(string _userID)
    {
        if (File.Exists(Application.persistentDataPath + "/" + _userID + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + _userID + ".dat", FileMode.Open);
            SceneStatus data = (SceneStatus)bf.Deserialize(file);
            List<Filho> listFilhos = data.listFilhos;
            List<Quest> listQuests = data.listQuests;
            file.Close();

            ListLoaded listLoaded = new ListLoaded();
            listLoaded.listFilhos = listFilhos;
            listLoaded.listQuests = listQuests;
            return listLoaded;

        }
        else
        {
            print("Arquivo não encontrado.");
            return null;
        }
    }

    public class ListLoaded
    {
        public List<Filho> listFilhos = null;
        public List<Quest> listQuests = null;
    }
}

[Serializable]
class SceneStatus
{
    public List<Filho> listFilhos = null;
    public List<Quest> listQuests = null;
}
