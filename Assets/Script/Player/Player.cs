using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get;private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        idleState = new PlayerIdleState(this, stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine,"Move");
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    // private Rigidbody2D rb;
    // private PlayerInput playerInput;
    // private InputAction jumpAction;
    // private InputAction DashAction;
    // [Header("Movement")]
    // public float moveSpeed = 2f;
    // // 最大跳跃力
    // public float maxJumpForce = 100f;
    // // 最小跳跃力
    // public float minJumpForce = 4f;
    // // 最大耐力
    // public float maxStamina = 100f;
    // // 每次跳跃消耗的耐力
    // public float staminaCostPerJump = 20f;
    // // 每秒恢复的耐力
    // public float staminaRecoveryRate = 10f;
    // // 跳跃所需的最小耐力
    // public float minStaminaToJump = 20f;
    // private bool isJumping = false;
    // private float currentStamina;
    // private bool canJump = true;
    //
    // float moveDirection;
    //
    // [Header("GroundCheck")]
    // public Transform groundCheckPos;
    // public LayerMask whatIsGround;
    // public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    // [Header("Gravity")]
    // public float baseGravity = 9.81f;
    // public float maxFallSpeed = 18f;
    // public float fallSpeedMultiplier = 2.5f;
    // [Header("Dash info")] 
    // public float staminaCostPerDash = 30f;
    //
    // public float minStaminaToDash = 30f;
    // [SerializeField] private float dashDuration;
    // [SerializeField] private float dashTime;
    // [SerializeField] private float dashSpeed = 20f;
    // private bool isDashing = false;
    // private bool canDash = true;
    //
    // private Animator anim;
    // public bool isMoving;
    // private int facingDirection = 1;
    // private bool facingRight = true;
    // public float currentSpeed;
    //
    // private void Awake()
    // {
    //     playerInput = GetComponent<PlayerInput>();
    //     rb = GetComponent<Rigidbody2D>();
    //     anim = GetComponentInChildren<Animator>();
    //     // 添加输入动作和我们的跳跃方法的绑定
    //     jumpAction = playerInput.actions["Jump"];
    //     jumpAction.started +=  OnJump;
    //     jumpAction.performed += OnJump;
    //     jumpAction.canceled +=  OnJump;
    //     DashAction = playerInput.actions["Dash"];
    //     DashAction.performed +=  OnDash;
    //     currentStamina = maxStamina;
    // }
    //
    //
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    //     rb.linearVelocity = new Vector2(moveDirection* moveSpeed , rb.linearVelocity.y );
    //     
    //     isMoving = rb.linearVelocity.x != 0;
    //     anim.SetBool("isMoving",isMoving);
    //     anim.SetFloat("yVelocity", rb.linearVelocity.y);
    //     anim.SetBool("isGrounded",isGrounded());
    //     RecoverStamina();
    //     // Gravity();
    //     FlipController();
    //     dashTime -= Time.deltaTime;
    //     
    //     
    // }
    //
    // private void FixedUpdate()
    // {
    //     
    // }
    // public void Move(InputAction.CallbackContext context)
    // {
    //     moveDirection = context.ReadValue<Vector2>().x;
    // }
    //
    // private bool isGrounded()
    // {
    //     print("grounded");
    //     Collider2D groundCheck = 
    //         Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, whatIsGround);
    //     return groundCheck != null;
    //     
    // }
    // public void OnJump(InputAction.CallbackContext context)
    // {
    //     if (context.started && isGrounded() && canJump && currentStamina >= minStaminaToJump)
    //     {
    //         isJumping = true;
    //         
    //     }else if (context.canceled && isJumping)
    //     {
    //         // 判断按下时长以决定跳跃高度
    //         float baseJumpForce = (context.duration >= 0.1f) ? maxJumpForce : minJumpForce;
    //         float appliedJumpForce = AdjustJumpForceByStamina(baseJumpForce);
    //        // 跳跃并减少耐力
    //        // rb.linearVelocity = new Vector2(rb.linearVelocity.x, appliedJumpForce);
    //        rb.AddForce(Vector2.up * appliedJumpForce, ForceMode2D.Impulse);
    //        currentStamina -= staminaCostPerJump;
    //        CheckStaminaForActions();
    //        
    //        isJumping = false;
    //     } 
    // }
    // private float AdjustJumpForceByStamina(float baseForce)
    // {
    //     float staminaPercentage = currentStamina / maxStamina;
    //     if (staminaPercentage > 0.8f)
    //     {
    //         print("stamina full");
    //         // 最高跳跃力
    //         return baseForce;
    //     }else if (staminaPercentage < 0.6f)
    //     {
    //         // 耐力在60%-80%，跳跃力降低10%
    //         print("stamina 80");
    //         return baseForce * 0.6f;
    //     }else if (staminaPercentage > 0.4f)
    //     {
    //         // 耐力在40%-60%，跳跃力降低20%
    //         print("stamina 60");
    //         return baseForce * 0.4f;
    //     }else if (staminaPercentage > 0.2f)
    //     {
    //         // 耐力在20%-40%，跳跃力降低30%
    //         print("stamina 40");
    //         return baseForce * 0.2f;
    //     }
    //     else
    //     {
    //         // 耐力在0%-20%，跳跃力降低40%
    //         print("stamina 20");
    //         return baseForce * 0.1f;
    //     }
    // }
    // public void OnDash(InputAction.CallbackContext context)
    // {
    //     if (context.performed && canDash && currentStamina >= minStaminaToDash)
    //     {
    //         print("dash");
    //         dashTime = dashDuration;
    //         StartCoroutine(Dash());
    //
    //
    //     }
    // }
    //
    // private IEnumerator Dash()
    // {
    //    if(isDashing)
    //        yield break;
    //    
    //    
    //         // 开始冲刺
    //         isDashing = true;
    //         // 备份重力
    //         float originalGravity = rb.gravityScale;
    //         rb.gravityScale = 0f; // 暂时忽略重力
    //         rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed,0);
    //     
    //         currentStamina -= staminaCostPerDash;
    //         CheckStaminaForActions();
    //
    //         yield return new WaitForSeconds(dashTime);
    //         
    //         rb.gravityScale = originalGravity;// 恢复重力
    //         // rb.linearVelocity = Vector2.zero;
    //         isDashing = false;
    //         print("isdashing");
    //     
    //     
    // }
    //
    //
    // private void RecoverStamina()
    // {
    //     if (!isJumping && !isDashing && currentStamina < maxStamina)
    //     {
    //         currentStamina += staminaRecoveryRate * Time.deltaTime;
    //         CheckStaminaForActions();
    //         print(currentStamina);
    //         
    //     }
    // }
    //
    // private void CheckStaminaForActions()
    // {
    //     canJump = currentStamina >= minStaminaToJump;
    //     canDash = currentStamina >= minStaminaToDash;
    // }
    // private void Gravity()
    // {
    //     if (rb.linearVelocity.y < 0)
    //     {
    //         rb.gravityScale = baseGravity * fallSpeedMultiplier;
    //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y,-maxFallSpeed));
    //     }
    //     else
    //     {
    //         rb.gravityScale = baseGravity;
    //     }
    // }
    //
    // private void Flip()
    // {
    //     facingDirection = facingDirection * -1;
    //     facingRight = !facingRight;
    //     transform.Rotate(0f, 180f, 0f);
    // }
    //
    // private void FlipController()
    // {
    //     if (rb.linearVelocity.x > 0 && !facingRight)
    //     {
    //         Flip();
    //     }else if (rb.linearVelocity.x < 0 && facingRight)
    //     {
    //         Flip();
    //     }
    // }
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    // }
}