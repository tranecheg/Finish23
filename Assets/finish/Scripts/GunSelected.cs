using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelected : MonoBehaviour
{
    public GameObject[] SelectableGun;
    public static int selectionNumber;
    public static Vector3 selectionPos, selectionRot;
    public Vector3 pos, rot;

    private void ActivateGun(int x)
    {
        foreach (GameObject selectablePlayer in SelectableGun)
        {
            selectablePlayer.SetActive(false);
        }
        SelectableGun[x].SetActive(true);
        selectionPos = SelectableGun[x].transform.localPosition;
        selectionRot = SelectableGun[x].transform.localEulerAngles;
        pos = selectionPos;
        rot = selectionRot;

    }
    public void GetGuns()
    {
        int gunsCount = GameObject.Find(CarChoise.car).transform.GetChild(1).childCount;
        SelectableGun = new GameObject[gunsCount];
        for (int i = 0; i < gunsCount; i++)
        {
            SelectableGun[i] = GameObject.Find(CarChoise.car).transform.GetChild(1).transform.GetChild(i).gameObject;
        }
        ActivateGun(selectionNumber);
    }
   

    public void NextGun()
    {
        selectionNumber += 1;
        if (selectionNumber >= SelectableGun.Length)
        {
            selectionNumber = 0;
        }
        ActivateGun(selectionNumber);
    }

    public void PreviousGun()
    {

        selectionNumber -= 1;
        if (selectionNumber < 0)
        {
            selectionNumber = SelectableGun.Length-1;
        }
        ActivateGun(selectionNumber);

    }
}
