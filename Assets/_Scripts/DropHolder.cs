using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHolder : MonoBehaviour, IDropHandler
{
    [SerializeField] TileLetterManager tl;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            if (tl.DropHolderHasLetter(this))
            {
                eventData.pointerDrag.GetComponent<DragDrop>().ReturnToPrevPos();
                // do not set if this one already has a letter on it
            }
            else
            {
                eventData.pointerDrag.GetComponent<DragDrop>().Drop(this);
            }
        }
    }
}
