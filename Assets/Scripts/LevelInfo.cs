using UnityEngine;
using TMPro;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // Initial checks for null references.
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        if (levelText == null || scoreText == null)
        {
            Debug.LogError("TextMeshProUGUI references are not assigned in the inspector.");
            return;
        }

        GameManager.Instance.Load();
    }

    private void Update()
    {
        // Update the UI elements with the current level and score.
        if (GameManager.Instance != null && levelText != null && scoreText != null)
        {
            levelText.text = $"Level: {GameManager.Instance.level}";
            scoreText.text = $"Score: {GameManager.Instance.score}";
        }
    }
}