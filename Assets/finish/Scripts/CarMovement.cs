using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    Rigidbody rb;

    public Vector3 thrustForce = new Vector3(0f,0f,45f);
    public Vector3 rotationTorque = new Vector3(0f,8f,0f);


    public bool controlsEnabled;

    private string targetTag;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        controlsEnabled = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsEnabled)
        {
            float horPos = Input.GetAxis("Horizontal");
            float verPos = Input.GetAxis("Vertical");
            if(verPos!=0)
                rb.AddRelativeForce(thrustForce * verPos);
            if (horPos != 0)
                rb.AddRelativeTorque(rotationTorque * horPos);
           
        }

        
    }
}
