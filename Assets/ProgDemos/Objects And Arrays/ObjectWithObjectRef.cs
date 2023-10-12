using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWithObjectRef : MonoBehaviour
{
    public ObjectWithObjectRef obj;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    public void DoStuff()
    {
        // Print name, then check for references.
        // 
        Debug.Log(gameObject.name);
        if(obj != null) {
            obj.DoStuff();
        }
    }

}
