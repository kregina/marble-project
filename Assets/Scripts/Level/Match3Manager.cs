using System.Collections;
using System.Linq;
using UnityEngine;

public class Match3Manager : MonoBehaviour
{
    [SerializeField] private Wave wave;

    void Start()
    {
        wave = GetComponent<Wave>();
        wave.OnMarblesChanged += OnMarblesChanged;
    }

    private void OnMarblesChanged(Wave wave, int originIndex)
    {
        Match3OrMoreColors(originIndex);
    }

    private void Match3OrMoreColors(int originIndex)
    {
        Debug.Log($"Match3OrMoreColors {originIndex}");
        if (originIndex < 0 || originIndex >= wave.marbles.Count)
        {
            return;
        }
        var (count, startIndex) = findMatchesStartingAt(originIndex);

        if (count >= 3)
        {
            StartCoroutine(RemoveMarblesAfterDelay(startIndex, count, .4f));
        }
    }

    private IEnumerator RemoveMarblesAfterDelay(int startIndex, int count, float delay)
    {
        yield return new WaitForSeconds(delay);
        wave.RemoveMarblesRange(startIndex, count);
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

        Debug.Log($"findMatchesStartingAt originIndex: {originIndex}, count: {count}, leftIndex: {leftIndex}, rightIndex: {rightIndex} ");

        return (count, startIndex: leftIndex);
    }
}