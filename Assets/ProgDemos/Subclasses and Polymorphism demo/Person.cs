using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Person : MonoBehaviour
{

    protected string myName = "Dylan";
    protected int id = 0;

    // Virtual means this class can be extended by child subclasses. 
    public virtual string GetPersonType() {
        return "Person";
    }

    public virtual string SuperTest() {
        return "In Person ";
    }



}
