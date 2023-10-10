using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("Floating Animation")]
    [SerializeField] float speed = 1f;
    [SerializeField] float height = 1f;

    private float initialYPosition;

    void Start()
    {
        initialYPosition = transform.position.z;
    }

    void Update()
    {
        float newZPosition = initialYPosition + Mathf.PingPong(Time.time * speed, height) - (height / 2f);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
    }
}
