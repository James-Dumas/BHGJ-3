using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public MenuButtonType ButtonType;
    public GameObject MainMenu;
    public GameObject HelpMenu;

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
                SceneManager.LoadScene("Level 1");
                break;
            
            case MenuButtonType.HELP:
                MainMenu.SetActive(false);
                HelpMenu.SetActive(true);
                break;

            case MenuButtonType.HELP_BACK:
                MainMenu.SetActive(true);
                HelpMenu.SetActive(false);
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