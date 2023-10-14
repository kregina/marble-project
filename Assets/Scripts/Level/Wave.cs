using PathCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public Pusher pusherPrefab;
    public event Action<Wave, int> OnMarblesChanged;
    public float speed = .2f;
    public float wavePushBackOnMatch = 2f;

    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public List<Marble> marbles = new();
    [HideInInspector] public Pusher pusher;
    [HideInInspector] public int marblesPerWave;

    private MarbleColorManager marbleColorManager;
    private float distanceTravelled;

    private void Start()
    {
        marbleColorManager = GameObject.FindWithTag("MarbleColorManager").GetComponent<MarbleColorManager>();
        Initialize();
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
            marbleColorManager.AddColor(marble.color);

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

        marbles.Insert(index, marble);
        marbleColorManager.AddColor(marble.color);

        UpdateDistanceTraveledOnChildren();

        OnMarblesChanged(this, index);
    }

    public void InsertMarblesRangeAt(ICollection<Marble> marblesToInsert, int index)
    {
        foreach (var marble in marblesToInsert)
        {
            marble.gameObject.transform.SetParent(transform);
            marble.parentWave = this;
            marble.pathFollower = marble.GetComponent<PathFollower>();
            marble.pathFollower.pathCreator = pathCreator;

            marbles.Add(marble);
            marbleColorManager.AddColor(marble.color);
        }

        UpdateDistanceTraveledOnChildren();

        OnMarblesChanged(this, index);
    }

    public void RemoveMarblesRange(int startIndex, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var marble = marbles[startIndex + i];
            marbleColorManager.RemoveColor(marble.color);
            Destroy(marble.gameObject, 0.01f);
        }

        marbles.RemoveRange(startIndex, count);

        OnMarblesChanged(this, startIndex);

        int scoreIncrement = count * 1;
        GameManager.Instance.SetComboCount();
        GameManager.Instance.SetScore(scoreIncrement);

        distanceTravelled = Math.Max(0, distanceTravelled - wavePushBackOnMatch);
    }

    public void MergeWaves(Wave otherWave)
    {
        InsertMarblesRangeAt(otherWave.marbles, marbles.Count - 1);
        otherWave.marbles.Clear();
        otherWave.OnMarblesChanged(otherWave, 0);
    }
}