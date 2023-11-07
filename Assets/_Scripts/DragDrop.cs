using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector2 actualPosition = Vector2.zero;
    [SerializeField] Canvas canvas;
    public DropHolder dropHolder = null;
    private Vector2 startPos = Vector2.zero;
    private DropHolder lastDropHolder = null;
    public float movementResponsiveness = 15.0f;
    [SerializeField] TileLetterManager tileLettersMAIN;
    public bool playable = true;

    private void Awake()
    {
        playable = true;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        if (canvas == null) { Debug.LogError("No Canvas Found..."); }

        tileLettersMAIN = FindFirstObjectByType<TileLetterManager>();
        if(tileLettersMAIN == null) { Debug.LogError("No MAIN TileLetters Found..."); }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (playable)
        {
            actualPosition += eventData.delta / canvas.scaleFactor;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (playable)
        {
            startPos = rectTransform.anchoredPosition;
            transform.SetAsLastSibling(); //ensure it renders above all other tiles
            lastDropHolder = dropHolder;
            dropHolder = null;
            canvasGroup.blocksRaycasts = false; 
            tileLettersMAIN.RayCastSetAllLetters(false);
            canvasGroup.alpha = 0.75f;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (playable)
        {
            canvasGroup.blocksRaycasts = true;
            tileLettersMAIN.RayCastSetAllLetters(true);
            canvasGroup.alpha = 1f;
        }
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
        actualPosition = DH.transform.GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, actualPosition, movementResponsiveness * Time.deltaTime);
    }

    public void ReturnToPrevPos()
    {
        actualPosition = startPos;
        if(lastDropHolder != null) { Drop(lastDropHolder); }
    }

}
