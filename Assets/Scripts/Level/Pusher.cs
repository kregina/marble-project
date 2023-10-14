using UnityEngine;

public class Pusher : MonoBehaviour
{
    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Marble"))
        {
            var otherMarble = other.gameObject.GetComponent<Marble>();

            if (otherMarble.parentWave != null && parentWave != otherMarble.parentWave)
            {
                Debug.Log("Merge waves");
                otherMarble.parentWave.MergeWaves(parentWave);
            }
        }
    }
}