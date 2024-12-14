using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravityScale;
    private EntityFX entityFX;
    private bool isLightningDestroyed = false;
    private Keyboard keyboard = Keyboard.current;
   
    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        entityFX = PlayerManager.instance.player.GetComponent<EntityFX>();
        entityFX.ResetHasDestroyedLightning(); // Reset flag
        defaultGravityScale = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
        
    }

    
    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            Debug.LogWarning("stateTimer > 0, flying upward.");
            rb.linearVelocity = new Vector2(0,3);
            return;

        }

        if (stateTimer <= 0 && !skillUsed)
        {
            Debug.LogWarning("state timer expired casting blackholeskill");
            rb.linearVelocity = new Vector2(0, 0);
            
            
        
                if (player.skill.blackholeSkill.CanUseSkill())
                {
                    Debug.LogWarning("canuse skill");
                    player.skill.blackholeSkill.UseSkill();
                  skillUsed = true;
                }

                return;

        }

       

// Handle R key press for Lightning destruction
        if (!isLightningDestroyed && keyboard?.rKey.wasPressedThisFrame == true)
        {
            Debug.LogWarning("R key pressed. Destroying lightning FX.");
            entityFX.ForceDestroyLightning();
            isLightningDestroyed = true;
            return; // Ensure this doesn't accidentally block transitions indefinitely.
        }
        if (keyboard == null)
        {
            Debug.LogWarning("keyboard is null");
        }
        if (!isLightningDestroyed)
        {
            Debug.LogWarning("Timeout: Forcing manual FX destruction.");
            entityFX.ForceDestroyLightning();
            isLightningDestroyed = true;
            return;
        }
        if (!player.skill.blackholeSkill.BlackholeSkillCompleted())
        {
            Debug.LogWarning("Skill not completed. Waiting...");
            return;//wait untill skill completed
        }
        // Proceed with state transition logic only if the R key wasn't pressed
        if (player.skill.blackholeSkill.BlackholeSkillCompleted())
        {
            Debug.LogWarning("Skill completed, transitioning to next state.");
            ExitBlackholeState();
          
            return;
        }
        Debug.LogWarning("Skill not completed or blocked. Waiting...");
        // if (!player.skill.blackholeSkill.BlackholeSkillCompleted())
        // {
        //     Debug.LogWarning("[BlackholeState] Skill not completed. Blocking transition.");
        //     return;
        // }
        if (stateTimer <= -5.0f) // Arbitrary timeout
        {
            Debug.LogError("Timeout reached! Forcing clean state reset.");
            // 检查黑洞是否已销毁，防止崩溃
            if (player.skill.blackholeSkill==null||player.skill.blackholeSkill.BlackholeSkillCompleted())
            {
                Debug.LogWarning("skill completed, exiting blackhole state.");
                ExitBlackholeState();
                
            }

            return;
            
            
            
        }
        Debug.Log($"State Timer: {stateTimer}, Skill Used: {skillUsed}, Is Lightning Destroyed: {isLightningDestroyed}");
    }
    private void ExitBlackholeState()
    {
        Debug.LogWarning("Exiting Blackhole state.");
        rb.gravityScale = defaultGravityScale;
        PlayerManager.instance.player.anim.SetBool("Blackhole", false);
        stateMachine.ChangeState(player.straightJumpAirState);
    }
    public override void Exit()
    {
        base.Exit();
        // Reset Lightning FX
        EntityFX entityFX = PlayerManager.instance.player.GetComponent<EntityFX>();
        if (entityFX != null)
        {
            Debug.Log("Ensuring lightning FX is destroyed on state exit.");
            entityFX.ForceDestroyLightning();
        }
        
        PlayerManager.instance.player.MakeTransparent(false);
        
        // PlayerManager.instance.player.anim.Play("Idle"); // Force idle animation
    }
    
}
