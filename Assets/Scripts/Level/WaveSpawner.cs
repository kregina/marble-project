using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;

    public PathCreator pathCreator;
    public Wave wavePrefab;

    public int waveCount = 3;    
    public int marblesPerWave = 10;
    public int waveIntervalSeconds = 30;

    private int wavesSpawned = 0;
    private int completedWaves = 0;
    private Coroutine spawnWaveCoroutine;

    public List<Wave> waves = new List<Wave>();

    void Start()
    {
        spawnWaveCoroutine = StartCoroutine(SpawnWaveCoroutine());
    }

    IEnumerator SpawnWaveCoroutine()
    {
        if(wavesSpawned < waveCount)
        {
            SpawnWave();
            yield return new WaitForSeconds(waveIntervalSeconds);
            yield return spawnWaveCoroutine = StartCoroutine(SpawnWaveCoroutine());
        }
    }

    void SpawnWave()
    {
        Debug.Log("Spawning wave");
        var newWave = Instantiate(wavePrefab);
        newWave.pathCreator = pathCreator;
        newWave.marblesPerWave = marblesPerWave;

        newWave.OnMarblesChanged += OnMarblesChanged;

        waves.Add(newWave);
        wavesSpawned++;
    }

    void OnMarblesChanged(Wave wave, Marble originMarble, bool isTriggeredByPlayer)
    {
        if (wave.marbles.Count == 0)
        {
            Debug.Log("Wave completed");
            wave.gameObject.SetActive(false);
            waves.Remove(wave);
            completedWaves++;

            if (completedWaves >= waveCount)
            {
                StartCoroutine(ClearLevelPanelCoroutine());
            }
            else if (waves.Count == 0)
            {
                StopCoroutine(spawnWaveCoroutine);
                spawnWaveCoroutine = StartCoroutine(SpawnWaveCoroutine());
            }
        }        
    }

    private IEnumerator ClearLevelPanelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SetLevel(GameManager.Instance.Level + 1);
        GameManager.Instance.LevelCleared();
        GameManager.Instance.Pause();
        uiPanel.SetActive(true);
    }
}