using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int level { get; private set; } = 0;
    public int score { get; private set; } = 0;
    public int comboCount { get; private set; } = 0;

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

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }

    public void AddScore(int points)
    {
        score += points;

        if (comboCount > 1)
        {
            score += (comboCount * 2);
            ResetComboCount();
        }
    }

    public void ResetComboCount()
    {
        comboCount = 0;
    }

    public void IncrementComboCount()
    {
        comboCount += 1;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            level = 0;
        }
        if (PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
        }
        else
        {
            score = 0;
        }
    }
}
