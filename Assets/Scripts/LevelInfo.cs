using UnityEngine;
using TMPro;

public class LevelInfo: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        levelText.text = $"Level: {GameManager.Instance.level}";
    }
}