using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    [SerializeField] float timeOnScreen;
    [SerializeField] bool autoHide = true;
    [SerializeField] TextMeshProUGUI textPro;
    float internalTimer = 0f;

    public void NewPopUp(string text)
    {
        textPro.text = text;
        internalTimer = timeOnScreen;
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }
    public void NewPopUp()
    {
        internalTimer = timeOnScreen;
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }

    public void NewPopUp(bool autoHide)
    {
        this.autoHide = autoHide;
        internalTimer = timeOnScreen;
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetDuration(float duration)
    {
        internalTimer = duration;
    }

    public void NewPopUp(string text, bool autoHide)
    {
        this.autoHide = autoHide;
        textPro.text = text;
        internalTimer = timeOnScreen;
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        internalTimer -= Time.deltaTime;
        if (internalTimer <= 0f)
        {
            if (autoHide)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
