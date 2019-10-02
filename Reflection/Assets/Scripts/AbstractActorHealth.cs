using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractActorHealth : MonoBehaviour
{
    [FormerlySerializedAs("_hp")] public int Hp = 1;

    public virtual bool IsDied()
    {
        if (Hp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
    }

    public abstract void CheckDeath();


}
