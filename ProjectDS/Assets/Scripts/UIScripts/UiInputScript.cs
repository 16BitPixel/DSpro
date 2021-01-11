using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInputScript : MonoBehaviour
{
    public static UiInputScript UIInpScrpt;

      PlayerControls inputActions;
    GameObject canvas;


    private void Start()
    {
        if (UIInpScrpt == null)
        {
            UIInpScrpt = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void invokePause()
    {
       // bool isOpen = false;
       // bool pauseInvoked = inputActions.PlayerActions.Pause.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        /*if (pauseInvoked)
        {
            canvas.SetActive(!isOpen);
            isOpen = !isOpen;
        }*/

    }
}
