
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsandArraysDemo : MonoBehaviour
{
    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Array.html
    // 1D arrays
    public int[] array = new int[4];
    public GameObject[] objects = new GameObject[4];


    // 2D 
    public int[][] arrayOfArrays = new int[4][]; // can't fill totally here. 


    // 2D arrays
    public int[,] array2D = new int[5,3];

    public int[,,] array3D;

    public int[,,,] array4D = new int[5, 3, 2, 1];

    // Start is called before the first frame update
    void Start()
    {
        int sum = 0;
        // Basic Arrays
        for (int i = 0; i < array.Length; i++) {
            // stuff here
            sum += array[i];
        }


        //    public int[][] arrayOfArrays = new int[4][];
        // array of arrays
        // Initialization - Rect array of arrays. 
        for (int i = 0; i < arrayOfArrays.Length; i++) {
            arrayOfArrays[i] = new int[3];

            for (int j = 0; j < arrayOfArrays[i].Length; j++) {
                arrayOfArrays[i][j] = Random.Range(0, 10);
            }
        }



        // Jagged array 
        for (int i = 0; i < arrayOfArrays.Length; i++) {
            arrayOfArrays[i] = new int[Random.Range(0, 10)];

            for (int j = 0; j < arrayOfArrays[i].Length; j++) {
                arrayOfArrays[i][j] = Random.Range(0, 10);
            }
        }

        // 2D Array
        for (int i = 0; i < array2D.GetLength(0); i++) {
            for (int j = 0; j < array2D.GetLength(1); j++) {
                array2D[i,j] = Random.Range(0, 10);
            }
        }


        Debug.Log("GetLength(0)" +  array2D.GetLength(0));
        Debug.Log("GetLength(1)" + array2D.GetLength(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
