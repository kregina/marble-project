using Cinemachine;
using PathCreation;
using System.Collections;
using UnityEngine;

public class StaffPathFollower : MonoBehaviour
{
    [SerializeField] private GameObject staffPrefab;
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
    [SerializeField] private Vector3 newCameraOffset = new Vector3(0, 8, 0);
    [SerializeField] private float speed = 5;
    [SerializeField] private float transitionDuration = 1f;

    private float distanceTravelled;

    private void Update()
    {
        if(distanceTravelled >= pathCreator.path.length)
        {
            StopMovement();
            staffPrefab.GetComponent<Floating>().enabled = true;
        }
    }

    public void ToggleMovement()
    {
        StartCoroutine(AdjustCameraOffset(true));
    }

    private void StopMovement()
    {
        StopCoroutine(MoveAlongPath());
    }

    private IEnumerator AdjustCameraOffset(bool isAddingOffset)
    {
        var transposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (transposer != null)
        {
            Vector3 currentOffset = transposer.m_FollowOffset;
            Vector3 targetOffset = isAddingOffset ? newCameraOffset : currentOffset;
            for (float time = 0; time < transitionDuration; time += Time.deltaTime)
            {
                float t = time / transitionDuration;
                transposer.m_FollowOffset = Vector3.Lerp(currentOffset, targetOffset, t);
                yield return null;
            }
            transposer.m_FollowOffset = targetOffset;
        }

        if (isAddingOffset)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(MoveAlongPath());
            staffPrefab.SetActive(true);
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            yield return null;
        }
    }
}
