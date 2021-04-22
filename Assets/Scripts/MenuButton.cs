using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public MenuButtonType ButtonType;

    private Text text;

    void Start()
    {
    }

    public void OnPointerEnter(PointerEventData e)
    {
        transform.localScale = new Vector3(1.05f, 1.05f, 1f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerClick(PointerEventData e)
    {
        switch(ButtonType)
        {
            case MenuButtonType.START:
                break;
            
            case MenuButtonType.HELP:
                break;

            case MenuButtonType.QUIT:
                Application.Quit();
                break;
            
            default:
                break;
        }
    }
}

public enum MenuButtonType
{
    NONE,
    START,
    HELP,
    QUIT,
    HELP_BACK,
    LEVELS_BACK,
    LEVEL_START
}