using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Most basic projectile spawning for the player
public class PlayerShooter : MonoBehaviour
{

    public GameObject projectilePrefab;

    public float fireRate = .3f; // time between bullets
    private float currentFireTimer = 0;

    private bool isFiringButtonDown = false;
    // no reload or ammo for now. 


    // Start is called before the first frame update
    void Start()
    {
        if (!projectilePrefab) {
            Debug.LogWarning("Please give the " + gameObject.name + " PlayerShooter script a projectile reference");
        }
        currentFireTimer = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        isFiringButtonDown = Input.GetButton("Jump");

        currentFireTimer += Time.deltaTime;

        if (isFiringButtonDown) {
            if (currentFireTimer >= fireRate) {
                FireOneBullet();
                currentFireTimer = 0;
            }
        }

        
       
    }

    private void FireOneBullet() {
        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity); 

    }
}
