using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChoise : MonoBehaviour
{
    private int carIndex, currentWeapon;
    GameObject firstActiveGameObject;
    
    void Update()
    {
        carIndex = PlayerSelection.playerSelectionNumber;
        for (int i = 0; i < 3; i++)
        {
            if (i == carIndex)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
        
