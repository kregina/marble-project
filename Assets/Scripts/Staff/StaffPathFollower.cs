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
    [SerializeField] private float transitionDuration = 1f;

    private float distanceTravelled;

    private void Update()
    {
        if(distanceTravelled >= pathCreator.path.length)
        {
            StopMovement();
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
                transposer.m_FollowOffset = Vector3.Slerp(currentOffset, targetOffset, t);
                yield return null;
            }
            transposer.m_FollowOffset = targetOffset;
        }

        if (isAddingOffset)
        {
            yield return new WaitForSeconds(0.5f);
            staffPrefab.SetActive(true);
            StartCoroutine(MoveAlongPath());
        }
    }

    private IEnumerator MoveAlongPath()
    {
        var startDistance = 0;
        var targetDistance = pathCreator.path.length;
        var startTime = Time.time;
        var duration = 3f;

        var aaa = pathCreator.path.GetPointAtDistance(0, endOfPathInstruction);
        Debug.Log(aaa);

        while (pathCreator != null)
        {            
            float t = (Time.time - startTime) / duration;
            distanceTravelled = Mathf.SmoothStep(startDistance, targetDistance, t);
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            yield return null;
        }
    }
}
