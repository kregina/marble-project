using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject loseLevelPanel;

    public static LevelManager Instance { get; private set; }

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

    public void Lose()
    {
        loseLevelPanel.SetActive(true);
    }
}