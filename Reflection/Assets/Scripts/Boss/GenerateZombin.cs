using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Boss
{

    public class GenerateZombin : MonoBehaviour
    {
        public GameObject Zombin;
        public Transform generatePos; //生成地点


        public void generateZombin()
        {
            //Debug.Log(generatePos.position);
            var zombin = Instantiate(Zombin, generatePos.position, Quaternion.identity);
            
        }
        
    }
}