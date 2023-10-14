using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void ReloadScene()
    {
        GameManager.Instance.Continue();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TransitionToSpecificScene(string sceneName)
    {
        GameManager.Instance.Continue();
        LoadSceneCoroutine(sceneName);
    }

    private void LoadSceneCoroutine(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}