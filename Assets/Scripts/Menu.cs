using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    [SerializeField] GameObject menu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMap()
    {
        LoadSceneWithTransition("Map");
    }

    public void LoadLevel(string enterSceneName)
    {
        LoadSceneWithTransition(enterSceneName);
    }

    public void PauseGame()
    {
        ShowMenu();
    }

    public void BackToMain()
    {
        LoadSceneWithTransition("Main");
    }

    private void LoadSceneWithTransition(string enterSceneName)
    {
        CloseMenu();
        GameManager.Instance.LoadSceneWithTransition(enterSceneName);
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
    }
}
