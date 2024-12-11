using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

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
        Debug.Log("animation trigger climb grab"+player.isHanging);
        player.isHanging = true;
    }
    private void AnimationFinishEvent()
    {
        Debug.Log("animation finish");
        player.ledgeClimbState.AnimationFinishTrigger();
    }

    

    
    // private void AttackTrigger()
    // {
    //     Debug.Log("AttackTrigger fired.");
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position,player.attackCheckRadius);
    //     foreach (var hit in colliders)
    //     {
    //         if (hit.GetComponent<Enemy>() != null)
    //         {
    //             Debug.Log("Enemy hit!");
    //             hit.GetComponent<Enemy>().Damage();
    //             player.HandleAttackHit(); // 触发攻击命中后调用
    //         }
    //     }
    // }
    // private void AttackTrigger()
    // {
    //     Collider2D[] colliders =
    //         Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
    //     foreach (var hit in colliders)
    //     {
    //         if (hit.GetComponent<Enemy>() != null)
    //         {
    //             Debug.Log("enemy hit");
    //             hit.GetComponent<Enemy>().Damage();
    //             if (player.isCrossKick)
    //             {
    //                 // 应用crosskick的特殊击退力
    //                 Vector2 crossKickForce = new Vector2(player.specialKnockbackForce, player.firstKickKnockbackYdirection);
    //                 // Vector2 force = player.isCrossKick
    //                 //     ? new Vector2(player.specialKnockbackForce,10)
    //                 //     : new Vector2(player.regularForce, 0);
    //                 hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
    //                 player.HandleAttackHit();
    //                 Debug.Log("enemy received crosskick knockback");
    //             }
    //             else
    //             {
    //                 Vector2 regularKnockBackForce = new Vector2(player.regularForce, player.secondKickKnockbackYdirection);
    //                 hit.GetComponent<Enemy>().ApplyKnockback(regularKnockBackForce);
    //                 Debug.Log("enemy received regular knockback");
    //             }
    //             
    //         }
    //
    //          
    //     }
    //     // 攻击完成后重置 isCrossKick
    //     player.isCrossKick = false;
    // }
    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("enemy hit");
                hit.GetComponent<Enemy>().Damage();

                if (player.isCrossKick)
                {
                    // 应用 CrossKick 的击打逻辑
                    Vector2 crossKickForce = new Vector2(player.specialKnockbackForce , player.firstKickKnockbackYdirection);
                    hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
                    Debug.Log("enemy received crosskick knockback");
                }
                else
                {
                    // 应用常规连击的击打逻辑
                    Vector2 regularKnockBackForce = new Vector2(player.regularForce , player.regularForceY); 
                    hit.GetComponent<Enemy>().ApplyKnockback(regularKnockBackForce);
                    Debug.Log("enemy received regular knockback");
                }

                if (player.isKneeKick)
                {
                    Vector2 kneeKickForce = new Vector2(player.kneeKickKnockbackDirection.x,
                        player.kneeKickKnockbackDirection.y);
                    hit.GetComponent<Enemy>().ApplyKnockback(kneeKickForce);
                    Debug.Log("enemy received knee kick knockback");
                }
            }
        }
        player.isCrossKick = false;
        player.isKneeKick = false;
    }

    private void ThrowGrenadeEvent()
    {
        Debug.Log("throw grenade");
        SkillManager.instance.grenadeSkill.CreateGrenade();
    }
}
