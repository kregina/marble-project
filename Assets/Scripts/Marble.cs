using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Marble : MonoBehaviour
{
    MarbleColorManager marbleColorManager;
    public MarbleColor color;

    [HideInInspector] public PathFollower pathFollower;


    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        marbleColorManager.AddColor(color);
    }

    void OnDestroy()
    {
        Debug.Log("FUUUUUUUUUUUUUUUUUUUUU");
        marbleColorManager.RemoveColor(color);
    }
}


