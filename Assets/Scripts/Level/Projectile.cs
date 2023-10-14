using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 50f;

    private Rigidbody rb;
    private Marble marble;

    private void Start()
    {
        marble = GetComponent<Marble>();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = transform.forward * speed;
    }

    private bool hasCollidedBefore = false;

    private void OnCollisionEnter(Collision collision)
    {        
        if (hasCollidedBefore) return;

        if (collision.gameObject.CompareTag("Marble"))
        {
            hasCollidedBefore = true;
            Marble otherMarble = collision.gameObject.GetComponent<Marble>();
            var otherMarbleIndex = otherMarble.parentWave.marbles.IndexOf(otherMarble);
            var insertIndex = DidHitOnTheFront(collision) ? otherMarbleIndex + 1 : otherMarbleIndex;
            otherMarble.parentWave.InsertMarbleAt(marble, insertIndex, true);
            StopProjectile();
        }
        else
        {
            Debug.Log($"Hit something else {collision.gameObject.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollidedBefore) return;

        if (other.gameObject.CompareTag("Pusher"))
        {
            hasCollidedBefore = true;
            Pusher pusher = other.gameObject.GetComponent<Pusher>();
            pusher.parentWave.InsertMarbleAt(marble, 0, true);
            StopProjectile();
        }
        else
        {
            Debug.Log($"Hit something else {other.gameObject.name}");
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
            return true;
        }
        else
        {
            return false;
        }
    }
}