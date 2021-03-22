using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public GameObject carChoise;
    private bool isActive;
    
    void Update()
    {
        carChoise.SetActive(isActive);
    }
    public void ChangeCar()
    {
        isActive = !isActive;
    }
    
}
