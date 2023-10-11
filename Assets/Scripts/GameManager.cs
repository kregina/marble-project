using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Image fadeImage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        FindFadeImage();
    }

    public void LoadSceneWithTransition(string targetScene)
    {
        Debug.Log("Loading scene: " + targetScene);
        StartCoroutine(FadeAndLoadScene(targetScene));
    }

    private void FindFadeImage()
    {
        Debug.Log("Finding fade image");
        GameObject fadeImageObject = GameObject.FindGameObjectWithTag("FadeImage");
        if (fadeImageObject != null)
        {
            fadeImage = fadeImageObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("No fade image found. Please ensure your fade image is tagged with 'FadeImage'.");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindFadeImage();
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade to black
        for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Load the scene
        //SceneManager.LoadScene("LoadingScene");
        //yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(sceneName);

        // Fade from black
        for (float alpha = 1; alpha >= 0; alpha -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
