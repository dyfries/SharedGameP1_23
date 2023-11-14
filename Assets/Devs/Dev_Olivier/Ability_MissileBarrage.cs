using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MissileBarrage : Ability_Simple
{
    [Header("Missile Barrage Settings")]
    [SerializeField] private Rigidbody2D missile;
    [SerializeField] private int amountOfMissiles = 5;
    [SerializeField] private float missileForce = 10;
    [SerializeField] private float horizontalSpacing = 0;
    [SerializeField] private float verticalSpacing = 1;
    [SerializeField] private AnimationCurve windupCurve;
    [SerializeField] private AnimationCurve missilePlacementCurve;


    private Rigidbody2D[] missiles;
    private Vector3[] startRotations;
    private Vector2 barrageOrigin;
    private float windupTimer = 0;
    private AudioSource releaseAudio;

    private void Awake()
    {
        releaseAudio = GetComponentInChildren<AudioSource>();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        releaseAudio.Play();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Jump"))
        {
            ActivateAbility();
        }

        if (stageOfAbility == StageOfAbility.windup)
        {
            windupTimer += Time.deltaTime / activatedAbility_WindupTimer;

            for (int i = 0; i < amountOfMissiles; i++)
            {
                //centers missile formation above player
                float missileUnit = missile.transform.localScale.x + (horizontalSpacing* missile.transform.localScale.x/2);
                float xPos = (i * missileUnit) - ((float)amountOfMissiles / 2 * missileUnit) + (missile.transform.localScale.x / 2) + (horizontalSpacing * missile.transform.localScale.x/4);
                float yPos = missilePlacementCurve.Evaluate(1 - (Mathf.Abs(xPos)/((amountOfMissiles-1)* missile.transform.localScale.x)));
                Vector2 formationPos = new Vector2(barrageOrigin.x + xPos,barrageOrigin.y + (yPos*verticalSpacing));

                //lerps missiles from player position to their desired position in arrow formation
                //happens in first half of the windup
                missiles[i].transform.position = Vector2.Lerp(barrageOrigin, formationPos, windupCurve.Evaluate(windupTimer * 2));

                //lerps rotaion to be forward
                //happens in second half of the windup
                missiles[i].GetComponent<Projectile_Missile>().setHeadingDirection(Quaternion.Slerp(Quaternion.Euler(startRotations[i]), Quaternion.Euler(Vector3.zero), windupCurve.Evaluate((windupTimer * 2) - 1)).eulerAngles.z);
            }
        }

    }

    protected override void StartWindup()
    {
        base.StartWindup();

        windupTimer = 0;

        missiles = new Rigidbody2D[amountOfMissiles];
        startRotations = new Vector3[amountOfMissiles];
        barrageOrigin = transform.position;

        for (int i = 0; i < amountOfMissiles; i++)
        {
            //create missiles in scene and keep track of them
            Rigidbody2D newMissile = Instantiate(missile, barrageOrigin, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)));
            newMissile.GetComponent<Collider2D>().enabled = false;
            missiles[i] = newMissile;
            startRotations[i] = missiles[i].transform.rotation.eulerAngles;
        }

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        StartCoroutine(StageredFire());

    }

    IEnumerator StageredFire()
    {
        for (int i = 0; i < amountOfMissiles; i ++)
        {
            int currentMissile;

            //stagger order being launched so that edges go first and middle goes last
            if (i % 2 == 0)
            {
                currentMissile = i / 2;
            }
            else
            {
                currentMissile = amountOfMissiles - Mathf.CeilToInt((float)i/2f);
            }

            //calculate how long to wait based on how many missles we need to fire in how much time
            yield return new WaitForSeconds(activatedAbility_FiringTimer / amountOfMissiles);
            //missiles[currentMissile].AddForce(Vector2.up * missileForce, ForceMode2D.Impulse);
            missiles[currentMissile].GetComponent<Projectile_Missile>().setMoveSpeed(missileForce);
            missiles[currentMissile].GetComponent<Projectile_Missile>().setIsWobble(true);
            missiles[currentMissile].GetComponent<Collider2D>().enabled = true;
        }

    }

}
