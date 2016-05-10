using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataManagement : MonoBehaviour
{
    public static DataManagement data;
    public string fileName = "DataManager";

    public int[] savedDandelions = new int[3];
    public int[] biggestFollowerAmount = new int[3];
    public int[] topScore = new int[3];
    public bool[] levelUnlocked = new bool[3];
    public int levelNumber;

    void Awake()
    {
        if(data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if(data != this)
        {
            Destroy(gameObject);
        }

        Load();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");

        GameData data = new GameData();
        data.savedDandelions = savedDandelions;
        data.biggestFollowerAmount = biggestFollowerAmount;
        data.topScore = topScore;
        data.levelUnlocked = levelUnlocked;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);

            file.Close();

            savedDandelions = data.savedDandelions;
            biggestFollowerAmount = data.biggestFollowerAmount;
            topScore = data.topScore;
            levelUnlocked = data.levelUnlocked;
        }
    }

}

[Serializable]
class GameData
{
    public int[] savedDandelions = new int[3];
    public int[] biggestFollowerAmount = new int[3];
    public int[] topScore = new int[3];
    public bool[] levelUnlocked = new bool[3];
}
