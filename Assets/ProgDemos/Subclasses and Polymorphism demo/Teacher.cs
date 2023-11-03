using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : Person
{

    public List<Student> studentsInClass; 
    // Override means other function is overriding a parent class function. 
    // This should be in the child function. 
    public override string GetPersonType() {
        return "Teacher";
    }

    public override string SuperTest() {
        
        string s = base.SuperTest();
        return s += "In Teacher ";
    }
}
