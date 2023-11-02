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
    private Vector2 startPos = Vector2.zero;
    private DropHolder lastDropHolder = null;
    public float movementResponsiveness = 15.0f;
    [SerializeField] TileLetters tileLettersMAIN;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        if (canvas == null) { Debug.LogError("No Canvas Found..."); }
        actualPosition = rectTransform.position;

        tileLettersMAIN = FindFirstObjectByType<TileLetters>();
        if(tileLettersMAIN == null) { Debug.LogError("No MAIN TileLetters Found..."); }
    }


    public void OnDrag(PointerEventData eventData)
    {
        actualPosition = eventData.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;
        transform.SetAsLastSibling(); //ensure it renders above all other tiles
        lastDropHolder = dropHolder;
        dropHolder = null;
        canvasGroup.blocksRaycasts = false; // TODO Set all letters to block
        tileLettersMAIN.RayCastSetAllLetters(false);
        canvasGroup.alpha = 0.75f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; // TODO Set all letters to unblock
        tileLettersMAIN.RayCastSetAllLetters(true);
        canvasGroup.alpha = 1f;
    }

    public void RayCastSet(bool set)
    {
        canvasGroup.blocksRaycasts = set;
    }

    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }

    public void Drop(DropHolder DH)
    {
        dropHolder = DH;
        actualPosition = DH.transform.GetComponent<RectTransform>().position;
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        rectTransform.position = Vector2.Lerp(rectTransform.position, actualPosition, movementResponsiveness * Time.deltaTime);
    }

    public void ReturnToPrevPos()
    {
        actualPosition = startPos;
        if(lastDropHolder != null) { Drop(lastDropHolder); }
    }

}
