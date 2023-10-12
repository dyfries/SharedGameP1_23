using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerArtController : MonoBehaviour {
    // Start is called before the first frame update
    [Header("Component Reference")]
    // Main SpriteRenderer for body art
    public SpriteRenderer bodyRenderer;
    // Thrusters
    public SpriteRenderer thrust_main;
    public SpriteRenderer thrust_reverse;
    public SpriteRenderer thrust_boost; // Additive main + boost for effect

    [Header("Sprites")]
    public Sprite neutralSprite;
    public Sprite tiltLeft;
    public Sprite turnLeft;
    public Sprite tiltRight;
    public Sprite turnRight;

    [Header("Sprite Settings")]
    public float turnThreshold = .7f;
    public float tiltThreshold = .2f;

    // Cached Input values
    private float xIn;
    private float yIn;
    void Start() {
       // renderer = GetComponent<SpriteRenderer>(); // Need Specific Renderer for specific effects
    }

    // Update is called once per frame
    void Update() {

        // ======= X-Axis and Turning (Body tilt) =======
        xIn = Input.GetAxis("Horizontal");

        // Determien the sprite based on the input
        // Q: Why Input and not movement? 

        // remember left turn is a negative x value
        if (xIn < -turnThreshold) {
            // Left Turn
            bodyRenderer.sprite = turnLeft;
        } else if (xIn > -turnThreshold && xIn < -tiltThreshold) {
            // Tilt Left
            bodyRenderer.sprite = tiltLeft;
        } else if (xIn > -tiltThreshold && xIn < tiltThreshold) {
            // mid
            bodyRenderer.sprite = neutralSprite;
        } else if (xIn < turnThreshold && xIn > tiltThreshold) {
            // Tilt right
            bodyRenderer.sprite = tiltRight;
        } else if (xIn > turnThreshold) {
            // Turn Right
            bodyRenderer.sprite = turnRight;
        } else {
            Debug.LogWarning("Dummy check - Should never reach this ");
        }

        // ======= Y-Axis and thrusters =======
        // Y Input is going to determine thrusters
        yIn = Input.GetAxis("Vertical");

        // Main Booster
        if (yIn > -.1f) {
            thrust_main.enabled = true ;
        } else {
            thrust_main.enabled = false;
        }

        // Extra thrust booster (stacks with main)
        if (yIn > .9f) {
            thrust_boost.enabled = true;
        } else {
            thrust_boost.enabled = false;
        }

        // reverse thrust booster (for reversing). 
        if (yIn < -.2f) {
            thrust_reverse.enabled = true;
        } else {
            thrust_reverse.enabled = false;
        }

    }

}

/* Additional Notes: 
This works because the Input that you are getting already has a smoothing function on it. 
You can set up these settings in the Input Editor (Edit > Project Settings > Input Manager). 
The setting you are looking for are on the individual Axis' and Gravity and Sensitivity are a good place to start. 
Sensitivity is how fast the Input value moves towards the keypress, Gravity is how fast it returns to neutral. 
Read the tooltip s(Hover your mouse over the word) for more details. 
 You can use Input.RawAxis to bypass this smoothing (although usually it is good to have it on ). 
*/
