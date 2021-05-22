using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChoise : MonoBehaviour
{
    private int carIndex;
    public static string car;
    
    void Update()
    {
        carIndex = PlayerSelection.playerSelectionNumber;
        for (int i = 0; i < 3; i++)
        {
            if (i == carIndex)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                car = transform.GetChild(i).gameObject.name;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
        
