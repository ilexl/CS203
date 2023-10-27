using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLetters : MonoBehaviour
{
    [SerializeField] Transform lettersParent;

    public List<TileLetter> GetAllLetters()
    {
        List<TileLetter> allLetters = new List<TileLetter>();
        foreach (Transform t in lettersParent)
        {
            allLetters.Add(t.GetComponent<TileLetter>());
        }
        return allLetters;
    }
}
