using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] PopUp[] allPopUps;
    public void ShowPopUp(PopUp popUp, string message)
    {
        popUp.NewPopUp(message);
    }
    public void ShowPopUp(PopUp popUp, string message, bool autoHide)
    {
        popUp.NewPopUp(message, autoHide);
    }

    public void ShowPopUp(int popUp, string message)
    {
        allPopUps[popUp].NewPopUp(message);
    }
    public void ShowPopUp(int popUp, string message, bool autoHide)
    {
        allPopUps[popUp].NewPopUp(message, autoHide);
    }

    public void ShowPopUp(PopUp popUp, string message, float duration)
    {
        popUp.NewPopUp(message);
        popUp.SetDuration(duration);
    }
    public void ShowPopUp(PopUp popUp, string message, float duration, bool autoHide)
    {
        popUp.NewPopUp(message, autoHide);
        popUp.SetDuration(duration);
    }

    public void ShowPopUp(int popUp, string message, float duration)
    {
        allPopUps[popUp].NewPopUp(message);
        allPopUps[popUp].SetDuration(duration);
    }
    public void ShowPopUp(int popUp, string message, float duration, bool autoHide)
    {
        allPopUps[popUp].NewPopUp(message, autoHide);
        allPopUps[popUp].SetDuration(duration);
    }

    public void ShowPopUp(PopUp popUp, bool autoHide)
    {
        popUp.NewPopUp(autoHide);
    }
    public void ShowPopUp(PopUp popUp, float duration, bool autoHide)
    {
        popUp.NewPopUp(autoHide);
        popUp.SetDuration(duration);
    }
    public void ShowPopUp(PopUp popUp, float duration)
    {
        popUp.NewPopUp();
        popUp.SetDuration(duration);
    }

    public void ShowPopUp(PopUp popUp)
    {
        popUp.NewPopUp();
    }

    public void ShowPopUp(int popUp, bool autoHide)
    {
        allPopUps[popUp].NewPopUp(autoHide);
    }
    public void ShowPopUp(int popUp, float duration, bool autoHide)
    {
        allPopUps[popUp].NewPopUp(autoHide);
        allPopUps[popUp].SetDuration(duration);
    }
    public void ShowPopUp(int popUp, float duration)
    {
        allPopUps[popUp].NewPopUp();
        allPopUps[popUp].SetDuration(duration);
    }

    public void ShowPopUp(int popUp)
    {
        allPopUps[popUp].NewPopUp();
    }
}
