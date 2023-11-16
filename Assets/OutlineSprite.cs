using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSprite : MonoBehaviour
{

    private SpriteRenderer sp;
    private SpriteMask[] spms;

    // Start is called before the first frame update
    void Start()
    {
        sp = transform.root.GetComponentInChildren<SpriteRenderer>();

        spms = GetComponentsInChildren<SpriteMask>();

    }

    // Update is called once per frame
    void Update()
    {
        if (spms[0].sprite != sp.sprite)
        {
            foreach (SpriteMask spm in spms)
            {
                spm.sprite = sp.sprite;
            }
        }
    }
}
