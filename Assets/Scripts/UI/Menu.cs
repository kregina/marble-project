using System.Collections;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveButton;


    public void Continue()
    {
        GameManager.Instance.Continue();
    }

    public void Pause()
    {
        GameManager.Instance.Pause();
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

}