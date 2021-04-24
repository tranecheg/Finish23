using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CamRotate : MonoBehaviourPun
{
    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);


    private void Awake()
    {
        if (photonView == null)
            return;

        gameObject.name = "CameraHolder " + photonView.Controller.NickName;
        transform.SetParent(GameObject.Find("Environment").transform);


        if (!photonView.IsMine)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<CamRotate>().enabled = false;

        }

    }


    private void Start()
    {
        if (photonView == null)
            return;

        

        if (_target == null)
        {
            _target = GameObject.Find(PhotonNetwork.LocalPlayer.NickName).transform;

        }
        
        

       
    }

    private void FixedUpdate()
    {
        if (photonView == null)
            return;
        
        if (GameObject.Find(photonView.Controller.NickName))
            photonView.RPC("TransformCam", RpcTarget.AllBuffered, transform.position, transform.eulerAngles); // localPos

       


    }

    void Update()
    {
        if (_target == null)
            return;

        if (GameObject.Find("GameOptionsPanel") != null)
        {
            if (Input.GetMouseButton(1))
                CamMove();
        }
        else if (SceneManager.GetActiveScene().name == "DeathRaceScene")
            CamMove();





    }
    
    void CamMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;



        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }

    [PunRPC]
    void TransformCam(Vector3 targPos, Vector3 targRot)
    {
        GameObject.Find(photonView.Controller.NickName).GetComponent<TakeDamage>().camPos = targPos;
        GameObject.Find(photonView.Controller.NickName).GetComponent<TakeDamage>().camRot = targRot;
     

    }
}
