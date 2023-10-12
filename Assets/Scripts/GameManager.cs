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
        Debug.Log($"Adding {points} points to the score.");

        score += points;

        if (comboCount > 1)
        {
            score += comboCount - 1;

            Debug.Log($"comboCount: {comboCount} new score ${score}");
        }
    }

    public void ResetComboCount()
    {
        comboCount = 0;
    }

    public void IncrementComboCount()
    {
        comboCount++;
    }
}
