using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Game : MonoBehaviour
{
    public Board board;
    [SerializeField] TileLetterManager tileLetters;
    public List<List<char>> lettersGrid;
    [SerializeField] Words words;
    public PopUpManager popUpManager;
    [SerializeField] Score score;
    [SerializeField] int startingLettersAmount;
    [SerializeField] Button playButton;
    List<string> allPreviousWords;
    int pointsMultiplier = 1;
    public bool localTurn = false;
    [SerializeField] WindowManager mainWindowManager;
    [SerializeField] Window mainMenuWindow;
    [SerializeField] NetworkManager networkManager;
    public void PowerUp(string s)
    {
        // TODO - BLOCKED(Multiplayer) : Y,V
        if (!localTurn) 
        { 
            popUpManager.ShowPopUp(6);
            return; 
        }

        char c = s[0];
        switch (c)
        {
            case 'Y':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        Debug.LogWarning("Y Power Up Not Implemented Yet!");
                        tileLetters.RemovePlayerLetter(c);
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                break;
                }
            case 'Q':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        string bestWord = tileLetters.BestPossiblePlay();
                        string message = "The BEST word you can make is : " + bestWord;
                        popUpManager.ShowPopUp(3, message);
                        tileLetters.RemovePlayerLetter(c);
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    break;
                }
            case 'J':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        pointsMultiplier += 2;
                        tileLetters.RemovePlayerLetter(c);
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    break;
                }
            case 'V':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        tileLetters.RemovePlayerLetter(c);
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    Debug.LogWarning("V Power Up Not Implemented Yet!");
                    break;
                }
            case 'Z':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        tileLetters.RemovePlayerLetter(c);
                        pointsMultiplier += 3;
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    break;
                }
            case 'X':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        tileLetters.RemovePlayerLetter(c);
                        tileLetters.SpawnPlayerLetters(startingLettersAmount);
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    break;
                }
            case 'P':
                {
                    if (tileLetters.PlayerHasLetter(c))
                    {
                        RefreshLetters();
                    }
                    else
                    {
                        popUpManager.ShowPopUp(2);
                    }
                    break;
                }
            default:
                {
    #if UNITY_EDITOR
                    Debug.LogError("No such power up - " + c);
    #endif
                    break;
                }

        }
    }

    private void Awake()
    {
        #region config-chks
        if (board == null)
        {
            board = gameObject.GetComponentInChildren<Board>();
        }
        if (board == null)
        {
            Debug.LogError("Board not found...");
        }
        if (tileLetters == null)
        {
            tileLetters = gameObject.GetComponentInChildren<TileLetterManager>();
        }
        if (tileLetters == null)
        {
            Debug.LogError("TileLetters not found...");
        }
        if (popUpManager == null)
        {
            popUpManager = gameObject.GetComponentInChildren<PopUpManager>();
        }
        if (popUpManager == null)
        {
            Debug.LogError("PopUpManager not found...");
        }
        if (score == null)
        {
            score = gameObject.GetComponentInChildren<Score>();
        }
        if (score == null)
        {
            Debug.LogError("Score not found...");
        }
        if (words == null)
        {
            words = gameObject.GetComponentInChildren<Words>();
        }
        if (words == null)
        {
            Debug.LogError("Words not found...");
        }
        #endregion
        
        lettersGrid = new List<List<char>>();
        for (int i = 0; i < board.BoardSize * 2; i++)
        {
            lettersGrid.Add(new List<char>());
            for (int j = 0; j < board.BoardSize * 2; j++)
            {
                lettersGrid[i].Add(' ');
            }
        }

        allPreviousWords = new List<string>();
        pointsMultiplier = 1;
        LocalCanPlay(false);

        tileLetters.ResetLettersBag();
        tileLetters.SpawnPlayerLetters(startingLettersAmount);
        tileLetters.Retrieve();

        score.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        List<TileLetter> allLetters = tileLetters.GetAllLetters();

        // refresh board
        for(int i = 0; i < board.BoardSize * 2; i++)
        {
            for (int j = 0; j < board.BoardSize * 2; j++)
            {
                lettersGrid[i][j] = ' ';
            }
        }
        foreach(TileLetter t in allLetters)
        {
            if(t.currentPos == null) { continue; }
            Vector3 pos = (Vector3)t.currentPos;
            lettersGrid[(int)pos.y][(int)pos.x] = t.currentLetter;
        }
    }

    public void Play()
    {
        List<string> allWords = words.ConvertCharBoardToWords(lettersGrid);
        bool allValid = true;

        // check all words on board are valid
        string lastInvalidWord = "";
        foreach (string word in allWords)
        {
            //Debug.Log(word);
            bool isWord = words.isWord(word);
            //Debug.Log(word + " == " + isWord);
            if (!isWord) 
            { 
                allValid = false;
                lastInvalidWord = word;
            }
        }

        //Debug.Log("All words valid == " + allValid.ToString());

        if (allValid)
        {
            bool newPlay = false;
            foreach(string word in allWords)
            {
                if (!allPreviousWords.Contains(word))
                {
                    newPlay = true;
                }
            }
            
            if(newPlay)
            {
                // stop all letters played from moving if valid - no longer players letters as played
                List<TileLetter> allLetters = tileLetters.GetAllLetters();
                foreach (TileLetter t in allLetters)
                {
                    if (t.currentPos == null) { continue; }
                    if (t.GetDragDrop().dropHolder == null) { continue; }
                    t.SetPlayable(false);

                    tileLetters.SpawnPlayerLetters(startingLettersAmount);
                }
                int scoreToAdd = 0;
                foreach (string word in allWords)
                {
                    if (!allPreviousWords.Contains(word))
                    {
                        scoreToAdd += Score.CalculateBaseScore(word) * pointsMultiplier;
                    }
                }
                score.ChangeOwnScore(scoreToAdd);

                pointsMultiplier = 1;
                allPreviousWords = allWords;
                LocalCanPlay(false);

                // Play here - server code
                networkManager.ClientPlays(lettersGrid, scoreToAdd);
            }
            else
            {
                popUpManager.ShowPopUp(1);
            }

        }
        else
        {
            string message = "Invalid Word!\n" + lastInvalidWord;
            popUpManager.ShowPopUp(0, message);
        }
    }

    public void Pass()
    {
        LocalCanPlay(false);
        RefreshLetters();

        networkManager.ClientPlays(lettersGrid, 0);
    }

    private void RefreshLetters()
    {
        tileLetters.BagPlayersLetters();
        Invoke(nameof(SpawnPLettersDelayed), 0.25f); // Not sure why but the letters spawn were being deleted so dirty fix with a delay...
    }

    private void SpawnPLettersDelayed()
    {
        tileLetters.SpawnPlayerLetters(startingLettersAmount);
        tileLetters.Retrieve();
    }

    public void OppPlay(List<List<char>> lettersGridData, int oppPoints)
    {
        List<string> allWords = words.ConvertCharBoardToWords(lettersGridData);

        for(int i = 0; i < lettersGridData.Count; i++)
        {
            for (int j = 0; j < lettersGridData[i].Count; j++)
            {
                if (lettersGrid[i][j] != lettersGridData[i][j])
                {
                    tileLetters.SpawnOppLetterOnBoard(lettersGridData[i][j], j, i);
                }
            }
        }


        bool newPlay = false;
        foreach (string word in allWords)
        {
            if (!allPreviousWords.Contains(word))
            {
                newPlay = true;
            }
        }

        if (newPlay)
        {
            // stop all letters played from moving if valid - no longer players letters as played
            List<TileLetter> allLetters = tileLetters.GetAllLetters();
            foreach (TileLetter t in allLetters)
            {
                if (t.currentPos == null) { continue; }
                if (t.GetDragDrop().dropHolder == null) { continue; }
                t.SetPlayable(false);

            }
            score.ChangeOppScore(oppPoints);

            allPreviousWords = allWords;

            // Show your turn
            popUpManager.ShowPopUp(4);
            LocalCanPlay(true);
        }
        else
        {
            // Show Opp passed popup
            popUpManager.ShowPopUp(5);
            LocalCanPlay(true);
        }
        

    }

    public void LocalCanPlay(bool play)
    {
        localTurn = play;
        playButton.interactable = play;
    }

    public void GameOver(string win)
    {
        string message = "Game Over...\n" + win + " Win!!!";
        popUpManager.ShowPopUp(7, message);
        Invoke(nameof(ShowMainMenu), 5);
    }

    void ShowMainMenu()
    {
        mainWindowManager.ShowWindow(mainMenuWindow);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Game))]
public class EDITOR_Game : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        Game game = (Game)target;

        if(GUILayout.Button("Allow Local To Play"))
        {
            game.LocalCanPlay(true);
        }
        if(GUILayout.Button("Test"))
        {
            List<List<char>> test = new List<List<char>>();
            for(int r = 0; r < 10; r++)
            {
                var row = new List<char>();
                test.Add(row);
                for(int i =0; i<10; i++)
                {
                    row.Add(' ');
                }
            }

            test[0][1] = 'T';
            test[0][2] = 'E';
            test[0][3] = 'S';
            test[0][4] = 'T';

            game.OppPlay(test, 10);
        }
        if(GUILayout.Button("GameOver"))
        {
            game.GameOver("YOU");
        }
    }
}
#endif

