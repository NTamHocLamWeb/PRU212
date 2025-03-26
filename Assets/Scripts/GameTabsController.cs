using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameTabsController : MonoBehaviour
{
    public GameObject gameTabsPanel;
    public bool isOpen = false;
    private static GameTabsController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ToggleVisible()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            gameTabsPanel.SetActive(true);
        }
        else
        {
            gameTabsPanel.SetActive(false);
        }
    }
}
