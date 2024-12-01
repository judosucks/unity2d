using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            //use skill
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("skill is on cooldown");
        return false;
    }
    public virtual void UseSkill()
    {
        //do some skill stuff
        Debug.Log("skill used");
    }
}
