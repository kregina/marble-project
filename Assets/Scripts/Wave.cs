using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Wave : MonoBehaviour
{
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public List<Marble> marbles = new();
    [HideInInspector] public Pusher pusher;

    public float speed = .2f;

    private float distanceTravelled;

    private void Start()
    {
        Move();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        distanceTravelled += Time.deltaTime * speed;
        pusher.pathFollower.targetDistance = distanceTravelled;

        for (int i = 0; i < marbles.Count; i++)
        {
            marbles[i].pathFollower.targetDistance = distanceTravelled + ((i + 1) * marbles[i].transform.localScale.z); ;
        }
    }
}