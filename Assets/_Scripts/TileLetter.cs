using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileLetter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] DragDrop dragDrop;
    public char currentLetter;
    public Vector3? currentPos = null;
    [SerializeField] bool playable;

    // Update is called once per frame
    void Update()
    {
        currentLetter = text.text[0];
        if(dragDrop.dropHolder == null) { currentPos = null; }
        else { currentPos = dragDrop.dropHolder.transform.GetComponent<BoardPart>().realPos; }
    }

    public void ResetDropHolder()
    {
        dragDrop.dropHolder = null;
    }
    public void SetPlayable(bool p)
    {
        playable = p;
        dragDrop.enabled = playable;
    }

    public bool GetPlayable() { return playable; }

    public DragDrop GetDragDrop() { return dragDrop; }
}
