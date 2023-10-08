using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
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

		if(completedWaves == waveCount)
		{
			Debug.Log("You win!");
		}
    }
}