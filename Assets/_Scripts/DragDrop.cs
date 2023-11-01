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
        actualPosition = rectTransform.position;
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); //ensure it renders above all other tiles
        actualPosition = GameToCanvas(eventData.position) - offset;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); //ensure it renders above all other tiles
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
        actualPosition = DH.transform.GetComponent<RectTransform>().position;
    }
    //Called by other tiles that it is dragged onto
    public void ReturnToPrevious()
    {
        if (lastDropHolder == null) return;
        dropHolder = lastDropHolder;
        actualPosition = lastDropHolder.transform.GetComponent<RectTransform>().position;
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        rectTransform.position = Vector2.Lerp(rectTransform.position, actualPosition, movementResponsiveness * Time.deltaTime);
    }
    
}
