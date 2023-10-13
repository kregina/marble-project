using UnityEngine;

public class Marble : MonoBehaviour
{
    public MarbleColor color;

    [SerializeField] ParticleSystem explosionParticlePrefab;
    [SerializeField] AudioClip explosionSound;

    [HideInInspector] public PathFollower pathFollower;
    [HideInInspector] public Wave parentWave;

    private void OnBecameInvisible()
    {
        Destroy(gameObject);    
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("End"))
        {
           Debug.Log("Marble collided with end");
        }
    }

    void OnDestroy()
    {
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 2f);
    }
}


