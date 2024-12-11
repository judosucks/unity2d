using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravityScale;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
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
            Debug.Log("state timer > 0 form blackhole");
            rb.linearVelocity = new Vector2(0,3);
            
        }

        if (stateTimer < 0)
        {
            Debug.Log("state timer < 0 form blackhole");
            rb.linearVelocity = new Vector2(0, 0);
            
            
            if (!skillUsed)
            {
                Debug.Log("cast blackhole");
                if (player.skill.blackholeSkill.CanUseSkill())
                {
                    Debug.Log("canuse skill");
                   
                  skillUsed = true;
                }
                
            }
        }

        if (player.skill.blackholeSkill.BlackholeSkillCompleted())
        {
            Debug.Log("skill completed");
            player.rb.gravityScale = defaultGravityScale;
            PlayerManager.instance.player.anim.SetBool("Blackhole",false);
            stateMachine.ChangeState(player.straightJumpAirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        PlayerManager.instance.player.MakeTransparent(false);
        
        
       
        
    }
    
}
