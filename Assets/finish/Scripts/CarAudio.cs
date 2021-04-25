using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    AudioSource audioSource;
    public float minPitch = 0.6f, pitchRB = 10f;
    private float pitchFromCar;
    public AudioClip clip;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true;

        // start the clip from a random point
        audioSource.time = Random.Range(0f, clip.length);
        audioSource.Play();
        audioSource.volume = 0.5f;
        audioSource.minDistance = 5;
        audioSource.dopplerLevel = 0;
        audioSource.pitch = minPitch;
    }

    // Update is called once per frame
    void Update()
    {
        pitchFromCar = GetComponent<Rigidbody>().velocity.magnitude / pitchRB;
        if (pitchFromCar < minPitch)
            audioSource.pitch = minPitch;
        else
            audioSource.pitch = pitchFromCar;
    }
}
