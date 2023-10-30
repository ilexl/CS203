using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;
    public DropHolder dropHolder = null;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        if(canvas == null) { Debug.LogError("No Canvas Found..."); }
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = (eventData.pressPosition - (Vector2)canvas.transform.position) / canvas.scaleFactor;
        dropHolder = null;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }

    public void Drop(DropHolder DH)
    {
        dropHolder = DH;
        rectTransform.anchoredPosition = DH.transform.GetComponent<RectTransform>().anchoredPosition;
    }
}
