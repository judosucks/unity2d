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
                    // 应用crosskick的特殊击退力
                    Vector2 crossKickForce = new Vector2(player.specialKnockbackForce, player.firstKickKnockbackYdirection);
                    // Vector2 force = player.isCrossKick
                    //     ? new Vector2(player.specialKnockbackForce,10)
                    //     : new Vector2(player.regularForce, 0);
                    hit.GetComponent<Enemy>().ApplyKnockback(crossKickForce);
                    player.HandleAttackHit();
                    Debug.Log("enemy received crosskick knockback");
                }
                else
                {
                    Vector2 regularKnockBackForce = new Vector2(player.regularForce, player.secondKickKnockbackYdirection);
                    hit.GetComponent<Enemy>().ApplyKnockback(regularKnockBackForce);
                    Debug.Log("enemy received regular knockback");
                }
                
            }

             
        }
        // 攻击完成后重置 isCrossKick
        player.isCrossKick = false;
    }
}
