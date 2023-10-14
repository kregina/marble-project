using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameTitle;
    [SerializeField] private TextMeshProUGUI menuHeader;
    [SerializeField] private TextMeshProUGUI moreLevelsText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private GameObject uiPanel;

    void Start()
    {
        GameManager.Instance.Load();
        uiPanel.SetActive(false);
    }

    private void Update()
    {
        levelText.text = $"Level: {GameManager.Instance.Level}";
        scoreText.text = $"Score: {GameManager.Instance.TempScore}";

        if (GameManager.Instance.GameIsOver)
        {
            gameTitle.text = "Game Over!!!";
            menuHeader.text = "Menu";

            uiPanel.SetActive(true);
            continueButton.gameObject.SetActive(false);
            tryAgainButton.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.GameIsLevelCleared)
        {
            gameTitle.text = "Level Cleared!!!";
            menuHeader.text = "Menu";
            uiPanel.SetActive(true);

            continueButton.gameObject.SetActive(false);
            moreLevelsText.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        uiPanel.SetActive(false);
    }
}