using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource freezeSound;
    public AudioSource waterDripSound;
    public void PlayFreezeSound()
    {
        freezeSound.Play();
    }

    public void PlayWaterDripSound()
    {
        waterDripSound.Play();
    }
}
