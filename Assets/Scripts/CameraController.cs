using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed = 10f;
    private float rotationSpeed = 25f;
    private float step;
    private float rotationStep;

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKey) 
        {
            step = speed * Time.deltaTime;
            rotationStep = rotationSpeed * Time.deltaTime;
            if (Input.GetKey("w"))
            {
                transform.Translate(Vector3.forward * step);
            }
            else if (Input.GetKey("s"))
            {
                transform.Translate(Vector3.back * step);
            }
            else if (Input.GetKey("a"))
            {
                transform.Translate(Vector3.left * step);
            }
            else if (Input.GetKey("d"))
            {
                transform.Translate(Vector3.right * step);
            }
            else if (Input.GetKey("q"))
            {
                transform.Rotate(Vector3.down * rotationStep);
            }
            else if (Input.GetKey("e"))
            {
                transform.Rotate(Vector3.up * rotationStep);
            }
        }
    }
}
