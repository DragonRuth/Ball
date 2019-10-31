using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public Transform targetBall;

    private float speed = 5f;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(targetBall.position, transform.up, Input.GetAxis("Mouse X") * speed);
            transform.RotateAround(targetBall.position, transform.right, Input.GetAxis("Mouse Y") * speed);
        }
    }
}
