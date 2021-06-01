using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public GameObject upgrade, gunControl, cam, target, mobile;
    public Vector3 targetPos;
    public bool isUp, isDown;
    public Color col;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target!=null && target.transform.GetChild(0).GetComponent<MeshRenderer>().material.color == col)
            targetPos = target.transform.localPosition;

        if(isUp)
            target.GetComponent<GunMove>().yPos += 0.1f;
        if(isDown)
            target.GetComponent<GunMove>().yPos -= 0.1f;
    }
    public void GunControlOpen()
    {
        upgrade.SetActive(false);
        gunControl.SetActive(true);
        cam.transform.localPosition = new Vector3(-33f, 26f, -3.5f);
        cam.transform.localEulerAngles = new Vector3(35f, 90f, 0f);
        cam.transform.parent.GetComponent<CamFocus>().enabled = false;
        target = GameObject.Find(CarChoise.car).transform.GetChild(1).transform.GetChild(GunSelected.selectionNumber).gameObject;
        target.GetComponent<GunMove>().enabled = true;
        target.GetComponent<GunMove>().yPos = target.transform.position.y;
        Renderer ren = target.transform.GetChild(0).GetComponent<MeshRenderer>();
        ren.material.color = col;

#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
        mobile.SetActive(true);
#endif
    }
    public void GunControlClose()
    {
        upgrade.SetActive(true);
        gunControl.SetActive(false);
        target.GetComponent<GunMove>().enabled = false;
        target.transform.localPosition = targetPos;
        Renderer ren = target.transform.GetChild(0).GetComponent<MeshRenderer>();
        ren.material.color = col;
        mobile.SetActive(false);

    }
    public void gunUp()
    {
        isUp = !isUp;
    }
    public void gunDown()
    {
        isDown = !isDown;
    }

}
