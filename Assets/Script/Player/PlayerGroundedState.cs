using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (Mouse.current.leftButton.wasPressedThisFrame||(gamepad!=null && gamepad.buttonWest.wasPressedThisFrame))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        
        if (!player.IsGroundDetected()&& moveDirection != 0)
        {
            stateMachine.ChangeState(player.airState);
        }else if (!player.IsGroundDetected() && moveDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpAirState);
        }
        if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection != 0)
        {
            stateMachine.ChangeState(player.jumpState);
        }else if ((gamepad != null && gamepad.buttonSouth.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0) || Keyboard.current.spaceKey.wasPressedThisFrame && player.IsGroundDetected() && moveDirection == 0)
        {
            stateMachine.ChangeState(player.straightJumpState);
        }
    }
}
