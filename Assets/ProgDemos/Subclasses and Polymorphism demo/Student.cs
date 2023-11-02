using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Person
{
    public float debt = 10f;
     
    public override string GetPersonType() {
        return "Student";
    }

    public override string SuperTest() {
        string s = base.SuperTest();
        return s += "In Teacher ";
    }

    public float GetStudentDebt() {
        return debt;
    }
}
