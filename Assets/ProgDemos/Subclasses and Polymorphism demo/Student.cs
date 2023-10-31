using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Person
{

     
    public override string GetPersonType() {
        return "Student";
    }

    public override string SuperTest() {
        string s = base.SuperTest();
        return s += "In Teacher ";
    }
}
