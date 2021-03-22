using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float speedRotate = 300f;

    // Update is called once per frame
    void Update()
    {

       transform.Rotate(0, Time.deltaTime * speedRotate * Input.GetAxis("Mouse X"), 0);

    }
}
