using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GrenadeSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D _circleCollider2D;
    private Player player;
  
    
    
    private bool explosion;

    [Header("grenade timer")] 
    [SerializeField]private float explosionTimer;
    [Header("frag grenade")] 
    private List<Transform> enemyTarget;
    private bool isFragGrenade;
    [Header("flash grenade")] 
    private bool isFlashGrenade;
    private int targetIndex;
    private void Awake()
    {
        
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        
        Debug.Log("awake sword skill controller"+anim+" "+rb+" "+_circleCollider2D);
    }

    private void Start()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
            Debug.Log("anim is null"+" "+anim);
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            Debug.Log("rb is null"+" "+rb);
        }

        if (_circleCollider2D == null)
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            Debug.Log("circlecollider2d is null"+" "+_circleCollider2D);
        }
        else if(anim && rb && _circleCollider2D != null)
        {
            Debug.Log("not null");
        }
    }

    private void Update()
    {
        // if (canRotate)
        // {
        //     Debug.Log("can rotate");
        //     transform.right = rb.linearVelocity;
        // }

        // if (explosion == true)
        // {
        //     Debug.Log("follow player because exploded "+explosion);
        //     // CameraManager.instance.newCamera.FollowPlayer();
        // }
        
    }

    public void SetupGrenade(Vector2 _direction, float _gravity, Player _player)
    {
        Debug.Log("setup grenade");
        if (player == null || rb == null)
        {
            Debug.Log("player or rb is null");
            player = PlayerManager.instance.player;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            if (player && rb && _circleCollider2D != null && anim != null)
            {
                Debug.Log("player rb anim col not null");
                player = _player;
                rb.linearVelocity = _direction;
                rb.gravityScale = _gravity;
                anim.SetBool("Rotation",true);
                
                StartCoroutine(StartExplosionTimer());

            }
            
        }
        
    }
    private IEnumerator StartExplosionTimer()
    {
        yield return new WaitForSeconds(explosionTimer); // 3秒后爆炸
        explosion = true;
            Debug.Log("explosiontriggered");
                
            anim.SetBool("Rotation",false);
            
            _circleCollider2D.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
                
        
        GrenadeExplosion(transform.position);
        
    }
    public void SetupFragGrenade(bool _isFragGrenade)
    {
        Debug.Log("set up frag grenade");
        isFragGrenade = _isFragGrenade;
        enemyTarget = new List<Transform>();
    }

    public void SetupFlashGrenade(bool _isFlashGrenade)
    {
        Debug.Log("set up flash grenade");
        isFlashGrenade = _isFlashGrenade;
        enemyTarget = new List<Transform>();
    }
    public void ReturnGrenade()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy"))
        {
            // 减少弹跳力
            // 调整因子以获得所需的反弹效果
            Debug.Log("on ground or enemy should bounce");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,Mathf.Abs(rb.linearVelocity.y) * 0.7f); // 例如，把垂直速度减少到原来的70%
            // 其他与反弹相关的逻辑
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //    
    //     // collision.GetComponent<Enemy>().Damage();
    //     // if (collision.GetComponent<Enemy>() != null)
    //     // {
    //     //     
    //     //     if (isFragGrenade && enemyTarget.Count <= 0)
    //     //     {
    //     //         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
    //     //         foreach (var hit in colliders)
    //     //         {
    //     //             if (hit.GetComponent<Enemy>() != null)
    //     //             {
    //     //                 enemyTarget.Add(hit.transform);
    //     //             }
    //     //         }
    //     //     }
    //     // }
    //     if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
    //     {
    //         Debug.Log("grenade on trigger enter enemy or ground");
    //        
    //        
    //         
    //     }
    //     
    // }
    // 定义触发生命的逻辑
    private void GrenadeExplosion(Vector2 position)
    {
        player.ClearGrenade();    
        explosion = false;
        
        
        _circleCollider2D.enabled = true;
        transform.parent = null;
        
        rb.bodyType = RigidbodyType2D.Dynamic;
        // 例如：生成粒子特效、伤害附近的敌人等
        Debug.Log("Explosion triggered at position: " + position);
        // 例如：Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}
