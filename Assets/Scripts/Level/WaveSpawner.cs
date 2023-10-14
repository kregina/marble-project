using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;

    public PathCreator pathCreator;
    public Wave wavePrefab;

    public int waveCount = 1;
    public int marblesPerWave = 10;
    public int waveIntervalSeconds = 30;

    private int completedWaves = 0;

    [SerializeField] private List<Wave> waves = new List<Wave>();

    void Start()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    IEnumerator SpawnWaveCoroutine()
    {
        for (int i = 0; i < waveCount; i++)
        {
            SpawnWave();
            yield return new WaitForSeconds(waveIntervalSeconds);
        }
    }

    void SpawnWave()
    {
        var newWave = Instantiate(wavePrefab);
        newWave.pathCreator = pathCreator;
        newWave.marblesPerWave = marblesPerWave;

        newWave.OnMarblesChanged += OnMarblesChanged;

        waves.Add(newWave);
    }

    void OnMarblesChanged(Wave wave, int originIndex)
    {
        if (wave.marbles.Count == 0)
        {
            completedWaves++;
        }

        if (completedWaves == waveCount)
        {
            HandleCompletedWave(wave);
        }
    }

    private void HandleCompletedWave(Wave wave)
    {
        Destroy(wave.pusher.gameObject, 0.1f);
        StartCoroutine(ClearLevelPanelCoroutine());
    }

    private IEnumerator ClearLevelPanelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SetLevel(GameManager.Instance.Level + 1);
        GameManager.Instance.LevelCleared();
        uiPanel.SetActive(true);
    }
}