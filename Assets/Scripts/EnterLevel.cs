using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private int levelToSet;

    private SceneTransition sceneTransition;

    private void Start()
    {
        sceneTransition = GameObject.FindWithTag("SceneTransition").GetComponent<SceneTransition>();
    }

    private void OnMouseDown()
    {
        sceneTransition.TransitionToSpecificScene(sceneName);
        GameManager.Instance.SetLevel(levelToSet);
    }
 }
