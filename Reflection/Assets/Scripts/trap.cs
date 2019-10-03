using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    public int Demage=100;
    public float waitTime=2;
    public float attackCD=3;//陷阱触发后再触发所需要的时间
    public float InvokeTime=2;
    private float time=2;
    
   
    void Update()
    {
        time += Time.deltaTime;
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag=="Zombie")
        {
            if (time >= attackCD)
            {
                Invoke("wait", waitTime);
                collider.gameObject.GetComponent<AbstractActorHealth>().TakeDamage(Demage);
                time = 0;
            }
        }
    }

   void wait()
    {
        
    }
}
