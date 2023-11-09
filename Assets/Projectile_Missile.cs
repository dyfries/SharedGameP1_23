using UnityEngine;

public class Projectile_Missile : MonoBehaviour
{

    [SerializeField] GameObject mainThrust;
    [SerializeField] GameObject rightThrust;
    [SerializeField] GameObject leftThrust;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rot = transform.rotation.eulerAngles.z;
        rot -= 180;
        Debug.Log(rot);

        leftThrust.SetActive(rot < 0 && Mathf.Abs(rot) < 175);
        rightThrust.SetActive(rot > 0 && Mathf.Abs(rot) < 175);

        mainThrust.SetActive(rb.velocity.magnitude > 0.5f);

    }
}
