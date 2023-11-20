using System.Collections;
using UnityEngine;

public class Ability_MissileBarrage : Ability_Simple
{
    [Header("Missile Barrage Settings")]
    [SerializeField] private Rigidbody2D missile;
    [SerializeField] private int amountOfMissiles = 5;
    [SerializeField] private float missileForce = 10;
    [SerializeField] private float horizontalSpacing = 0;
    [SerializeField] private AnimationCurve windupCurve;
    [SerializeField] private AnimationCurve missilePlacementCurve;
    [SerializeField] private AnimationCurve outlineRegenCurve;

    private Rigidbody2D[] missiles;
    private Vector3[] startRotations;
    private Vector2 barrageOrigin;
    private float timer = 0;
    private AudioSource releaseAudio;
    private ParticleSystem winddownParticles;
    private SpriteRenderer outline;

    private void Awake()
    {
        releaseAudio = GetComponentInChildren<AudioSource>();
        winddownParticles = GetComponentInChildren<ParticleSystem>();
        winddownParticles.Stop();
        outline = GetComponentInChildren<SpriteRenderer>();
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
            timer += Time.deltaTime / activatedAbility_WindupTimer;

            for (int i = 0; i < amountOfMissiles; i++)
            {
                //gets a unit to mesure the missile and the space between missiles
                float missileUnit = missile.transform.localScale.x;
                float spacingUnit = horizontalSpacing * missile.transform.localScale.x;

                //line up the missiles side by side with spacing included
                float xPos = i * (missileUnit + spacingUnit);
                //offset the line of missiles left to center it on the ship
                xPos -= (amountOfMissiles - 1) * (missileUnit + spacingUnit) / 2f;

                //find points to make a triangle with the y coordinates of the line of missiles
                //triangle ranges from 0 - 1 in position
                float yPos = 1 - (Mathf.Abs(xPos) / ((amountOfMissiles - 1) * (missileUnit + spacingUnit) / 2f));
                //evaluates to a curve for extra style
                yPos = missilePlacementCurve.Evaluate(yPos);

                //offset the missiles to the player position
                xPos += barrageOrigin.x;
                yPos += barrageOrigin.y;

                //creates a vector out of the found positions
                Vector2 formationPos = new Vector2(xPos, yPos);

                //lerps missiles from player position to their desired position in arrow formation
                //happens in first half of the windup
                missiles[i].transform.position = Vector2.Lerp(barrageOrigin, formationPos, windupCurve.Evaluate(timer * 2));

                //lerps rotaion to be forward
                //happens in second half of the windup
                missiles[i].GetComponent<Projectile_Missile>().setHeadingDirection(Quaternion.Slerp(Quaternion.Euler(startRotations[i]), Quaternion.Euler(Vector3.zero), windupCurve.Evaluate((timer * 2) - 1)).eulerAngles.z);
            }
        }

        if (stageOfAbility == StageOfAbility.cooldown)
        {
            
            timer += Time.deltaTime / activatedAbility_CooldownTimer;

            Color clear = new Color(outline.color.r, outline.color.g, outline.color.b, 0);
            Color opaque = new Color(outline.color.r, outline.color.g, outline.color.b, 1);

            outline.color = Color.Lerp(clear, opaque, outlineRegenCurve.Evaluate(timer));

        }

    }

    protected override void StartWinddown()
    {
        base.StartWinddown();
        winddownParticles.Play();
    }

    protected override void StartCooldown()
    {
        base.StartCooldown();
        winddownParticles.Stop();
        timer = 0;
    }

    protected override void StartWindup()
    {
        base.StartWindup();

        timer = 0;

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

        outline.color = new Color(outline.color.r, outline.color.g, outline.color.b, 0);

    }

    protected override void StartFiring()
    {
        base.StartFiring();

        StartCoroutine(StageredFire());

    }

    IEnumerator StageredFire()
    {
        for (int i = 0; i < amountOfMissiles; i++)
        {
            int currentMissile;

            //stagger order being launched so that edges go first and middle goes last
            if (i % 2 == 0)
            {
                currentMissile = i / 2;
            }
            else
            {
                currentMissile = amountOfMissiles - Mathf.CeilToInt((float)i / 2f);
            }

            //calculate how long to wait based on how many missles we need to fire in how much time
            yield return new WaitForSeconds(activatedAbility_FiringTimer / amountOfMissiles);

            missiles[currentMissile].GetComponent<Projectile_Missile>().setMoveSpeed(missileForce);
            missiles[currentMissile].GetComponent<Projectile_Missile>().setIsWobble(true);
            missiles[currentMissile].GetComponent<Collider2D>().enabled = true;
        }

    }

    private void OnDrawGizmos()
    {
        if (DEBUG_MODE)
        {
            for (int i = 0; i < amountOfMissiles; i++)
            {
                //gets a unit to mesure the missile and the space between missiles
                float missileUnit = missile.transform.localScale.x;
                float spacingUnit = horizontalSpacing * missile.transform.localScale.x;

                //line up the missiles side by side with spacing included
                float xPos = i * (missileUnit + spacingUnit);
                //offset the line of missiles left to center it on the ship
                xPos -= (amountOfMissiles - 1) * (missileUnit + spacingUnit) / 2f;

                //find points to make a triangle with the y coordinates of the line of missiles
                //triangle ranges from 0 - 1 in position
                float yPos = 1 - (Mathf.Abs(xPos) / ((amountOfMissiles - 1) * (missileUnit + spacingUnit) / 2f));
                //evaluates to a curve for extra style
                yPos = missilePlacementCurve.Evaluate(yPos);

                //offset the missiles to the player position
                xPos += barrageOrigin.x;
                yPos += barrageOrigin.y;

                //creates a vector out of the found positions
                Vector2 formationPos = new Vector2(xPos, yPos);

                //lerps missiles from player position to their desired position in arrow formation
                //happens in first half of the windup
                Gizmos.DrawWireSphere(formationPos, missileUnit/2);

            }
        }
    }

}
