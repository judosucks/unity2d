using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerDashState : PlayerState
{
    public bool IsDashing { get; private set; }

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        IsDashing = true;
        stateTimer = player.dashDuration;
       
    }

    public override void Exit()
    {
        base.Exit();
        IsDashing = false;
        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        // 设置冲刺速度
        player.SetVelocity(player.dashSpeed * player.facingDirection, rb.linearVelocity.y);

        // if (Mouse.current.leftButton.wasPressedThisFrame || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame))
        // {
        //     stateMachine.ChangeState(player.primaryAttackState);
        //     return;
        // }
        
        
        if (canPerformDashAttack)
        {
            Debug.Log("candashattack"+canPerformDashAttack);
            if (mouse.leftButton.wasPressedThisFrame || (gamepad != null && gamepad.buttonWest.wasPressedThisFrame))
            {
                Debug.Log("candashattack left"+canPerformDashAttack);
                stateMachine.ChangeState(player.crossKickState);
            }
            
            
        }
        else if(stateTimer < 0f)
        {
            Debug.Log("Switching to idle state");
            stateMachine.ChangeState(player.idleState);
        }
    }

    
}