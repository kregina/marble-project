using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private int currentLevel;

    private void OnMouseDown()
    {
        pauseButton.SetActive(false);
        Menu.Instance.TransitionToScene(sceneName);
        GameManager.Instance.SetLevel(currentLevel);
    }
}
