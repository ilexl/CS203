using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] TileLetters tileLetters;
    public List<List<char>> lettersGrid;

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
        //Debug.Log(lettersGrid);
    }

    // Update is called once per frame
    void Update()
    {
        List<TileLetter> allLetters = tileLetters.GetAllLetters();

        foreach(TileLetter t in allLetters)
        {
            if(t.currentPos == null) { continue; }
            Vector3 pos = (Vector3)t.currentPos;
            lettersGrid[(int)pos.y][(int)pos.x] = t.currentLetter;
        }

        
    }
}

