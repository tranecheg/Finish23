using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;

public class BulletScript : MonoBehaviourPun
{

    float bulletDamage;
    private string Killer;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
       
        if (collision.CompareTag("Player"))
        {

            if (collision.GetComponent<PhotonView>().IsMine)
            {
               collision.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);

                if (collision.GetComponent<TakeDamage>() != null && collision.GetComponent<TakeDamage>().health <= 0)
                {
                    ChangeParams();
                }
                if (collision.GetComponent<EnemyTakeDamage>() != null && collision.GetComponent<EnemyTakeDamage>().health <= 0)
                {

                }



            }         
        }
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
               collision.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);
                if (collision.GetComponent<TakeDamage>() != null && collision.GetComponent<TakeDamage>().health <= 0)
                {
                    ChangeParams();
                }
                if (collision.GetComponent<EnemyTakeDamage>() != null && collision.GetComponent<EnemyTakeDamage>().health <= 0)
                {

                }


            }

        }
        Destroy(gameObject);

    }

    void ChangeParams()
    {
           PlayerParams.expDb += 10;
           PlayerParams.coinsDb += 5;
           if (PlayerParams.expDb >= 100)
           {
               PlayerParams.expDb = 0;
               PlayerParams.levelDb++;
           }
           StartCoroutine(ChangeParams(PlayerParams.loginDb, PlayerParams.levelDb.ToString(), PlayerParams.expDb.ToString(), PlayerParams.coinsDb.ToString()));
    }

    public void Initialize(Vector3 _direction,float speed, float damage)
    {
        
            bulletDamage = damage;

            transform.forward = _direction;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = _direction * speed;
        
        
        
    }
    IEnumerator ChangeParams(string login, string level, string exp, string coins)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("level", level);
        form.AddField("exp", exp);
        form.AddField("coins", coins);

        UnityWebRequest www = UnityWebRequest.Post("https://finish230.000webhostapp.com/change.php", form);
        yield return www.SendWebRequest();


    }


}
