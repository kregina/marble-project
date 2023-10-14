using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarbleColorManager : MonoBehaviour
{
    [SerializeField] private Marble[] marblePrefabs;

    public event Action<MarbleColor> OnAvailableColorAdded;
    public event Action<MarbleColor> OnAvailableColorRemoved;
    public ICollection<MarbleColor> availableColors => colorCount.Keys;

    private Dictionary<MarbleColor, int> colorCount = new();


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
        return marblePrefabs.ElementAt(UnityEngine.Random.Range(0, marblePrefabs.Length - 1)).color;
    }

    public MarbleColor GetRandomAvailableMarbleColor()
    {
        return availableColors.ElementAt(UnityEngine.Random.Range(0, availableColors.Count - 1));
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
