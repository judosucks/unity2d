using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityFX : MonoBehaviour
{
   
   private SpriteRenderer sr;
   [Header("Flash FX")] [SerializeField] private Material hitMat;
   private Material originalMat;

   [Header("player fx")] 
   [SerializeField] private GameObject lightningFx;
   [SerializeField] private float yOffset;
   [SerializeField] private float lightningDuration;
   private bool canDestroyLightning = false;
   private GameObject currentLightning;
   private Keyboard keyboard= Keyboard.current;
   public bool hasDestroyedLightning { get; private set; }= false;

   private void Awake()
   {
      
   }
   
   public void PlayerLightningFx(Transform _playerTransform)
   {
      // Destroy any existing lightning FX before creating a new one.
      if (currentLightning != null)
      {
        
         Destroy(currentLightning);
      }
      Vector3 lightningStartPosition = _playerTransform.position + new Vector3(0, yOffset, 0);
      currentLightning = Instantiate(lightningFx, lightningStartPosition, Quaternion.identity);
      currentLightning.transform.SetParent(_playerTransform);

      

      if (keyboard != null)
      {
        
         if (keyboard.rKey.wasPressedThisFrame)
         {
            Debug.LogWarning("kKey was pressed");
            Destroy(currentLightning); // Immediate destruction if R key is pressed
            hasDestroyedLightning = true;
            canDestroyLightning = false;
            return;
         }
      }
      
      hasDestroyedLightning = false; // Ensure proper state reset
      canDestroyLightning = true;
      Destroy(currentLightning, lightningDuration);
   }

   public void ForceDestroyLightning()
   {
      if (hasDestroyedLightning)
      {
         
         return; // Prevent duplicate execution
      }



      if (currentLightning != null)
      {
         
         Destroy(currentLightning);
         currentLightning = null;
      }

      hasDestroyedLightning = true;
   }

   public void ResetHasDestroyedLightning()
   {
      hasDestroyedLightning = false;
   }

   private void Start()
   {
      if (keyboard == null)
      {
         
         keyboard = Keyboard.current;
         return;
      }
      
      sr = GetComponentInChildren<SpriteRenderer>();
      originalMat = sr.material;
   }

   private IEnumerator FlashFX()
   {
      sr.material = hitMat;
      yield return new WaitForSeconds(0.2f);
      sr.material = originalMat;
   }

   private void RedColorBlink()
   {
      if (sr.color != Color.white)
      {
         sr.color = Color.white;
      }
      else
      {
         sr.color = Color.red;
      }
   }

   private void CancelRedBlink()
   {
      CancelInvoke();
      sr.color = Color.white;
   }
}
