# CHALLENGE: Implement Data Persistence
[Mission #3](https://learn.unity.com/tutorial/submission-data-persistence-in-a-new-repo) of Unity Learn's Junior Programmer Pathway.

## Key Features
1. Saving data between scenes: a piece of data set in one scene will be stored and used in another scene.
1. Saving data between sessions: a piece of data set during runtime will be saved and used the next time the application is run. 

## Instructions
### Player name (saving data between scenes)
- [ ✔️ ] Create a new Start Menu scene for the game with a text entry field prompting the user to enter their name, and a Start button.
- [ ✔️ ] When the user clicks the Start button, the Main game scene will be loaded and their name will be displayed next to the score. 
### High score (saving data between sessions):
- [ ✔️ ] As the user plays, the current high score will be displayed on the screen alongside the player name who created the score.
- [ ✔️ ] If the high score is beaten, the new score and player name will be displayed instead.
- [ ✔️ ] The highest score will be saved between sessions, so that if the player closes and reopens the application, the high score and player name will be retained.

## Implementation Notes
I added one new scene (*menu*), wrote two new scripts (`MenuUI` & `PlayerDataManager`), and made a few tweaks to `MainManager` to get everything up and running.

### 📃MenuUI
*new script (attached to **Canvas** game object--which holds 4 UI elements: name input field, start button, quit button, best score text--in **menu** scene)*
- `using` directives (apart from the standard namespaces):
    - `UnityEditor` - to access `EditorApplication` class
    - `UnityEngine.UI` - to access `Button` class
    - `UnityEngine.SceneManagement` - to access `SceneManager` class
    - `TMPro` - to access `TMP_InputField` class
- **Methods**
    - `Start`
        - sets start button's `interactable` state to `false` to disable it when scene loads
        - calls `PlayerDataManager`'s `LoadBestScore` method to display/update best score stats in the *Best Score* UI element when scene loads
    - `StartGame` - callback function for start button (on click: starts game by switching to *main* scene)
        - adjusted the following four *Transition* properties of the start button via the Unity Editor to provide player with visual feedback re: start button's `interactable` state: 
            - *Disabled Color* - set to gray, indicates button is not clickable (button's `interactable` property is `false`)
            - *Normal Color* - set to white, button turns from gray to white when enabled (button's `interactable` property is `true`)
            - *Highlighted Color* - set to green, enabled button turns green on hover (for mouse/trackpad users when cursor is over button)
            - *Pressed Color* - set to yellow, enabled button turns yellow on click
    - `SetPlayerName` - callback function for name input field (on value changed: formats/validates the current player's name, sets `PlayerDataManager`'s `playerName` property to the formatted name, and enables start button)
        - how an `OnValueChanged` event is triggered:
            - a key must be pressed within the input field to constitute as a "value changed"--simply clicking inside the field won't do. This is the condition for enabling the start button (i.e., updating its `interactable` property to `true`)
        - how player name is determined when an `OnValueChanged` event is triggered:
            - if input field is BLANK--i.e., player simply presses space bar or deletes all text after typing inside field--player name will be set to "Anonymous" when start button is clicked
            - if input field is NOT BLANK - the input text is passed in to `FormatName` to perform *very basic* input validation before current player's name is set.
    - `FormatName` - outputs a formatted string based on a string input. This method's return value will be used to set `PlayerDataManager`'s `playerName` property. For simplicity--since this is outside the scope of this assignment--method only evaluates two things:
        1. character type: converts non-alphanumeric characters to dashes (to prevent emojis, symbols, white space, etc. from showing up in player name without having to explicitly specify this in the UI and to ask player to reenter a valid string--basically ensures whatever is entered will be valid on first go without additional effort from player)
        2. length: limits player name to 12 characters (To prevent player from entering an excessively long name that may require altering the layouts in both scenes to accommodate it, the end portion of any name exceeding 12 characters will be trimmed off after the 12th character.)
    - `Quit` - callback function for quit ("X") button (on click--closes app or, if in Unity Editor, exits play mode)
        - adjusted the *Normal Color*, *Highlighted Color*, and *Pressed Color* *Transition* properties of the quit button via the Unity Editor 
### 📃 PlayerDataManager
*new script (attached to **PlayerData**--an empty game object--in **menu** scene)*
- `using` directives (apart from the standard namespaces):
    - `System.IO` - to access `File` class
    - `TMPro` - to access `TextMeshProUGUI` class
- **Singleton** - `PlayerDataManager` holds all the data that needs to be shared between the two scenes. 
    - `public static PlayerDataManager Instance;` - enables `MainManager` and `MenuUI` scripts to access properties and methods in `PlayerDataManager` by referencing `PlayerDataManager.Instance` outside of `PlayerDataManager`.
    - `Awake` - code inside this method ensures that only one `PlayerDataManager` instance exists, and any duplicates are destroyed before scene loads
-  `[System.Serializable] public class SaveData` - mini nested class, a template for storing our game stats in a serializable format so that they can be coverted to *json*
- **Properties**
    - Current Player Info 
        - `playerName` - holds the current player's name (based on data from `MenuUI`)
        - `finalScore` - hold's the current player's final score when game is over (based on data from `MainManager`)
    - Game Stats Info
        - `bestPlayer` - initialized to "TBD" for very first game, holds name of player with best score on record
        - `bestScore` - initialized to 0 for very first game so that there is a value to compare the first player's score against, holds best score on record
- **Methods**
    - `SaveBestScore` (accepts 2 arguments)
        - saves updated game stats as a *json* file
        - method is only called if current player's score is higher than the current recordholder--as determined by `MainManager` script when game ends. If current player scores higher, that player's name and final score are passed in to `SaveBestScore`. Otherwise, game stats (namely, `bestScore` and `bestPlayer`) remain as is, since there is no new info to save.
    - `LoadBestScore` 
        - updates `PlayerDataManager`'s own `bestPlayer` and `bestScore` properties based on data on file (saved by `SaveBestScore`), so that in the next game, `MainManager` can compare the `PlayerDataManager`'s `bestScore` value with the next player's `finalScore` value to determine if game stats need to be updated
        - data on file is passed in to `DisplayBestScore` so that when `LoadBestScore` is called in `MainManager` and `MenuUI`, the most updated game stats are displayed in their respective "Best Score" UI.
    - `DisplayBestScore` (accepts 2 arguments) 
        - provides the text to display in the "Best Score" UIs of `MainManager` and `MenuUI`, using the latest `bestScore` and `bestPlayer` values passed in to it from `LoadBestScore`.
### 📃 MainManager
*Updated this script with the following:*
- `Start` - calls `PlayerDataManager`'s `LoadBestScore` method to display the latest best score stats while game is in session
    -  *Best Score* UI element - deactivated legacy `Text` object in *main* scene, changed it to a `TextMeshProGUI` object so that it's consistent with the *Best Score* UI used in *menu* scene--this way, it can also be referenced using the same code. Both are also tagged as "BestScore" in Unity Editor.
- `Update` - calls `ReturnToMainMenu` to allow current player to exit game early/return to main menu
- `GameOver` - calls `CompareScores` to evaluate current player's final score against `PlayerDataManager`'s `bestScore` value when game ends 
- added two new methods:
    - `CompareScores` 
        - `PlayerDataManager`'s `finalScore` value is set to current player's final score. 
        - Current player's final score is compared to the best score on record (i.e., `PlayerDataManager`'s `bestScore` value). The higher of the two scores and the corresponding player's name is saved using `PlayerDataManager`'s `SaveBestScore` method. 
        - `CompareScores` is called in `GameOver` to ensure that scores under consideration for "best score" are based on game completion.
    - `ReturnToMainMenu`
        - switches to *menu* scene when `ESC` key is pressed
        - score for the current session will not be saved or evaluated by `CompareScores` if player leaves *main* (game) scene before game is over.
