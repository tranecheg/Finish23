using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyTakeDamage : MonoBehaviour
{
    public float starthHealth = 100f;
    private float health;
    public Image healthBar;

    

    // Start is called before the first frame update
    void Start()
    {

        health = starthHealth;

        healthBar.fillAmount = health / starthHealth;


    }


    [PunRPC]
    public void DoDamage(float _damage)
    {
        health -= _damage;
        Debug.Log(health);

        healthBar.fillAmount = health / starthHealth;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }


    
}
