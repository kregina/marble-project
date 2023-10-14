using System.Collections;
using System.Linq;
using UnityEngine;

public class Match3Manager : MonoBehaviour
{
    [SerializeField] private Wave wave;
    
    private AudioSource explosionSound;

    void Start()
    {
        explosionSound = GetComponent<AudioSource>();
        wave = GetComponent<Wave>();
        wave.OnMarblesChanged += OnMarblesChanged;
    }

    private void OnMarblesChanged(Wave wave, int originIndex, bool isTriggeredByPlayer)
    {
        if (wave == null || !wave.gameObject.activeSelf) return;
        StopAllCoroutines();
        StartCoroutine(Match3Coroutine(wave, originIndex, isTriggeredByPlayer));
    }

    private IEnumerator Match3Coroutine(Wave wave, int originIndex, bool isTriggeredByPlayer)
    {
        yield return new WaitForSeconds(.4f);

        if (wave == null || !wave.gameObject.activeSelf) yield break;
        if (originIndex < 0 || originIndex >= wave.marbles.Count) yield break;

        var (count, startIndex) = findMatchesStartingAt(originIndex);

        if (count < 3) yield break;
        if (!isTriggeredByPlayer && startIndex >= originIndex) yield break;

        explosionSound.PlayOneShot(explosionSound.clip);
        int scoreIncrement = count * 1;
        GameManager.Instance.SetComboCount();
        GameManager.Instance.SetScore(scoreIncrement);
        wave.RemoveMarblesRange(startIndex, count, false);
    }

    private (int count, int startIndex) findMatchesStartingAt(int originIndex)
    {
        var current = wave.marbles.ElementAt(originIndex);

        var leftIndex = originIndex;
        var rightIndex = originIndex;

        while (leftIndex > 0 && wave.marbles[leftIndex - 1].color == current.color)
        {
            leftIndex--;
        }

        while (rightIndex < wave.marbles.Count() - 1 && wave.marbles[rightIndex + 1].color == current.color)
        {
            rightIndex++;
        }

        var count = rightIndex - leftIndex + 1;

        return (count, startIndex: leftIndex);
    }
}