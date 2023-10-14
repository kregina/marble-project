using UnityEngine;

public class Marble : MonoBehaviour
{
    public MarbleColor color;

    [SerializeField] ParticleSystem explosionParticlePrefab;

    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 0.01f);    
    }

    void OnDestroy()
    {
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
    }
}


