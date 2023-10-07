using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    public float offset = 90f;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 direction = mousePosition - objectScreenPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;  

        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, angle, currentRotation.z);
    }

}
