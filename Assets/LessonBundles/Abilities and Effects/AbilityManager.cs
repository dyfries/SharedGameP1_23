using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simply for testing the abilies. 
// [ ] feel free to improve this but your code Must run on my version (and base class provided)
public class AbilityManager : MonoBehaviour {
    // Reference to a single ability
    public Ability_Simple currentAbility;
    // List of abilities - all will be checked. NO ERROR CHECKING
    // [ ] How would you add error checking to make sure
    // - [ ] Array is not empty
    // - [ ] Array is not holding NULL references
    // - [ ] anything else that could go wrong? 
    public Ability_Simple[] abilityList;
    public int currentAbilityID = 0; // Current index of the array - 

    // Iterate to the next ability in the Array. 
    public void SelectNextAbility() {
        currentAbilityID = (currentAbilityID + 1) % abilityList.Length;
        currentAbility = abilityList[currentAbilityID];
    }

    // Activate that ability. 
    public void Activate() {
        currentAbility.ActivateAbility();
    }

    // Activate all abilities in the array
    public void ActivateAll() {
        for (int i = 0; i < abilityList.Length; i++) {
            abilityList[i].ActivateAbility();
        }
    }


    // Start is called before the first frame update
    void Start() {
        Debug.Log("-- Testing One ability at a time -- ");
        Activate();
        SelectNextAbility();
        Activate();

        Debug.Log("-- Testing Whole Array of Abilities at once -- ");
        ActivateAll();
    }
}
