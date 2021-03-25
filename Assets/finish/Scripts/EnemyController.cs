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
   

    private bool useLaser;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.speed = EnemyProperties.speedMove;

        if (EnemyProperties.weaponName == "Laser Gun")
            useLaser = true;

        else
            useLaser = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!photonView.IsMine || player.GetComponent<TakeDamage>().health <= 0f)
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

        if (useLaser)
        {
            //laser codes
                
            
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;

                }

                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.1f;



                lineRenderer.SetPosition(0, firePosition.position);
                lineRenderer.SetPosition(1, player.transform.position);


                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, EnemyProperties.damage);
                    }

                }
                yield return new WaitForSeconds(0.2f);
                lineRenderer.enabled = false;
        }
        else
        {
            GameObject bullletGameObject = Instantiate(BulletPrefab, firePosition.position, Quaternion.identity);
            bullletGameObject.GetComponent<BulletScript>().Initialize(ray.direction, EnemyProperties.bulletSpeed, EnemyProperties.damage);
        }
        


        
        yield return new WaitForSeconds(EnemyProperties.shootDelay);
        canShoot = true;
    }


}
