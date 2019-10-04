using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global {  
    public string loadName;  
    //进行单例
    private static Global instance;  
    public static Global GetInstance()  
    {  
        if (instance == null)  
            instance = new Global();  
          
        return instance;  
    }  
}  