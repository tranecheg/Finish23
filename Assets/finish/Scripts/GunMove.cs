using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMove : MonoBehaviour
{
    public float yPos;
    Rigidbody rb;
    Renderer ren;
    Material mat;
    public Joystick joystick;
    public Color col;
    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        ren = transform.GetChild(0).GetComponent<MeshRenderer>();
        GetComponent<BoxCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        float verPos = Input.GetAxis("Vertical");
        float horPos = Input.GetAxis("Horizontal");
        if (Input.GetMouseButton(0))
            yPos += 0.1f;

        if (Input.GetMouseButton(1))
            yPos -= 0.1f;
#else
        float verPos = joystick.Vertical;
        float horPos = joystick.Horizontal;
            
#endif
        Vector3 dir = new Vector3(verPos, 0, horPos * -1f);
        rb.velocity = dir * 5f;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        mat = ren.material;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            ren.material.color = col;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ren.material.color = Color.red;
        }

    }
}
