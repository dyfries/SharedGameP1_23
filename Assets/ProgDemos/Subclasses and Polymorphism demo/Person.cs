using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Person : MonoBehaviour
{
    private int iAmPrivate;
    protected string myName = "Dylan";

    // Virtual means this class can be extended by child subclasses. 
    public virtual string GetPersonType() {
        return "Person";
    }

    public virtual string SuperTest() {
        return "In Person ";
    }



}
