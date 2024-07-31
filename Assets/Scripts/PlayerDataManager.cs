using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{

    // SINGLETON - make PlayerDataManager object accessible from other scripts (MainManger & MenuUI)
    public static PlayerDataManager Instance;

    // current player info
    public string playerName;
    public int finalScore;

    // game stats
    public string bestPlayer = "TBD";
    public int bestScore = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class SaveData
    {
        public string bestPlayer;
        public int bestScore;
    }

    // call in MainManager when game is over
    public void SaveBestScore(int score, string playerName)
    {
        // create an instance of SaveData
        SaveData data = new SaveData();
        data.bestScore = score;
        data.bestPlayer = playerName;

        // convert instance to JSON
        string json = JsonUtility.ToJson(data);

        // store json data to file
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    // call in Start() function of MainManager & MenuUI
    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            // read data
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // update game stats
            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;
        }
        DisplayBestScore(bestPlayer, bestScore);
    }

    void DisplayBestScore(string bestPlayer, int bestScore)
    {
        GameObject.FindWithTag("BestScore")
                  .GetComponent<TextMeshProUGUI>()
                  .text = $"Best Score: {bestScore}\nPlayer: {bestPlayer}";
    }

}
