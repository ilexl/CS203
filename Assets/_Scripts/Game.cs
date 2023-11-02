using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] TileLetters tileLetters;
    public List<List<char>> lettersGrid;
    [SerializeField] Words words;
    [SerializeField] int startingLettersAmount;
    public int extras = 0;

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
            tileLetters = gameObject.GetComponentInChildren<TileLetters>();
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

        tileLetters.ResetLettersBag();
        tileLetters.SpawnPlayerLetters(startingLettersAmount);
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
        foreach (string word in allWords)
        {
            bool isWord = words.isWord(word);
            //Debug.Log(word + " == " + isWord);
            if (!isWord) { allValid = false; }
        }

        Debug.Log("All words valid == " + allValid.ToString());

        if (allValid)
        {
            // stop all letters played from moving if valid - no longer players letters as played
            List<TileLetter> allLetters = tileLetters.GetAllLetters();
            foreach (TileLetter t in allLetters)
            {
                if (t.currentPos == null) { continue; }
                if (t.GetDragDrop().dropHolder == null) { continue; }
                t.SetPlayable(false);

                tileLetters.SpawnPlayerLetters(startingLettersAmount + extras);
            }
        }
        else
        {
            // exit and do NOT play this round
            // inform user with pop up
        }
    }
}

