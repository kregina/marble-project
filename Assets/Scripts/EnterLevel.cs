using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnMouseDown()
    {
        GameManager.Instance.LoadSceneWithTransition(sceneName);
    }


}
