using Unity.VisualScripting;
using UnityEngine;

public class Projectile_Missile : MonoBehaviour
{

    [SerializeField] GameObject mainThrust;
    [SerializeField] GameObject rightThrust;
    [SerializeField] GameObject leftThrust;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource explodeAudio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explodeAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rot = transform.rotation.eulerAngles.z;
        rot -= 180;

        if (leftThrust && rightThrust && mainThrust)
        {
            leftThrust.SetActive(rot < 0 && Mathf.Abs(rot) < 175);
            rightThrust.SetActive(rot > 0 && Mathf.Abs(rot) < 175);

            mainThrust.SetActive(rb.velocity.magnitude > 0.5f);
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        explodeAudio.Play();
        
        if (transform.GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                Debug.Log(transform.GetChild(0).GetChild(i).gameObject.name);
                Destroy(transform.GetChild(0).GetChild(i).gameObject);
            }
        }

        animator.SetTrigger("Explode");

        rb.simulated = false;
    }

    public void DestroySelf() { Destroy(gameObject); }

}
