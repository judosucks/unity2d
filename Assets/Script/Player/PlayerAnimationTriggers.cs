using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    [SerializeField] private GameObject playerGameObject; // Ensure this is referenced correctly in the inspector.
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void DashEvent()
    {
        player.OnDashAttackFrame();
    }

    private void DashEndEvent()
    {
        player.OnDashAttackComplete();
    }

    private void CrossKickEvent()
    {
        player.OnPerformCrossKick();
    }

    private void CrossKickEndEvent()
    {
        player.OnCrossKickComplete();
    }
    private void AnimationTriggerClimbEvent()
    {
       
        player.isHanging = true;
    }
    private void AnimationFinishEvent()
    {
        
        player.ledgeClimbState.AnimationFinishTrigger();
    }


    private void AnimationLighteningTrigger()
    {
        if (PlayerManager.instance != null && PlayerManager.instance.player != null)
        {
            var currentState = PlayerManager.instance.player.stateMachine.currentState;

            // Skip triggering if the skill is complete or state is transitioning
            if (PlayerManager.instance.player.skill.blackholeSkill.BlackholeSkillCompleted())
            {
                
                return;
            }

            if (currentState == PlayerManager.instance.player.blackholeState)
            {
                player.GetComponent<EntityFX>().PlayerLightningFx(player.transform);
                
            }
            
        }
        
    }
    
    
    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);
                
               
                
                if (player.isCrossKick)
                {
                    // 应用 CrossKick 的击打逻辑
                    Vector2 crossKickForce = new Vector2(player.specialKnockbackForce , player.firstKickKnockbackYdirection);
                    hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
                    player.stats.DoDamage(_target);
                    Debug.Log("enemy received crosskick knockback");
                }
                

                if (player.isKneeKick)
                {
                    Vector2 kneeKickForce = new Vector2(player.kneeKickKnockbackDirection.x,
                        player.kneeKickKnockbackDirection.y);
                    hit.GetComponent<Enemy>().ApplyKnockback(kneeKickForce);
                    player.stats.DoDamage(_target);
                    Debug.Log("enemy received knee kick knockback");
                }
            }
        }
        player.isCrossKick = false;
        player.isKneeKick = false;
    }

    private void ThrowGrenadeEvent()
    {
        
        SkillManager.instance.grenadeSkill.CreateGrenade();
    }
}
