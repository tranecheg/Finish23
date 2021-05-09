using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPun
{
    public GameObject BulletPrefab, startShot;
    public Transform firePosition;
    

    public DeathRacePlayer DeathRacePlayerProperties;
    public DeathRaceEnemy EnemyProperties;
    public AudioSource audioSource;
    public AudioClip laserGun;
    public AudioClip machineGun;
    public AudioClip rocketLauncher;
    public Camera PlayerCamera;


    private float fireRate;
    private float fireTimer = 0.0f;
    public bool useLaser;
    public LineRenderer lineRenderer;
    public bool isShooting;
    public Joystick joystick;
    private float joystickHor, joystickVer;
    Ray ray;

    public float bullet = 10f, reloadFinish = 10f;
    public float bulletCount, reloadTime;
    void Start()
    {
        fireRate = DeathRacePlayerProperties.fireRate;
        if (!GetComponent<PhotonView>().Controller.IsLocal)
            GetComponent<Shooting>().enabled = false;

         PlayerCamera = GameObject.Find("CameraHolder " + gameObject.name).transform.GetChild(0).GetComponent<Camera>();

        if (DeathRacePlayerProperties.weaponName== "Laser Gun" )
            useLaser = true;
        else
            useLaser = false;

#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
        if (photonView.IsMine)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            joystick = GameObject.Find("CarMove").GetComponent<FixedJoystick>();
        }
            
#endif



    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (Input.GetMouseButton(0) && bulletCount > 0)
        {
            if (fireTimer>fireRate)
            {
                //fİRE
                photonView.RPC("Fire", RpcTarget.All, firePosition.position);
                bulletCount--;
                fireTimer = 0.0f;
            }       
        }
#else
       
        if (isShooting && bulletCount > 0)
        {
            if (fireTimer > fireRate)
            {
                //fİRE
                photonView.RPC("Fire", RpcTarget.All, firePosition.position);

                fireTimer = 0.0f;
            }
        }
        joystickHor = joystick.Horizontal;
        PlayerPrefs.SetFloat("JoystickHor", joystick.Horizontal);
        joystickVer = joystick.Vertical;
        PlayerPrefs.SetFloat("JoystickVer", joystick.Vertical);
#endif

        if (fireTimer<fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        if (bulletCount == 0)
            reloadTime -= Time.deltaTime;

        if (reloadTime <= 0)
        {
            reloadTime = reloadFinish;
            bulletCount = bullet;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);//переворот обратно, потом удалить!




    }
    public void Shot()
    {
        isShooting = !isShooting;

    }

    [PunRPC]
    public void Fire(Vector3 _firePosition)
    {

        if (useLaser)
        {
            //laser codes
            RaycastHit _hit;
            ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray,out _hit, 200))
            {

                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                    audioSource.clip = laserGun;
                    audioSource.Play();
                }

                lineRenderer.startWidth = 0.3f;
                lineRenderer.endWidth = 0.1f;

                

                lineRenderer.SetPosition(0,_firePosition);
                lineRenderer.SetPosition(1,_hit.point);

                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    if (_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, DeathRacePlayerProperties.damage);
                    }
                
                }
                if (_hit.collider.gameObject.CompareTag("Enemy"))
                {
                    if (_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, EnemyProperties.damage);
                    }

                }


                StopAllCoroutines();
                StartCoroutine(DisableLaserAfterSecs(0.3f));
                
            }

        }
        else
        {
            Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            GameObject bullletGameObject = Instantiate(BulletPrefab, _firePosition, Quaternion.identity);
            bullletGameObject.GetComponent<BulletScript>().Initialize(ray.direction, DeathRacePlayerProperties.bulletSpeed, DeathRacePlayerProperties.damage);

            

            if (DeathRacePlayerProperties.weaponName == "Rocket Launcher")
            {
                GameObject exp = Instantiate(startShot, firePosition.position, Quaternion.identity);
                exp.transform.SetParent(firePosition);
                exp.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                Destroy(exp, 5);
                audioSource.clip = rocketLauncher;
                audioSource.Play();


            }
            if (DeathRacePlayerProperties.weaponName == "Machine Gun")
            {
                GameObject exp = Instantiate(startShot, firePosition.position, Quaternion.identity);
                exp.transform.SetParent(firePosition);
                Destroy(exp, 3);

                audioSource.clip = machineGun;
                audioSource.Play();


            }




        }      
    }



    IEnumerator DisableLaserAfterSecs(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lineRenderer.enabled = false;

    }
}
