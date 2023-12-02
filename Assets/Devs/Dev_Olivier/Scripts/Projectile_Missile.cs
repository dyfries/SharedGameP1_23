
using UnityEngine;

public class Projectile_Missile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0;
    [SerializeField] float lifeTime = 5f;

    [Header("Wobble Settings")]
    [SerializeField] bool isWobble = false;
    [SerializeField] float wobbleAmount = 10;
    [SerializeField] float wobbleSpeed = 10;

    [Header("Thrusters")]
    [SerializeField] GameObject mainThrust;
    [SerializeField] GameObject rightThrust;
    [SerializeField] GameObject leftThrust;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource explodeAudio;
    private float headingDirection;
    private float wobble;

    private float timer;

    public void setMoveSpeed(float _moveSpeed) { moveSpeed = _moveSpeed; }
    public void setHeadingDirection(float _headingDirection) { headingDirection = _headingDirection; }
    public float getHeadingDirection() { return headingDirection; }
    public void setIsWobble(bool _isWobble) { isWobble = _isWobble; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        explodeAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        timer += Random.Range(0, 10f);
        //will destroy itself after n seconds
        Invoke("BlowUp", lifeTime);
    }

    void Update()
    {
        //use a sin wave to wobble the missiles as they fly
        //adds character
        if (isWobble)
        {
            timer += Time.deltaTime * wobbleSpeed;
            wobble = Mathf.Sin(timer) * wobbleAmount;
        }
        else
        {
            wobble = 0;
        }

        //rotates missile to desired direction
        transform.rotation = Quaternion.Euler(Vector3.forward * (headingDirection + wobble));

        //"animates" the thrusters
        if (leftThrust && rightThrust && mainThrust)
        {
            float signedRot = transform.rotation.eulerAngles.z;
            signedRot -= 180;

            leftThrust.SetActive(signedRot < 0 && Mathf.Abs(signedRot) < 175);
            rightThrust.SetActive(signedRot > 0 && Mathf.Abs(signedRot) < 175);

            mainThrust.SetActive(rb.velocity.magnitude > 0.5f);
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BlowUp();
    }

    //stop the missile, play a sound, and play explosion animation
    void BlowUp()
    {
        explodeAudio.Play();

        if (transform.GetChild(0).childCount > 0)
        {
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                Destroy(transform.GetChild(0).GetChild(i).gameObject);
            }
        }

        animator.SetTrigger("Explode");

        rb.simulated = false;
    }

    //used for the animator to destroy this
    public void DestroySelf()
    {
        explodeAudio.Stop();
        Destroy(gameObject);
    }
}
