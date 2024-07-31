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
    Button startButton;
    void Start()
    {
        startButton = GameObject.FindWithTag("StartButton").GetComponent<Button>();
        startButton.interactable = false; // disable start button on start
        PlayerDataManager.Instance.LoadBestScore();
    }

    // START BUTTON - on click function ------------------------------------------------
    [SerializeField]
    void StartGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log($"Current Player: {PlayerDataManager.Instance.playerName}");
    }

    // INPUT FIELD - on change function -----------------------------------------------------
    // player must interact with--i.e., type something into--input field to enable start button
    // name defaults to "Anonymous" if player inputs an empty string (e.g., white space)
    [SerializeField]
    void SetPlayerName()
    {
        TMP_InputField inputField = GameObject.FindFirstObjectByType<TMP_InputField>();
        string name = string.IsNullOrWhiteSpace(inputField.text) ? "Anonymous" : FormatName(inputField.text);
        PlayerDataManager.Instance.playerName = name;

        startButton.interactable = true;
    }

    string FormatName(string name)
    {
        string formattedName = "";

        // replace non-alphanumeric chars (emojis, spaces, symbols, etc.) with dashes
        foreach (char letter in name)
        {
            if (!char.IsLetterOrDigit(letter))
            {
                formattedName += '-';
            }
            else
            {
                formattedName += letter;
            }
        }

        //  truncate input if it exceeds 12 chars
        formattedName = formattedName.Length > 12 ? formattedName[..12] : formattedName;

        return formattedName;
    }

    // CLOSE ('x') BUTTON - on click function ----------------------------------------------
    [SerializeField]
    void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
