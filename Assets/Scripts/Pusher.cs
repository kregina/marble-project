using UnityEngine;

public class Pusher : MonoBehaviour
{
    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void Start()
    {
        Debug.Log("Push start");
    }
}