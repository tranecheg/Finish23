using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;

public class EnemyController : MonoBehaviourPun
{
    public GameObject BulletPrefab;
    public Transform firePosition;
    private NavMeshAgent agent;
    private GameObject player;
    private RaycastHit hit;
    private Ray ray;
    private bool canShoot = true;
    
    public DeathRaceEnemy EnemyProperties;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = EnemyProperties.speedMove;

    }

    // Update is called once per frame
    void Update()
    {
        if(!photonView.IsMine || player.GetComponent<TakeDamage>().health <= 0f)
        {
            return;
        }

        agent.SetDestination(player.transform.position);

        ray = new Ray(firePosition.position, firePosition.right);
        Physics.Raycast(ray, out hit, 30f);
        if (hit.transform != null && hit.transform.gameObject.CompareTag("Player") && canShoot)
        {
            photonView.RPC("StartFire", RpcTarget.All);
        }
        

    }

    [PunRPC]
    public void StartFire()
    {
       StartCoroutine("Fire");
    }

    
    IEnumerator Fire()
    {
        canShoot = false;
        
        GameObject bullletGameObject = Instantiate(BulletPrefab, firePosition.position, Quaternion.identity);
        bullletGameObject.GetComponent<BulletScript>().Initialize(ray.direction, EnemyProperties.bulletSpeed, EnemyProperties.damage);
        
        yield return new WaitForSeconds(EnemyProperties.shootDelay);
        canShoot = true;
    }


}
