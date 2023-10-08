using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public List<Marble> marbles = new();
    [HideInInspector] public Pusher pusher;
    public Pusher pusherPrefab;
    [HideInInspector] public int marblesPerWave;
    [HideInInspector] 
    MarbleColorManager marbleColorManager;


    public event Action<Wave, int> OnMarblesChanged;

    public float speed = .2f;

    private float distanceTravelled;

    private void Start()
    {
        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        Initialize();
        //Move();
    }

    void Update()
    {
        Move();
    }

    public void Initialize()
    {
        pusher = Instantiate(pusherPrefab, transform);
        pusher.parentWave = this;
        pusher.pathFollower = pusher.GetComponent<PathFollower>();
        pusher.pathFollower.pathCreator = pathCreator;

        for (int i = 0; i < marblesPerWave; i++)
        {
            var marble = Instantiate(marbleColorManager.GetRandomMarblePrefab(), transform);
            marble.parentWave = this;
            marble.pathFollower = marble.GetComponent<PathFollower>();
            marble.pathFollower.pathCreator = pathCreator;

            marbles.Add(marble);
        }
    }

    void Move()
    {
        distanceTravelled += Time.deltaTime * speed;
        UpdateDistanceTraveledOnChildren();
    }

    void UpdateDistanceTraveledOnChildren()
    {
        pusher.pathFollower.targetDistance = distanceTravelled;

        for (int i = 0; i < marbles.Count; i++)
        {
            marbles[i].pathFollower.targetDistance = distanceTravelled + ((i + 1) * marbles[i].transform.localScale.z); ;
        }
    }

    public void InsertMarbleAt(Marble marble, int index)
    {
        marble.parentWave = this;
        marble.pathFollower = marble.GetComponent<PathFollower>();
        marble.pathFollower.pathCreator = pathCreator;        

        Debug.Log($"Inserting marble at index: {index}");
        marbles.Insert(index, marble);

        UpdateDistanceTraveledOnChildren();

        OnMarblesChanged(this, index);
    }

    public void RemoveMarbleAt(Marble marble, int index)
    {
        Debug.Log($"Removing marble at index: {index}");
        marbles.RemoveAt(index);
        Destroy(marble.gameObject);

        OnMarblesChanged(this, index);
    }

    public void RemoveMarblesRange(int startIndex, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Destroy(marbles[startIndex + i].gameObject);
        }

        Debug.Log($"Removing marbles at range {startIndex} +{count}");
        marbles.RemoveRange(startIndex, count);

        OnMarblesChanged(this, startIndex);
    }
}