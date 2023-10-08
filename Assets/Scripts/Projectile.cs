using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;

    private Rigidbody rb;
    private Marble marble;
    private SphereCollider marbleCollider;

    private void Start()
    {
        Debug.Log("Projectile start");
        marbleCollider = GetComponent<SphereCollider>();
        marble = GetComponent<Marble>();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = transform.forward * speed;
    }

    private bool hasCollidedBefore = false;

    private void OnCollisionEnter(Collision collision)
    {        
        if (hasCollidedBefore) return;

        if (collision.gameObject.CompareTag("Marble") || collision.gameObject.CompareTag("Pusher"))
        {
            Debug.Log($"OnCollisionEnter tag: {tag}");
            hasCollidedBefore = true;
            HandleCollisionWithMarbleOrPusher(collision);
            StopProjectile();
        }
        else
        {
            Debug.Log($"Hit something else {collision.gameObject.name}");
        }
    }

    private void HandleCollisionWithMarbleOrPusher(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Marble"))
        {
            tag = "Marble";
            Marble otherMarble = collision.gameObject.GetComponent<Marble>();
            var otherMarbleIndex = otherMarble.parentWave.marbles.IndexOf(otherMarble);
            var insertIndex = DidHitOnTheFront(collision) ? otherMarbleIndex + 1 : otherMarbleIndex;
            otherMarble.parentWave.InsertMarbleAt(marble, insertIndex);
        }
        else if (collision.gameObject.CompareTag("Pusher"))
        {
            tag = "Marble";
            Pusher pusher = collision.gameObject.GetComponent<Pusher>();
            Debug.Log($"Hit Pusher. ParentWaveCount: {pusher.parentWave.marbles.Count}");
            pusher.parentWave.InsertMarbleAt(marble, 0);
        }
    }

    private void StopProjectile()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<PathFollower>().enabled = true;
        enabled = false;
    }

    private bool DidHitOnTheFront(Collision collision)
    {
        Vector3 collisionPoint = collision.contacts[0].point;
        Vector3 directionToCollision = collisionPoint - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, directionToCollision.normalized);

        if (dotProduct > 0)
        {
            Debug.Log("Hit on the front " + collision.gameObject.name);
            return true;
        }
        else
        {
            Debug.Log("Hit on the back " + collision.gameObject.name);
            return false;
        }
    }
}