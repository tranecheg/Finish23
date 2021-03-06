using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Networking;
using System;

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
    private bool useLaser;
    public LineRenderer lineRenderer;
   
    public Joystick joystick;
    private float joystickHor, joystickVer;
    Ray ray;

    public float bullet = 10f, reloadFinish = 10f;
    public float bulletCount, reloadTime;
    


    void Start()
    {
        
        transform.SetParent(GameObject.Find(photonView.Controller.NickName).transform.GetChild(1));
        PlayerCamera = GameObject.Find("CameraHolder " + transform.parent.transform.parent.gameObject.name).transform.GetChild(0).GetComponent<Camera>();
        
        audioSource = transform.parent.transform.parent.GetComponent<AudioSource>();

        if (DeathRacePlayerProperties.weaponName == "Laser Gun")
            useLaser = true;
        else
            useLaser = false;
        fireRate = DeathRacePlayerProperties.fireRate;



        if (GameObject.Find(photonView.Controller.NickName))
            photonView.RPC("TransformGun", RpcTarget.AllBuffered, GunSelected.selectionPos, GunSelected.selectionRot);


#if !UNITY_EDITOR && !UNITY_STANDALONE_WIN
        if (photonView.IsMine)
        {
            transform.parent.transform.parent.transform.GetChild(3).gameObject.SetActive(true);
            joystick = GameObject.Find("CarMove").GetComponent<FixedJoystick>();
        }
            
#endif

        if (!transform.parent.transform.parent.GetComponent<PhotonView>().Controller.IsLocal)
            GetComponent<Shooting>().enabled = false;


    }
    [PunRPC]
    void TransformGun(Vector3 gunPos, Vector3 gunRot)
    {
        transform.localPosition = gunPos;
        transform.localEulerAngles = gunRot;
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
       
        if (transform.parent.transform.parent.GetComponent<TakeDamage>().isShooting && bulletCount > 0)
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
                        if (_hit.collider.gameObject.GetComponent<EnemyTakeDamage>().health <= 0)
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
                if (_hit.collider.gameObject.CompareTag("Enemy"))
                {
                    if (_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        _hit.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, EnemyProperties.damage);
                        if (_hit.collider.gameObject.GetComponent<EnemyTakeDamage>().health <= 0)
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
                Destroy(exp, 3);
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
