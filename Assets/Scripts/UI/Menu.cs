using System.Collections;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saveButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject uiPanel;

    public void Continue()
    {
        ToggleUI();
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

    private void ToggleUI()
    {
        pauseButton.SetActive(true);
        uiPanel.SetActive(false);
    }

    private IEnumerator SaveIndicatorCoroutine()
    {
        yield return new WaitForSeconds(1f);
        saveButton.text = "Save";
    }

}