using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

public class TileLetterManager : MonoBehaviour, IDropHandler
{
    [SerializeField] Transform lettersParent;
    [SerializeField] GameObject prefabLetter;
    [SerializeField] Transform playerLettersBottom;
    [SerializeField] List<int> remainingLettersBag;
    [SerializeField] List<int> defaultLettersBag;
    [SerializeField] Transform letterStartRect;
    [SerializeField] Canvas canvas;
    [SerializeField] Vector2 StartPosOffset;

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

    public void OnDrop(PointerEventData eventData) { }

    public int PlayersCurrentLetters()
    {
        int count = 0;
        foreach(Transform t in lettersParent)
        {
            TileLetter tileLetter;
            if(t.TryGetComponent(out tileLetter))
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

        newLetter.GetComponent<DragDrop>().actualPosition = RandomPosition(newLetter.transform);
        newLetter.GetComponent<TileLetter>().SetPlayable(true);
    }

    public Vector2 RandomPosition(Transform newLetter)
    {
        RectTransform rectTransform = letterStartRect.GetComponent<RectTransform>();
        newLetter.SetParent(letterStartRect, false);

        Vector2 newPos = StartPosOffset;
        newPos += new Vector2(Random.Range(-(rectTransform.rect.width / 2.2f) , (rectTransform.rect.width / 2.2f)),Random.Range(-(rectTransform.rect.height / 4), 0));
        //Debug.Log(newPos);
        newLetter.SetParent(lettersParent, false);
        return newPos;
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

    public bool DropHolderHasLetter(DropHolder dh)
    {
        foreach(Transform letter in lettersParent)
        {
            TileLetter t;
            if(letter.TryGetComponent(out t))
            {
                if(t.GetDragDrop().dropHolder == dh)
                {
                    return true;
                }
            }

        }
        return false;
    }

    public void RayCastSetAllLetters(bool set)
    {
        foreach (Transform letter in lettersParent)
        {
            TileLetter t;
            if (letter.TryGetComponent(out t))
            {
                t.GetDragDrop().RayCastSet(set);
            }

        }
    }



    public void Retrieve()
    {
        for (int i = PlayersCurrentLetters() + 1; i > 0; i--) // no idea why this makes it work but its a easy and dirty fix
        {
            foreach (Transform letter in lettersParent)
            {
            
                TileLetter t;
                if (letter.TryGetComponent(out t))
                {
                    if (t.GetPlayable())
                    {
                        t.GetComponent<DragDrop>().OnBeginDrag(null);
                        t.GetComponent<DragDrop>().OnEndDrag(null);
                        if (t.GetDragDrop().dropHolder != null)
                        {
                            t.ResetDropHolder();
                        }
                        t.GetComponent<DragDrop>().actualPosition = RandomPosition(t.transform);
                    }
                }
            }
        }
    }
}
