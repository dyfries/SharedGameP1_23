using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AccessEnum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        EnumExample.MyEnum e = EnumExample.MyEnum.Option1;

        if(e == EnumExample.MyEnum.Option1) {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
