using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarbleColorManager : MonoBehaviour
{
    [SerializeField] private Marble[] marblePrefabs;

    public event Action<MarbleColor> OnFirstColorAvailable;
    public event Action OnNoColorsAvailable;

    public event Action<MarbleColor> OnAvailableColorAdded;
    public event Action<MarbleColor> OnAvailableColorRemoved;
    public ICollection<MarbleColor> availableColors => colorCount.Keys;

    private Dictionary<MarbleColor, int> colorCount = new();

    private int lastRandom;

    public WaveSpawner waveSpawner;


    public void AddColor(MarbleColor color)
    {
        colorCount[color] = colorCount.TryGetValue(color, out var i) ? i + 1 : 1;

        if (colorCount[color] == 1)
        {
            if(colorCount.Count == 1)
            {
                OnFirstColorAvailable?.Invoke(color);
            }
            else
            {
                OnAvailableColorAdded?.Invoke(color);
            }            
        }
    }

    public void RemoveColor(MarbleColor color)
    {
        colorCount[color] = colorCount.TryGetValue(color, out var i) ? i - 1 : 0;

        if (colorCount[color] == 0)
        {
            colorCount.Remove(color);

            if(colorCount.Count == 0)
            {
                OnNoColorsAvailable?.Invoke();
            }
            else
            {
                OnAvailableColorRemoved?.Invoke(color);
            }            
        }   
    }

    public MarbleColor GetRandomMarbleColor()
    {
        return marblePrefabs.ElementAt(UnityEngine.Random.Range(0, marblePrefabs.Length - 1)).color;
    }

    public MarbleColor GetRandomAvailableMarbleColor()
    {
        if(availableColors.Count == 0)
        {
            throw new Exception("No available colors in availableColors");
        }

        var marblesInFirstWave = waveSpawner.waves.FirstOrDefault()?.marbles.Select(m => m.color).ToList();

        if (marblesInFirstWave.Count == 0)
        {
            throw new Exception("No available colors in marblesInFirstWave");
        }


        var rand = UnityEngine.Random.Range(0, marblesInFirstWave.Count - 1);

        if(rand == lastRandom)
        {
            rand = UnityEngine.Random.Range(0, marblesInFirstWave.Count - 1);
        }
        lastRandom = rand;

        return marblesInFirstWave.ElementAt(rand);
    }

    public Marble GetMarblePrefab(MarbleColor color)
    {
        return marblePrefabs.First(prefab => prefab.color == color);
    }

    public Marble GetRandomMarblePrefab()
    {
        return GetMarblePrefab(GetRandomMarbleColor());
    }
}

public enum MarbleColor
{
    Red,
    Blue,
    Green,
    Orange,
    Purple,
    Yellow,
}
