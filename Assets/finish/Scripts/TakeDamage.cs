using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class TakeDamage : MonoBehaviourPun
{
    public float starthHealth = 100f, health, speedRotateWeapon = 10f;
    public Image healthBar;
    public Transform weapon;
    Rigidbody rb;

    public GameObject PlayerGraphics;
    public GameObject PlayerUI;
    public GameObject PlayerWeaponHolder;
    public GameObject DeathPanelUIPrefab;
    private GameObject DeathPanelUIGameObject;

    public static int scoreA, scoreB;

    public Vector3 camPos, camRot;
    Vector3 startWeaponRot, startPos;
    public bool isShooting;


    private void Awake()
    {
        gameObject.name = GetComponent<PhotonView>().Controller.NickName;
        
    }
    void Start()
    {
        StartCoroutine(GetWeapon());

        health = starthHealth;

        healthBar.fillAmount = health / starthHealth;

        rb = GetComponent<Rigidbody>();
        
        string[] tags = { "Player", "Enemy" };
        gameObject.tag = tags[Random.Range(0,2)];

        startPos = transform.position;
    }

    IEnumerator GetWeapon()
    {
        yield return new WaitForSeconds(1f);
        weapon = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0);
        startWeaponRot = weapon.localEulerAngles;

    }

    [PunRPC]
    public void DoDamage(float _damage)
    {
        health -= _damage;
      
        healthBar.fillAmount = health / starthHealth;

        if (health <= 0f)
        {
            //Die
            Die();
        }
    }

    void Die()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        PlayerGraphics.SetActive(false);
        PlayerUI.SetActive(false);
        PlayerWeaponHolder.SetActive(false);
        GetComponent<CarUserControl>().enabled = false;

        if (photonView.IsMine)
        {
            //respawn
            StartCoroutine(ReSpawn());
        }
    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");

        if (DeathPanelUIGameObject==null)
        {
            DeathPanelUIGameObject = Instantiate(DeathPanelUIPrefab, canvasGameObject.transform);

        }
        else
        {
            DeathPanelUIGameObject.SetActive(true);
        }

        Text respawnTimeText = DeathPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8.0f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime> 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00f");


            //GetComponent<CarMovement>().enabled = false;
            GetComponent<Shooting>().enabled = false;
        }


        DeathPanelUIGameObject.SetActive(false);

        // GetComponent<CarMovement>().enabled = true;
       GetComponent<Shooting>().enabled = true;


        transform.position = startPos;

        if (gameObject.CompareTag("Player"))
            scoreB++;
        else
            scoreA++;

        photonView.RPC("Reborn",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
    {
        health = starthHealth;
        healthBar.fillAmount = health / starthHealth;

        PlayerGraphics.SetActive(true);
        PlayerUI.SetActive(true);
        PlayerWeaponHolder.SetActive(true);
        GetComponent<CarUserControl>().enabled = true;
    }

   

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            GameObject.Find("CameraHolder " + gameObject.name).transform.position = camPos;
            GameObject.Find("CameraHolder " + gameObject.name).transform.eulerAngles = camRot;
        }

        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, Quaternion.Euler(new Vector3(startWeaponRot.y, (camRot.y - transform.eulerAngles.y) * -1f, 0)), Time.deltaTime * speedRotateWeapon);

       //if (transform.localEulerAngles.z > 80 && transform.localEulerAngles.z < 300)
       //{
       //    transform.position = startPos;
       //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 5, 0);
       //}
           



    }
    public void Shot()
    {
        isShooting = !isShooting;

    }



}
