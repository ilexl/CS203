using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 actualPosition = Vector2.zero;
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
        actualPosition = rectTransform.anchoredPosition;
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        actualPosition = GameToCanvas(eventData.position);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
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
        actualPosition = DH.transform.GetComponent<RectTransform>().anchoredPosition;
    }
}
