using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPlayerSounds : MonoBehaviour
{
    [Header("Inputs")]
    private Vector2 directionalInput;

    [Header("Audio Source")]
	[SerializeField] private AudioSource collisionAudio;
    [SerializeField] private AudioClip collisionAudioClip;
	private AudioSource thisAudioSource;

    private void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(directionalInput.magnitude <= 0)
        {
            thisAudioSource.mute = true;
        }
        else
        {
            thisAudioSource.mute = false;
        }
    }

    public void CollisionSounds()
    {
        collisionAudio.clip = collisionAudioClip;

        collisionAudio.Play();
    }
}
