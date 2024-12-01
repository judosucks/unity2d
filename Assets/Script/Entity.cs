using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision info")] 
    public float attackCheckRadius;
    public Transform attackCheck;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("knockback info")] 
    [SerializeField] protected Vector2 knockbackDirection;

    protected bool isKnocked;
    [SerializeField] protected float knockbackDuration;
    protected int kickCount { get; private set; } = 0; // 计数第几次攻击
    [Header("cross kick info")]
    public Vector2 firstKickKnockbackForce;
    public Vector2 secondKickKnockbackForce;
    public float firstKickKnockbackYdirection;
    public float secondKickKnockbackYdirection;
    public bool isCrossKick;
    
    public float specialKnockbackForce = 10.0f; // 示例值，根据需要调整
    public float regularForce = 5.0f; // 示例值，根据需要调整
    [Header("ledge info")]
    [SerializeField]public Vector2 startOffset;
    [SerializeField]public Vector2 stopOffset;
    [SerializeField]protected float climbSpeed;
     public Vector2 climbBegunPosition;
    public Vector2 climbOverPosition;
    protected bool canGrabLedge = true;
    protected bool canClimb;
    public bool isHanging;
    [Header("ledge check")]
    [SerializeField]protected Transform ledgeCheck;
    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;

    #region components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    

    #endregion
    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {
        
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
        // rb.AddForce(new Vector2(knockbackForce.x * -facingDirection, knockbackForce.y));
        Debug.Log(gameObject.name+"Damage");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    public void HandleAttackHit()
    {
        kickCount++;
        Debug.Log("kickCount"+kickCount);
        if (kickCount == 1)
        {
            Debug.Log("kick1");
            rb.linearVelocity = new Vector2(firstKickKnockbackForce.x * -facingDirection,firstKickKnockbackForce.y);
        }
        else if (kickCount == 2)
        {
            Debug.Log("kick2");
            rb.linearVelocity = new Vector2(secondKickKnockbackForce.x * -facingDirection,secondKickKnockbackForce.y);
        }
        if (kickCount > 1) kickCount = 0; // 超过两次重置计数
        Debug.Log("kickcount"+kickCount);
    }
    #region velocity

    public void ZeroVelocity()
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        rb.linearVelocity = new Vector2(0f, 0f);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
        {
            return; //if knocked can not move
        }
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        if (IsGroundDetected()) FlipController(xVelocity);
    }

    #endregion
    #region collision

    public virtual bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }
    public virtual bool IsGroundDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection,
            wallCheckDistance, whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }


    #endregion
    
    #region Flip

    
    public virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight) Flip();
    }
    #endregion
    
}
