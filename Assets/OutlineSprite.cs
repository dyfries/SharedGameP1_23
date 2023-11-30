using UnityEngine;

public class OutlineSprite : MonoBehaviour
{

    private SpriteRenderer sp;
    private SpriteMask[] spms;
    private float pixelUnit;
    private int[,] offsetMatrix = new int[2, 8] { { 0, 0, 1, 1, 1, -1, -1, -1 },
                                                  { 1, -1, 0, 1, -1, 0, 1, -1 } };

    // Start is called before the first frame update
    void Start()
    {

        sp = transform.root.GetComponentInChildren<SpriteRenderer>();

        pixelUnit = 1f / sp.sprite.pixelsPerUnit;

        for (int i = 0; i < 8; i++)
        {
            GameObject outline = new GameObject(i.ToString(), typeof(SpriteMask));
            outline.transform.parent = transform;

            outline.transform.position = new Vector2(offsetMatrix[0, i] * pixelUnit, offsetMatrix[1, i] * pixelUnit);

        }

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
