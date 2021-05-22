using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFocus : MonoBehaviour
{
    public Transform _target;
    public GameObject cam;
    void Start()
    {
        StartCoroutine(Gun());
    }

    // Update is called once per frame
    void Update()
    {
        _target = GameObject.Find(CarChoise.car).transform.GetChild(1).transform.GetChild(0);
        transform.position = _target.position - transform.forward * 3;
        transform.GetChild(0).localPosition = Vector3.Slerp(transform.GetChild(0).localPosition, new Vector3(-19, 14, 4), 2f * Time.deltaTime);
        transform.GetChild(0).eulerAngles = Vector3.Slerp(transform.GetChild(0).eulerAngles, new Vector3(25, 90, 0), 2f * Time.deltaTime);
    }
    
    IEnumerator Gun()
    {
        while (true)
        {
           yield return new WaitForSeconds(5);
           GetComponent<CamFocus>().enabled = false;
                        
        }
        
       
    }

}
