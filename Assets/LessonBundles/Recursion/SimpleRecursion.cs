using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleRecursion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Iterative: " + IterativeSolutionEG(10));
        Debug.Log("Recursive: " + RecursiveSolutionEG(10));
        Debug.Log("Multiplication : " + RecursiveMultiplication(4, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Note: These are trivial problems just used to illustrate the 
    // difference in control structure. 

    // An Iterative solution to a problem is one in which a loop
    // control structure is used. 
    public int IterativeSolutionEG(int count) {
        int sum = 0;
        for(int i=0; i< count; i++) {
            sum++;
        }
        return sum;
    }

    // A Recursive solution is one in which the function calls itself, 
    // either directly or indirectly.
    // This particlur example is not a useful or good use of recursion, 
    // simply a simple example of what it looks like structurally. 
    public int RecursiveSolutionEG(int count) {
        // The recursion consists of two parts. 
        // A) The Base Case is the condition in which the recursion ends. 
        if (count <= 0) {
            return 0;
        // B) The Recursive Case is the condition in which the function is called again. 
        } else {
            return 1 + RecursiveSolutionEG(count - 1);
        }
    }

    // Every Recursion must have a base case, otherwise it will call itself forever, 
    // eventually consuming all available RAM and crashing your program. 
    // This is a type of infinite loop that will eventually cause a 
    // Stack Overflow, which means the memory becomes full and execution ends. 
    // If nothing else, now you know where the website gets its name. 


    // This is another intuitive example. You can think about multiplication of
    // two values as value A * value B, or put another way, you are 
    // Adding value A, value B number of times. 
    // 3x4 = 3 + 3 + 3 + 3
    // You could also think of 3x4 as equalling 3x3 + 3
    // or 3x2 + 3 + 3
    
    public int RecursiveMultiplication(int A, int B) {
        Debug.Log("RM: A:" + A + " B: " + B);

        // Base Case to end is B becomes 1 ( A * 1)
        if(B <= 1) {
            return A;
        } else {
            return RecursiveMultiplication(A, B - 1) + A;
        }
    }
}
