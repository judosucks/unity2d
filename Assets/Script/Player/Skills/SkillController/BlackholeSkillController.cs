using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{
   [SerializeField] private GameObject hotKeyPrefab;
   [SerializeField] private List<Key> keyCodeList;//input system
   
   private float maxSize;
   private float growSpeed;
   private float shrinkSpeed;
   private float blackholeTimer;
   private bool canGrow = true;
   private bool canShrink;
   private bool canCreateHotKeys = true;
   private bool cloneAttackReleased;
   private bool playerCanDisapear = true;
   private int amountOfAttacks = 4;
   private float cloneAttackCooldown = .3f;
   private float cloneAttackTimer;
   [SerializeField] private float rotationSpeed = 100f;
   private bool shouldRotate = false;
   private List<Transform> targets = new List<Transform>();
   private List<GameObject> createdHotKey = new List<GameObject>();
   private FireOrbitController fireOrbitController;// 火焰的公轉控制腳本
   public bool playerCanExitState { get; private set;}

   private void Awake()
   {
      fireOrbitController = GetComponent<FireOrbitController>();
      if (fireOrbitController == null)
      {
         Debug.LogError("FireOrbitController script is missing! Please attach it to the GameObject.");
      }
   }

   public void StartRotation()
   {
      shouldRotate = true;
   }
   public void StopRotation()
   {
      shouldRotate = false;
   }
   public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _amountOfAttacks,float _cloneAttackCooldown,float _blackholeDuration)
   {
      Debug.Log("setup blackhole");
      maxSize = _maxSize;
      growSpeed = _growSpeed;
      shrinkSpeed = _shrinkSpeed;
      amountOfAttacks = _amountOfAttacks;
      cloneAttackCooldown = _cloneAttackCooldown;
      blackholeTimer = _blackholeDuration;
   }
   private void Update()
   {
      blackholeTimer -= Time.deltaTime;
      cloneAttackTimer -= Time.deltaTime;
      if (shouldRotate)
      {
         transform.Rotate(0,0, rotationSpeed * Time.deltaTime);
      }
      if (blackholeTimer < 0)
      {
         blackholeTimer = Mathf.Infinity;

         if (targets.Count > 0)
         {
            ReleaseCloneAttack();
         }
         else
         {
            FinishBlackholeAbility();
         }
      }
      
      if (Keyboard.current.rKey.wasPressedThisFrame)
      {
         ReleaseCloneAttack();
      }

      CloneAttackLogic();
      if (canGrow && !canShrink)
      {
         Debug.Log("Growing " + amountOfAttacks + " attacks");
      transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growSpeed);
         StartRotation();
        
         
      }

      if (canShrink)
      {
         Debug.Log("Shrinking");
         if (transform != null)
         {
            Debug.Log("transform not null");
         transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * shrinkSpeed);
         
         if (transform.localScale.x < 0)
         {
            Debug.Log("destroy blackhole");
            Destroy(gameObject);
         }
         }

      }
      
   }

   private void ReleaseCloneAttack()
   {
      if (targets.Count <= 0)
      {
         Debug.Log("no targets");
         return;
      }
      Debug.Log("release clone");
      DestroyHotKey();
      cloneAttackReleased = true;
      canCreateHotKeys = false;
      if (playerCanDisapear)
      {
        playerCanDisapear = false;
        PlayerManager.instance.player.MakeTransparent(true);
      }
   }

   private void CloneAttackLogic()
   {
      Debug.Log("cloneattack logic");
      if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
      {
         cloneAttackTimer = cloneAttackCooldown;
         
         int randomIndex = Random.Range(0, targets.Count);

         float xOffset;

         if (Random.Range(0, 100) > 50)
         {
            xOffset = 1;
         }
         else
         {
            xOffset = -1;
         }
         SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex],new Vector3(xOffset,0));

         amountOfAttacks--;
         
         if (amountOfAttacks <= 0)
         {
            Debug.Log("attacks over"+amountOfAttacks);
            Invoke("FinishBlackholeAbility",.5f);;
         }
           
         
         
      }
   }

   private void FinishBlackholeAbility()
   {
      Debug.Log("finish blackhole ability");
      StopRotation();
      DestroyHotKey();
      playerCanExitState = true;
      canShrink = true;
      
      cloneAttackReleased = false;
   }

   private void DestroyHotKey()
   {
      Debug.Log("destroy hotkey");
      if (createdHotKey.Count <= 0)
      {
         return;
      }

      for (int i = 0; i < createdHotKey.Count; i++)
      {
         Destroy(createdHotKey[i]);
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      
      if (other.GetComponent<Enemy>() != null)
      {
         Debug.Log("number of targets"+targets.Count);
         other.GetComponent<Enemy>().FreezeTime(true);
         AddEnemyToList(other.transform);
         CreateHotkey(other);
        
      }
      
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      Debug.Log("exit trigger");
      other.GetComponent<Enemy>()?.FreezeTime(false);
      
   }
      

   private void CreateHotkey(Collider2D other)
   {
      Debug.Log("create hotkey");
      if (keyCodeList.Count <= 0)
      {
         Debug.Log("no more keys");
         return;
      }

      if (!canCreateHotKeys)
      {
         Debug.Log("no hotkey");
         return;
      }
      GameObject newHotKey =
         Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
      createdHotKey.Add(newHotKey);
      
      Key choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
      keyCodeList.Remove(choosenKey);
      BlackholeHotkeyController newHotkeyScript = newHotKey.GetComponent<BlackholeHotkeyController>();
      newHotkeyScript.SetHotKey(choosenKey,other.transform,this);
   }

   public void AddEnemyToList(Transform _enemyTransforms) => targets.Add(_enemyTransforms);
}
