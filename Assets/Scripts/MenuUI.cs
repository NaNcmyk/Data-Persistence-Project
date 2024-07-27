using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{
    public TMP_Text playerName;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SetUserName()
    {
        PlayerDataManager.Instance.playerName = playerName.text;
        print(PlayerDataManager.Instance.playerName);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
