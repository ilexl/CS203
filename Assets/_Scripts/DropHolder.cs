using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHolder : MonoBehaviour, IDropHandler
{
    [SerializeField] TileLetterManager tl;
    [SerializeField] Game game;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!game.localTurn)
            {
                eventData.pointerDrag.GetComponent<DragDrop>().ReturnToPrevPos();
                game.popUpManager.ShowPopUp(6);
                return;
            }
            if (tl.DropHolderHasLetter(this))
            {
                if (eventData.pointerDrag.GetComponent<DragDrop>().playable)
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().ReturnToPrevPos();
                }
                // do not set if this one already has a letter on it
            }
            else
            {
                if (eventData.pointerDrag.GetComponent<DragDrop>().playable)
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().Drop(this);
                }
            }
        }
    }
}
