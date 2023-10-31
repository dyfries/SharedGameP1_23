using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SubclassOfPerson : Person
{
    /* 
     *     protected string myName = "Dylan";

    // Virtual means this class can be extended by child subclasses. 
    public virtual string GetPersonType() {
        return "Person";
    }
    public virtual string SuperTest() {
        return "In Person ";
    }
*/
    // Start is called before the first frame update
    void Start()
    {
        myName = "Hello world";
        Debug.Log(GetPersonType());
    }

}
