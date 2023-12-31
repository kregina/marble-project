using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Level { get; private set; }
    public int Score { get; private set; }
    public int TempScore { get; private set; }
    public int ComboCount { get; private set; }
    public bool GameIsPaused { get; private set; }
    public bool GameIsOver { get; private set; }
    public bool GameIsLevelCleared { get; private set; }
    public bool ShowTutorial { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTutorialBox(bool show)
    {
        ShowTutorial = show;
    }

    public void LevelCleared()
    {
        GameIsOver = false;
        GameIsLevelCleared = true;
        Save();
    }

    public void GameOver()
    {
        GameIsOver = true;
        GameIsLevelCleared = false;
        TempScore = Score;
    }

    public void SetLevel(int newLevel)
    {
        Level = newLevel;
    }

    public void SetScore(int newScore)
    {
        if (ComboCount > 1)
        {
            TempScore += (ComboCount * 2) + newScore;
            ResetComboCount();
        }
        else
        {
            TempScore += newScore;
        }
    }

    public void ResetComboCount()
    {
        ComboCount = 0;
    }

    public void SetComboCount()
    {
        ComboCount += 1;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (GameIsOver)
        {
            GameIsOver = false;
            TempScore = 0;
        }
        else if (GameIsLevelCleared)
        {
            GameIsLevelCleared = false;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Save()
    {
        Score = TempScore;
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        GameIsOver = false;
        GameIsLevelCleared = false;
        if (PlayerPrefs.HasKey("Level"))
        {
            Level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            Level = 0;
        }
        if (PlayerPrefs.HasKey("Score"))
        {
            Score = PlayerPrefs.GetInt("Score");
            TempScore = Score;
        }
        else
        {
            Score = 0;
            TempScore = 0;
        }
    }
}
