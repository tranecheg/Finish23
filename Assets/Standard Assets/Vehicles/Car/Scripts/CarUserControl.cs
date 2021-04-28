using Photon.Pun;
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviourPun
    {
        private CarController m_Car; // the car controller we want to use

       
        private void Start()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            PlayerPrefs.DeleteKey("JoystickHor");
            PlayerPrefs.DeleteKey("JoystickVer");
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            if (!photonView.IsMine)
            {
                return;
            }
#if UNITY_EDITOR
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#else
            float h = PlayerPrefs.GetFloat("JoystickHor");
            float v = PlayerPrefs.GetFloat("JoystickVer");
#endif

#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
