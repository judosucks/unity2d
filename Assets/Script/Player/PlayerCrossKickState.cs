using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerCrossKickState : PlayerState
{      
    
    private int kickCount = 0; // 计数第几次攻击
    private float cooldownTime = 1f; // 冷却时间，单位：秒
    public PlayerCrossKickState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 跨踢动作的初始化逻辑
        Debug.Log("enter cross kick state");
        Vector2 knockbackForce = kickCount == 0 ? player.firstKickKnockbackForce : player.secondKickKnockbackForce;
        float xVelocity = knockbackForce.x * player.facingDirection;
        float yVelocity = knockbackForce.y;

        player.SetVelocity(xVelocity, yVelocity);
        // 启动冷却协程
        player.StartCoroutine(CrossKickCooldown());
        kickCount = (kickCount + 1) % 2; // 更新攻击计数
    }

    public override void Exit()
    {
        base.Exit();
        triggerCalled = false;
        kickCount = 0; // 重置计数器 
    }

    public override void Update()
    {
        base.Update();

    }
    private IEnumerator CrossKickCooldown()
    {
        // 禁用玩家输入
        player.SetIsBusy(true);
        
        // 等待冷却时间
        yield return new WaitForSeconds(cooldownTime);
        
        // 冷却结束，恢复输入
        player.SetIsBusy(false);
        // 切换回待机状态
        stateMachine.ChangeState(player.idleState);
    }
    private void HandleAttackHit()
    {
        kickCount++;
        if (kickCount > 1) kickCount = 0; // 如果超过两次，重置计数
    }
   
}