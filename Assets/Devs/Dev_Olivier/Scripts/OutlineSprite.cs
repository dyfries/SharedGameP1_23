using UnityEngine;

public class OutlineSprite : MonoBehaviour
{

    private SpriteRenderer sp;
    private SpriteMask[] spms;
    private float pixelUnit;
    //addition matrix for one unit in each direction from center
    private int[,] offsetMatrix = new int[2, 8] { { 0, 0, 1, 1, 1, -1, -1, -1 },
                                                  { 1, -1, 0, 1, -1, 0, 1, -1 } };

    void Start()
    {
        //get the ship sprite
        sp = transform.root.GetComponentInChildren<SpriteRenderer>();
        //find the ppu of the ship sprite
        pixelUnit = 1f / sp.sprite.pixelsPerUnit;
        //create  8 sprite maskes all offset one pixel from the ship sprite
        for (int i = 0; i < 8; i++)
        {
            GameObject outline = new GameObject(i.ToString(), typeof(SpriteMask));
            outline.transform.parent = transform;

            outline.transform.position = new Vector2(offsetMatrix[0, i] * pixelUnit, offsetMatrix[1, i] * pixelUnit);

        }
        //get the created sprite masks in an array
        spms = GetComponentsInChildren<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        //change the sprite of the sprite masks to match the ship sprite
        if (spms[0].sprite != sp.sprite)
        {
            foreach (SpriteMask spm in spms)
            {
                spm.sprite = sp.sprite;
            }
        }
    }
}
