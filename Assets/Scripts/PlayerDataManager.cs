using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
   public static PlayerDataManager Instance;

    // current player
    public string playerName;

    // game data
    public string bestPlayer;
    public int bestScore;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class SaveData {
        public string bestPlayer;
        public int bestScore;
    }

}
