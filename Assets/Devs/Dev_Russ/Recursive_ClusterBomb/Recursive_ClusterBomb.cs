using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive_ClusterBomb : MonoBehaviour
{
    public Helper_ClusterBomb ability;

    public GameObject toSpawnAtEnd;
    public float destroyTimer = 1.5f;
    public float endDestroyTimer = 1f;

    public float countdownTimer = 1f;
    public int depthCounter = 3;
    public int maxNumberBombs = 4;
    public float sizeScale = 1f;

    public Vector2 spawnOffset = Vector2.up;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bomb Launched with recursion depth " + depthCounter);
        Destroy(gameObject, destroyTimer);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (depthCounter > 0)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                //Activate: 
                Recursive_ClusterBomb rb = Instantiate<Recursive_ClusterBomb>(ability.bombToSpawn, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1,1)), Quaternion.identity);
                Recursive_ClusterBomb rb2 = Instantiate<Recursive_ClusterBomb>(ability.bombToSpawn, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)), Quaternion.identity);

                // NOTE: Because of the way unity instantiated prefabs, and references self prefabs, there is an edge case here where we need to manually update
                // the child reference of the prefab so it doesn't reference the prefab that just spawned it. 
                // Long thread but the answer is in the middle (Superpigs post).
                // https://forum.unity.com/threads/reference-to-prefab-changing-to-clone-self-reference.57312/
                // rb.toSpawnRecursively = toSpawnRecursively;
                // This only works one level deep. 

                // I am getting the reference directly from ability, and have to pass the ability reference as well
                rb.ability = ability;
                rb.depthCounter = depthCounter - 1;
                Rigidbody2D rigid = rb.GetComponent<Rigidbody2D>();
                rigid.AddForce(new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)) * 2, ForceMode2D.Impulse);
                
                rb2.ability = ability;
                rb2.depthCounter = depthCounter - 1;
                Rigidbody2D rigid2 = rb.GetComponent<Rigidbody2D>();
                rigid.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2, ForceMode2D.Impulse);


                // disable self so we don't spawn again
                this.enabled = false;
            }

        }
        else
        {
            // Recursion is done
            // Timer is finished. 
            GameObject end = Instantiate<GameObject>(toSpawnAtEnd, transform.position, Quaternion.identity);
            Destroy(end, destroyTimer);
            // disable self so we don't spawn again
            this.enabled = false;
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject end = Instantiate<GameObject>(toSpawnAtEnd, transform.position, Quaternion.identity);
        Destroy(end, endDestroyTimer);
        end.transform.localScale *= sizeScale;
        if (depthCounter > 0)
        {

            for(int i = Random.Range(1, maxNumberBombs); i > 0; i--)
            {
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                Recursive_ClusterBomb rb = Instantiate<Recursive_ClusterBomb>(ability.bombToSpawn, transform.position + new Vector3(x,y), Quaternion.identity);
                rb.ability = ability;
                rb.depthCounter = depthCounter - 1;
                rb.destroyTimer = destroyTimer / 2;
                rb.transform.localScale *= sizeScale;
                rb.sizeScale -= 0.15f;
                Rigidbody2D rigid = rb.GetComponent<Rigidbody2D>();
                rigid.AddForce(new Vector2(x, y) * 2, ForceMode2D.Impulse);

            }
            //Activate: 
            
            /*
            Recursive_ClusterBomb rb = Instantiate<Recursive_ClusterBomb>(ability.bombToSpawn, transform.position, Quaternion.identity);
                
            Recursive_ClusterBomb rb2 = Instantiate<Recursive_ClusterBomb>(ability.bombToSpawn, transform.position, Quaternion.identity);

                // NOTE: Because of the way unity instantiated prefabs, and references self prefabs, there is an edge case here where we need to manually update
                // the child reference of the prefab so it doesn't reference the prefab that just spawned it. 
                // Long thread but the answer is in the middle (Superpigs post).
                // https://forum.unity.com/threads/reference-to-prefab-changing-to-clone-self-reference.57312/
                // rb.toSpawnRecursively = toSpawnRecursively;
                // This only works one level deep. 

                // I am getting the reference directly from ability, and have to pass the ability reference as well
            
            rb.ability = ability;
            rb.depthCounter = depthCounter - 1;
            rb.destroyTimer = destroyTimer / 2;
            Rigidbody2D rigid = rb.GetComponent<Rigidbody2D>(); 
            rigid.AddForce(new Vector2(Random.Range(-1f, 1f) * 2, Random.Range(-1f, 1f)) * 2, ForceMode2D.Impulse);

            rb2.ability = ability;
            rb2.depthCounter = depthCounter - 1;
            rb2.destroyTimer = destroyTimer / 2;  
            Rigidbody2D rigid2 = rb2.GetComponent<Rigidbody2D>();
            rigid2.AddForce(new Vector2(Random.Range(-1f, 1f) * 2, Random.Range(-1f, 1f)) * 2, ForceMode2D.Impulse);
            */

                // disable self so we don't spawn again
            this.enabled = false;

        }
        else
        {
            // Recursion is done
            // Timer is finished. 
            // disable self so we don't spawn again
            this.enabled = false;
        }
    }
}
