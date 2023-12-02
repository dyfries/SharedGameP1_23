using UnityEngine;

[RequireComponent (typeof(Projectile_Missile))]
public class HeatSeeker : MonoBehaviour
{
    [SerializeField] private LayerMask seekingMask;
    [SerializeField] private float seekingRadius = 5;
    [SerializeField] private float seekSpeed = 0.1f;

    private Projectile_Missile missile;

    void Start()
    {
        missile = GetComponent<Projectile_Missile>();
    }

    void Update()
    {
        Transform nearestNPC = FindNearest();

        //check if there's an npc near
        if (nearestNPC != null)
        {
            Debug.DrawLine(transform.position, nearestNPC.position, Color.red);
            FaceTarget(nearestNPC);
        }
    }

    //find the direction for the missile to face
    void FaceTarget(Transform target)
    {
        if (target != null)
        {
            //Find the difference between current rotation, the desired rotation for facing the current target
            //This is expressed as a signed angle which is a value between -180 to 180
            Vector2 diff = target.position - transform.position;
            float desiredDirection = Vector2.SignedAngle(transform.up, diff);

            Debug.DrawRay(transform.position, Vector3.forward);

            //If the desired direction is less than 0, we want to turn counter clockwise in order to face our target
            //The opposite is also true
            //returns a value either -1 or 1 for rotation direction
            if (desiredDirection != 0)
            {
                float normalDirection = desiredDirection / Mathf.Abs(desiredDirection);
                missile.setHeadingDirection(missile.getHeadingDirection() + (normalDirection * seekSpeed));
            }
        }
    }

    //finds the nearest transform to the current object
    private Transform FindNearest()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, seekingRadius, seekingMask);
        Collider2D nearest = null;

        try
        {
            nearest = hits[0];
            float dist = Vector2.Distance(transform.position, nearest.transform.position);
            for (int i = 0; i < hits.Length; i++)
            {
                float currentDist = Vector2.Distance(transform.position, hits[i].transform.position);
                if (currentDist < dist)
                {
                    nearest = hits[i];
                    dist = Vector2.Distance(transform.position, nearest.transform.position);
                }
            }

            return nearest.transform;
        }
        catch { return null; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seekingRadius);
    }

}
