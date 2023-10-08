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
    Black,
}

public static class MarbleRGB
{
    public static Dictionary<MarbleColor, Color> rgbColors = new()
    {
        [MarbleColor.Red] = new Color(250, 86, 86),
        [MarbleColor.Blue] = new Color(36, 82, 188),
        [MarbleColor.Green] = new Color(91, 202, 112),
        [MarbleColor.Orange] = new Color(233, 149, 72),
        [MarbleColor.Purple] = new Color(147, 42, 200),
        [MarbleColor.Yellow] = new Color(241, 230, 110),
        [MarbleColor.Black] = new Color(5, 5, 5),
    };

    public static Color GetRgbColor(this MarbleColor color)
    {
        return rgbColors[color];
    }
}