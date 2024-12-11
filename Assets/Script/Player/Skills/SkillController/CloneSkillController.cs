using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController controller;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    private Transform closestEnemy;
    [SerializeField]private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    int ATTACKNUMBER = Animator.StringToHash("AttackNumber");
    [SerializeField] private float Radius = 10f;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
   
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        if (cloneTimer < 0)
        {
            
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset)
    {
       
       
            if (_canAttack)
            {
                FaceClosestTarget();
                int Number_Attack = Random.Range(1, 5);
                anim.SetInteger(ATTACKNUMBER,Number_Attack);
                Debug.Log("can attack"+Number_Attack);
            }
            transform.position = _newTransform.position + _offset;
            cloneTimer = _cloneDuration;
            
            
        
        
       
        
    }
    private void AnimationTrigger()
    {
        Debug.Log("animation trigger from cloneskillcontroller");
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        Debug.Log("collider");
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("enemy hit from cloneskillcontroller");
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceClosestTarget()
    {
       
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("hit from cloneskillcontroller");
                float distanceToEnemy = Vector2.Distance(transform.position,hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy != null)
            {
               
                if (transform.position.x > closestEnemy.position.x)
                {
                    Debug.Log("turn right ");
                    transform.Rotate(0, 180, 0);
                }
            }
        }
    }

}
