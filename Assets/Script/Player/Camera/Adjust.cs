using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Adjust : MonoBehaviour
{
    private Player player;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera grenadeCamera;
    public CinemachineFramingTransposer framingTransposer;
    [Header("camera info")]
    
    public float zoomSpeed = 10f;
    public float smoothTime = .01f; // 平滑过渡的时间
    public float temporaryScreenX; // 用于临时储存平滑过渡值
    [SerializeField]private float originalScreenX = 0.5f;
    public float targetScreenX;
    public float currentVelocity;
    [SerializeField] private float targetOrthoSize = 1f;
    [SerializeField] private float playerOrthoSize = 2f;
    [SerializeField] private float orthoSizeVelocity = 0f;
    public GameObject theGrenade { get;private set;}
    
    
    protected void Awake()
    {
        grenadeCamera = GameObject.FindGameObjectWithTag("GrenadeCamera").GetComponent<CinemachineVirtualCamera>();
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera is not found or doesn't have the correct tag.");
        }
        
        if (grenadeCamera == null)
        {
            Debug.LogError("GrenadeCamera is not found or doesn't have the correct tag.");
        }
    }

    protected void Start()
    {
        player = PlayerManager.instance.player;
        if (playerCamera != null)
        {
            Debug.Log("player camera found");
            
            framingTransposer = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer == null)
            {
                
                Debug.LogError("No CinemachineFramingTransposer attached to playerCamera.");
                return;
            }
            
        }
    }

    protected void Update()
    {
        
    }

    public virtual void CinemachineFollowPlayer()
    {
        SetCameraPriority(playerCamera, grenadeCamera);
    }
    public virtual void CinemachineFollowObject(GameObject target)
    {
        Debug.Log("follow grenade");
        grenadeCamera.Follow = target.transform;
        SetCameraPriority(grenadeCamera, playerCamera);
    }
    private void SetCameraPriority(CinemachineVirtualCamera mainCam, CinemachineVirtualCamera secondaryCam)
    {
        mainCam.Priority = 20;
        secondaryCam.Priority = 10;
    }
    public virtual void SmoothZoom(CinemachineVirtualCamera cam, float targetOrthoSize)
    {
        float currentOrthoSize = cam.m_Lens.OrthographicSize;
        cam.m_Lens.OrthographicSize = Mathf.SmoothDamp(
            currentOrthoSize, 
            targetOrthoSize, 
            ref orthoSizeVelocity, 
            zoomSpeed);
    }

    public void ResetZoom()
    {
        Debug.Log("reset zoom for player"+ originalScreenX+""+framingTransposer.m_ScreenX);
        framingTransposer.m_ScreenX = originalScreenX;
    }
    public void FollowGrenade(GameObject _grenade)
    {
        theGrenade = _grenade;
        Debug.Log("follow grenade");
        CinemachineFollowObject(_grenade);
        SmoothZoom(grenadeCamera,targetOrthoSize);
    }
    public virtual void FollowPlayer()
    {
        Debug.Log("follow player");
        CinemachineFollowPlayer();
        SmoothZoom(playerCamera, playerOrthoSize);
    }
    public virtual void SetScreenX(CinemachineVirtualCamera cam, float targetScreenX, float smoothTime)
    {
        
        float currentScreenX = framingTransposer.m_ScreenX;
        float velocity = 0f;
        framingTransposer.m_ScreenX = Mathf.SmoothDamp(
            currentScreenX,
            targetScreenX,
            ref velocity,
            smoothTime);
    }
}
