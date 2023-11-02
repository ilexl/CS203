using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TileLetters : MonoBehaviour, IDropHandler
{
    [SerializeField] Transform lettersParent;
    [SerializeField] GameObject prefabLetter;
    [SerializeField] Transform playerLettersBottom;
    [SerializeField] List<int> remainingLettersBag;
    [SerializeField] List<int> defaultLettersBag;

    public void ResetLettersBag()
    {
        remainingLettersBag = defaultLettersBag;
    } 

    public List<TileLetter> GetAllLetters()
    {
        List<TileLetter> allLetters = new List<TileLetter>();
        foreach (Transform t in lettersParent)
        {
            allLetters.Add(t.GetComponent<TileLetter>());
        }
        return allLetters;
    }

    //Called when a tile is dropped on another tile, tell tile to go back home (IDropHandler gives this behaviour)
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<DragDrop>().ReturnToPrevious();
    }

    public int PlayersCurrentLetters()
    {
        int count = 0;
        foreach(Transform t in lettersParent)
        {
            TileLetter tileLetter;
            if(t.TryGetComponent<TileLetter>(out tileLetter))
            {
                if (tileLetter.GetComponent<DragDrop>().enabled)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void SpawnPlayerLetters(int most)
    {
        int currentAmount = PlayersCurrentLetters();
        while(currentAmount < most)
        {
            SpawnLetter();
            currentAmount = PlayersCurrentLetters();
        }
    }

    private void SpawnLetter()
    {
        GameObject newLetter = Instantiate(prefabLetter, lettersParent);
        char c = RandomLetter();
        //Debug.Log((int)c);
        newLetter.GetComponentInChildren<TextMeshProUGUI>().text = c.ToString();

    }

    public int RemainingLetters()
    {
        int remainingLetters = 0;
        foreach (int test in remainingLettersBag)
        {
            remainingLetters += test;
        }

        return remainingLetters;
    }

    private char RandomLetter()
    {
        int remaining = RemainingLetters();
        char c = ' ';
        if(remaining <= 0)
        {
            return c;
        }

        while (c == ' ')
        {
            int rand = Random.Range(0, 26);
            if(remainingLettersBag[rand] > 0)
            {
                c = (char)(65 + rand);
                remainingLettersBag[rand] -= 1;
            }
        }
        return c;
    }

}
