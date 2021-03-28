using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyTakeDamage : MonoBehaviourPun
{
    public float starthHealth = 100f;
    public float health;
    public Image healthBar;

    Rigidbody rb;

    public GameObject PlayerGraphics;
    public GameObject PlayerUI;
    public GameObject PlayerWeaponHolder;
    private Vector3 startPos;

    
    void Start()
    {

        health = starthHealth;

        healthBar.fillAmount = health / starthHealth;

        startPos = transform.position;
        rb = GetComponent<Rigidbody>();

       
    }

    

    [PunRPC]
    public void DoDamage(float _damage)
    {
        health -= _damage;
        Debug.Log(health);

        healthBar.fillAmount = health / starthHealth;

        if (health <= 0f)
        {
            StartCoroutine(ReSpawn());
        }
    }

    IEnumerator ReSpawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GetComponent<EnemyController>().enabled = false;

        PlayerGraphics.SetActive(false);
        PlayerUI.SetActive(false);
        PlayerWeaponHolder.SetActive(false);

        transform.position = startPos;

        yield return new WaitForSeconds(5f);

        if (gameObject.CompareTag("Player"))
            TakeDamage.scoreB++;
        else
            TakeDamage.scoreA++;
        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
    {
        health = starthHealth;
        healthBar.fillAmount = health / starthHealth;

        PlayerGraphics.SetActive(true);
        PlayerUI.SetActive(true);
        PlayerWeaponHolder.SetActive(true);
        GetComponent<EnemyController>().enabled = true;
    }



}
