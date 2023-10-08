using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarbleColorManager : MonoBehaviour
{
    public event Action<MarbleColor> OnAvailableColorAdded;
    public event Action<MarbleColor> OnAvailableColorRemoved;

    [SerializeField] private Marble[] marblePrefabs;

    Dictionary<MarbleColor, int> colorCount = new();

    public ICollection<MarbleColor> availableColors => colorCount.Keys;

    public void AddColor(MarbleColor color)
    {
        colorCount[color] = colorCount.TryGetValue(color, out var i) ? i + 1 : 1;

        if (colorCount[color] == 1)
        {
            OnAvailableColorAdded?.Invoke(color);
        }
    }

    public void RemoveColor(MarbleColor color)
    {
        colorCount[color] = colorCount.TryGetValue(color, out var i) ? i - 1 : 0;

        if (colorCount[color] == 0)
        {
            colorCount.Remove(color);
            OnAvailableColorRemoved?.Invoke(color);
        }   
    }

    public MarbleColor GetRandomMarbleColor()
    {
        return availableColors.ElementAt(UnityEngine.Random.Range(0, availableColors.Count - 1));
    }

    public Marble GetMarblePrefab(MarbleColor color)
    {
        return marblePrefabs.First(prefab => prefab.color == color);
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
    Black,
}