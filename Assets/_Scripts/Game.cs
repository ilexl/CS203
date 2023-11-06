using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] TileLetterManager tileLetters;
    public List<List<char>> lettersGrid;
    [SerializeField] Words words;
    [SerializeField] PopUpManager popUpManager;
    [SerializeField] int startingLettersAmount;
    public int extras = 0;
    List<string> allPreviousWords;
    public void PowerUp(string s)
    {
        char c = s[0];
        switch (c)
        {
            case 'Y':
            {
                Debug.LogWarning("Y Power Up Not Implemented Yet!");
                break;
            }
            case 'Q':
            {
                Debug.LogWarning("Q Power Up Not Implemented Yet!");
                break;
            }
            case 'J':
            {
                Debug.LogWarning("J Power Up Not Implemented Yet!");
                break;
            }
            case 'V':
            {
                Debug.LogWarning("V Power Up Not Implemented Yet!");
                break;
            }
            case 'Z':
            {
                Debug.LogWarning("Z Power Up Not Implemented Yet!");
                break;
            }
            case 'X':
            {
                Debug.LogWarning("X Power Up Not Implemented Yet!");
                break;
            }
            case 'P':
            {
                Debug.LogWarning("P Power Up Not Implemented Yet!");
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
        //Debug.Log(transform.gameObject.name);
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

        tileLetters.ResetLettersBag();
        tileLetters.SpawnPlayerLetters(startingLettersAmount);
        tileLetters.Retrieve();
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

        // TODO add a letter count of how many letters are moveable - is this above the players max letters this round?
        // if so confirm with player with "are you sure you want to pass"
        // TODO create pass function

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
            // check if pass then contine else return
            if(allPreviousWords.Count == 0 && allWords.Count == 0)
            {
                // pass here
            }
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
                allPreviousWords = allWords;
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
        RefreshLetters();
    }

    private void RefreshLetters()
    {
        tileLetters.BagPlayersLetters();
        tileLetters.SpawnPlayerLetters(startingLettersAmount);
        tileLetters.Retrieve();
    }
}

