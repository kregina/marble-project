using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu Instance { get; private set; }
    public Animator animator;

    [SerializeField] private TextMeshProUGUI saveButton;

    [HideInInspector]
    public bool gameIsPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Save()
    {
        saveButton.text = "Saving...";
        GameManager.Instance.Save();
        StartCoroutine(SaveIndicatorCoroutine());
    }

    private IEnumerator SaveIndicatorCoroutine()
    {
        yield return new WaitForSeconds(1f);
        saveButton.text = "Save";
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        TransitionToScene(SceneManager.GetActiveScene().name);
    }

    public void TransitionToScene(string sceneName)
    {
        if (gameIsPaused)
        {
            Continue();
        }
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}