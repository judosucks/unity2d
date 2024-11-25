using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : Entity
{
   
   public EnemyStateMachine StateMachine { get; private set; }

   protected override void Awake()
   {
      base.Awake();
      StateMachine = new EnemyStateMachine();
   }

   protected override void Update()
   {
      base.Update();
      StateMachine.currentState.Update();
   }
}
