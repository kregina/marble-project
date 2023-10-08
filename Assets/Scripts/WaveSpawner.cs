using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	public PathCreator pathCreator;
	public Pusher pusherPrefab;
	public Marble[] marblePrefabs;
	public Wave wavePrefab;

	public int waveCount = 1;
	public int marblesPerWave = 10;
	public int waveIntervalSeconds = 30;

	[SerializeField] private List<Wave> waves = new List<Wave>();

	void Start()
	{
		SpawnWave();
	}

	void SpawnWave()
	{
		var newWave = Instantiate(wavePrefab);
		newWave.pathCreator = pathCreator;
		
		newWave.pusher = Instantiate(pusherPrefab, newWave.transform);
		newWave.pusher.GetComponent<PathFollower>().PathCreator = pathCreator;

		for (int i = 0; i < marblesPerWave; i++)
		{
			var marble = Instantiate(RandomMarblePrefab(), newWave.transform);
            marble.GetComponent<PathFollower>().PathCreator = pathCreator;
			newWave.marbles.Add(marble);
		}

		waves.Add(newWave);
	}

    public Marble RandomMarblePrefab()
    {
        int index = Random.Range(0, marblePrefabs.Length - 1);
        return marblePrefabs[index];
    }


}