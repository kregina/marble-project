using UnityEngine;

public class Marble : MonoBehaviour
{
    public MarbleColor color;

    [SerializeField] ParticleSystem explosionParticlePrefab;

    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void OnBecameInvisible()
    {
        Destroy(gameObject, .1f);
    }

    private void OnDisable()
    {
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, .1f);
    }
}


