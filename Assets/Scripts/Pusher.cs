using UnityEngine;

public class Pusher : MonoBehaviour
{
    [HideInInspector]
    public PathFollower pathFollower;

    private void Start()
    {
        Debug.Log("Push start");
        pathFollower = GetComponent<PathFollower>();
    }
}