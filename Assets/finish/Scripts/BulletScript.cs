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
       
        if (collision.gameObject.CompareTag("Player"))
        {

            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);
                if (collision.gameObject.GetComponent<EnemyTakeDamage>().health <= 0)
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

            }         
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, bulletDamage);
                if (collision.gameObject.GetComponent<EnemyTakeDamage>().health <= 0)
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

            }

        }
        Destroy(gameObject);

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
