using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTester : MonoBehaviour
{
    public Person person; // actually Student

    public Person[] persons;

    public void Update() {
        

        Student stu1;
        Student stu;

        // Works with all types
        person.GetPersonType();

        if ( person is Student) {
            stu1 = person as Student;
            // Casting
            stu = (Student)person;

            Debug.Log("Student Obj in Person var cast back to student" + stu.GetStudentDebt());
            Debug.Log("Student Obj in Person var cast back to student" + stu1.GetStudentDebt());

        } else if( person is Teacher){
            Teacher t = (person as Teacher);
         //   t.studentsInClass.Clear;

        }







        // person.GetStudentDebt();

    }


    public void ActivatePersonType() {
        Debug.Log("Activating  Test (child class overriding (not extending) parent method as well");

        Debug.Log(person.GetPersonType());
    }

    public void ActivateSuperTest() {
        Debug.Log("Activating Super Test (child class calling base to get parent method as well");
        Debug.Log(person.SuperTest());
    }

    public void ActivatePersonList() {

        Debug.Log("Activating Person List");

        for (int i = 0; i < persons.Length; i++) {
            Debug.Log(persons[i].GetPersonType());

        }
    }
}
