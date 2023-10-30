using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileLetters : MonoBehaviour, IDropHandler
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

    //Called when a tile is dropped on another tile, tell tile to go back home (IDropHandler gives this behaviour)
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<DragDrop>().ReturnToPrevious();
    }
}
