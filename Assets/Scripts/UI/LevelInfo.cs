using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameTitle;
    [SerializeField] private TextMeshProUGUI menuHeader;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject uiPanel;

    private SceneTransition sceneTransition;

    void Start()
    {
        sceneTransition = GameObject.FindWithTag("SceneTransition").GetComponent<SceneTransition>();
        GameManager.Instance.Load();
    }

    private void Update()
    {
        levelText.text = $"Level: {GameManager.Instance.Level}";
        scoreText.text = $"Score: {GameManager.Instance.TempScore}";

        if (GameManager.Instance.GameIsOver)
        {
            GameIsOver();
        }
        else if (GameManager.Instance.GameIsLevelCleared)
        {
            gameTitle.text = "Level Cleared!!!";
            HandleUI();
        }
    }

    private void GameIsOver()
    {
        gameTitle.text = "Game Over!!!";
        menuHeader.text = "Menu";

        continueButton.gameObject.SetActive(false);
        tryAgainButton.gameObject.SetActive(true);
        HandleUI();
    }

    private void HandleUI()
    {
        pauseButton.gameObject.SetActive(false);
        uiPanel.SetActive(true);
    }
}