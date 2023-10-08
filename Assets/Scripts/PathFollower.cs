using PathCreation;
using System.Collections;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [HideInInspector]
    public PathCreator PathCreator { get; set; }
    public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;

    public float targetDistance;
    public float smoothTime = 0.1f; // time to reach target
    public float smoothMaxSpeed = 50f; //projectile speed

    private float distanceTravelled;
    private float currentVelocity;

    void Start()
    {
        Debug.Log("PathFollower start");
        Move(false);
    }

    void Update()
    {
        Move(true);
    }

    private void Move(bool smooth)
    {
        if(smooth)
        {
            distanceTravelled = Mathf.SmoothDamp(distanceTravelled, targetDistance, ref currentVelocity, smoothTime, smoothMaxSpeed);
        }
        else
        {
            distanceTravelled = targetDistance;
        }
        
        transform.position = PathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        transform.rotation = PathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
    }
}