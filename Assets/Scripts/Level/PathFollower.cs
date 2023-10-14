using PathCreation;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [HideInInspector]
    public PathCreator pathCreator { get; set; }
    public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;

    public float targetDistance;
    public float smoothTime = 1f; // time to reach target
    public float smoothMaxSpeed = 50f; //projectile speed

    private Vector3 currentVelocityPosition = Vector3.zero;
    private float currentVelocityRotation = 0;

    void Update()
    {
        Move();

        if (targetDistance >= pathCreator.path.length)
        {
            GameManager.Instance.GameOver();
            return;
        }
    }

    private void Move()
    {
        var targetPosition = pathCreator.path.GetPointAtDistance(targetDistance, endOfPathInstruction);
        var targetRotation = pathCreator.path.GetRotationAtDistance(targetDistance, endOfPathInstruction);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocityPosition, smoothTime, smoothMaxSpeed);
        transform.rotation = SmoothDampQuaternionYAxis(transform.rotation, targetRotation, ref currentVelocityRotation, smoothTime, smoothMaxSpeed);
    }

    private Quaternion SmoothDampQuaternionYAxis(Quaternion current, Quaternion target, ref float currentVelocity, float smoothTime, float smoothMaxSpeed)
    {
        if (Time.deltaTime == 0) return current;
        if (smoothTime == 0) return target;

        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          t.x,
          Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity, smoothTime, smoothMaxSpeed),
          t.z
        );
    }
}