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
    private Vector2[] endPositions;
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

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Jump"))
        {
            ActivateAbility();
        }

        WindupLerp();
        CooldownLerp();
    }

    public override void ActivateAbility()
    {
        if (stageOfAbility == StageOfAbility.ready)
        {
            releaseAudio.Play();

        }
        base.ActivateAbility();
    }

    protected override void StartWindup()
    {
        base.StartWindup();

        timer = 0;

        //setup arrays
        missiles = new Rigidbody2D[amountOfMissiles];
        startRotations = new Vector3[amountOfMissiles];
        endPositions = new Vector2[amountOfMissiles];
        //cache the player's position
        barrageOrigin = transform.position;

        for (int i = 0; i < amountOfMissiles; i++)
        {
            //create missiles in scene and keep track of them in array
            Rigidbody2D newMissile = Instantiate(missile, barrageOrigin, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)));
            newMissile.GetComponent<Collider2D>().enabled = false;
            missiles[i] = newMissile;
            //get random rotation for missile
            startRotations[i] = missiles[i].transform.rotation.eulerAngles;
            //find the poition each missile should be in the final formation
            endPositions[i] = getEndPos(i);
        }
        //set the outline to be transparent;
        outline.color = new Color(outline.color.r, outline.color.g, outline.color.b, 0);
    }

    protected override void StartFiring()
    {
        base.StartFiring();
        StartCoroutine(StaggeredFire());
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

    private void CooldownLerp()
    {
        if (stageOfAbility == StageOfAbility.cooldown)
        {
            //increment timer from 0 to 1
            timer += Time.deltaTime / activatedAbility_CooldownTimer;

            //define two colors to lerp between
            Color clear = new Color(outline.color.r, outline.color.g, outline.color.b, 0);
            Color opaque = new Color(outline.color.r, outline.color.g, outline.color.b, 1);

            //lerp between the colors based on a curve
            outline.color = Color.Lerp(clear, opaque, outlineRegenCurve.Evaluate(timer));
        }
    }

    private void WindupLerp()
    {
        if (stageOfAbility == StageOfAbility.windup)
        {
            //increment timer from 0 to 1
            timer += Time.deltaTime / activatedAbility_WindupTimer;

            for (int i = 0; i < amountOfMissiles; i++)
            {
                //lerps missiles from player position to their desired position in arrow formation
                //happens in first half of the windup
                missiles[i].transform.position = Vector2.Lerp(barrageOrigin, endPositions[i], windupCurve.Evaluate(timer * 2));

                //lerps rotaion to be forward
                //happens in second half of the windup
                missiles[i].GetComponent<Projectile_Missile>().setHeadingDirection(Quaternion.Slerp(Quaternion.Euler(startRotations[i]), Quaternion.Euler(Vector3.zero), windupCurve.Evaluate((timer * 2) - 1)).eulerAngles.z);
            }
        }
    }

    private Vector2 getEndPos(int i)
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
        return formationPos;
    }

    IEnumerator StaggeredFire()
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
            //shows the position that the missiles will end up in
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
                Gizmos.DrawWireSphere(formationPos, missileUnit / 2);

            }
        }
    }
}
