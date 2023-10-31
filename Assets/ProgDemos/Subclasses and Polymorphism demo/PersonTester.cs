using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTester : MonoBehaviour
{
    public Person person;

    public Person[] persons;

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
