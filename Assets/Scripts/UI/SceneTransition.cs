using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject uiPanel;

    public void Quit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        if (GameManager.Instance.GameIsPaused)
        {
            GameManager.Instance.Continue();
        }
        TransitionToSpecificScene(SceneManager.GetActiveScene().name);
    }

    public void TransitionToSpecificScene(string sceneName)
    {
        if (GameManager.Instance.GameIsPaused)
        {
            GameManager.Instance.Continue();
        }

        LoadSceneCoroutine(sceneName);
    }

    private void LoadSceneCoroutine(string sceneName)
    {
        pauseButton.SetActive(false);
        uiPanel.SetActive(false);
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}