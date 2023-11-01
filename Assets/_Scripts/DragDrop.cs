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
    private DropHolder lastDropHolder = null;
    [SerializeField] float movementResponsiveness = 15.0f;
    [SerializeField] Vector2 offset;

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

        transform.SetAsLastSibling(); //ensure it renders above all other tiles
        actualPosition = GameToCanvas(eventData.position) - offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDropHolder = dropHolder;
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
    //Called by other tiles that it is dragged onto
    public void ReturnToPrevious()
    {
        if (lastDropHolder == null) return;
        dropHolder = lastDropHolder;
        actualPosition = lastDropHolder.transform.GetComponent<RectTransform>().anchoredPosition;
    }

    //Convert from game co ordinates to canvas co ordinates (IE mouse position to tile position)
    //might be worth moving into a helper class since this should work with any canvas of any size
    private Vector2 GameToCanvas(Vector2 screenPos)
    {
        return (screenPos - (Vector2)canvas.transform.position) / canvas.scaleFactor;
    }
    public void Update()
    {
        Move();
    }

    private void Move()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, actualPosition, movementResponsiveness * Time.deltaTime);
    }
    
}
