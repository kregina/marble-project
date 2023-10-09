using UnityEngine;

public class Marble : MonoBehaviour
{
    public MarbleColor color;
    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void OnBecameInvisible()
    {
        Debug.Log("Marble became invisible");
        Destroy(gameObject);    
    }
}


