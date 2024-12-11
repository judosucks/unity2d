using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerKneeKickState : PlayerState
{
    
    public PlayerKneeKickState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("enter kneekick state");
        player.isKneeKick = true;
        stateTimer = player.kneeKickCooldown;
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;
        // 冷却时间逻辑
        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(player.sprintState); // 膝击后返回冲刺状态
        }
    }

    public override void Exit()
    {
        base.Exit();
        // 开启冷却并在期间禁止其他输入
        
        player.StartCoroutine(player.BusyFor(player.kneeKickCooldown));
        player.isKneeKick = false;
        Debug.Log("exit kneekick state");
    }
}
