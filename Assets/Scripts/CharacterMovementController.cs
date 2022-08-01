using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathologicalGames;
using UltimateJoystickExample.Spaceship;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XftWeapon;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private float detectEnemyRange = 2f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float horizontalSpeed = 2.0f;
    [SerializeField] private float delayStepFx = 2.0f;
    [SerializeField] private Transform dirtStepFx;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject speedBuffFx;
    
    private float _currentIntervalStepTime = 0f;
    
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    Vector3 _lastMouseDownPos = Vector3.zero;
    private bool _onHold = false;
    private bool _isJumping = false;
    private float _currentRunningSpeed;
    private void Start()
    {
        _currentRunningSpeed = playerSpeed;
    }
    
    public void BuffRunningSpeed(float amount)
    {
        speedBuffFx.SetActive(true);
        _currentRunningSpeed += amount;
    }
    

    void Update()
    {
        if (!_isJumping)
        {
            JumpBoxSensor();
            Movement();
        }

    }

    void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _onHold = true;
            _lastMouseDownPos = Input.mousePosition;
        }
        Vector3 currentPos = transform.position;
        currentPos.z = Mathf.MoveTowards(currentPos.z, currentPos.z + 5, _currentRunningSpeed * Time.deltaTime);
        if (_onHold)
        {
            float deltaHorizontal = Input.mousePosition.x - _lastMouseDownPos.x;
            float valueX = currentPos.x;
            valueX = Mathf.MoveTowards(valueX, valueX + deltaHorizontal,
                horizontalSpeed * Time.deltaTime);
            if (Mathf.Abs(valueX) < 8.5f)
            {
                currentPos.x = valueX;
            }
            _lastMouseDownPos = Input.mousePosition;
        }
        transform.position = Vector3.Lerp(transform.position,currentPos,.2f);
        if (Input.GetMouseButtonUp(0))
        {
            _onHold = false;
        }
        CreateStepDirtFx();
    }

    void CreateStepDirtFx()
    {
        _currentIntervalStepTime += Time.deltaTime;
        if (_currentIntervalStepTime >= delayStepFx/_currentRunningSpeed)
        {
            _currentIntervalStepTime = 0f;
            Transform cloneDirtStepFx = PoolManager.Pools["Pool"].Spawn(dirtStepFx, transform.position + Vector3.up*.2f,
                Quaternion.identity);
            cloneDirtStepFx.gameObject.SetActive(true);
        }
    }

    void JumpBoxSensor()
    {
        int layerMask = 1 << 11;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out hit, 5, layerMask))
        {
            int layerMaskNextGround = 1 << 10;
            RaycastHit hitNextGround;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + Vector3.down + Vector3.forward * 2f, Vector3.forward, out hitNextGround, 500, layerMaskNextGround))
            {
                _isJumping = true;
                anim.SetTrigger("JumpStart");
                anim.ResetTrigger("JumpEnd");
                anim.SetFloat("Distance",hitNextGround.distance);
                Vector3 startJumPos = transform.position + Vector3.up;
                Vector3 targetJumpPos = new Vector3(0, 0, hitNextGround.point.z + 5f);
                Vector3[] jumpPath = new Vector3[3] {startJumPos,startJumPos + Vector3.forward*hitNextGround.distance*.4f + Vector3.up*hitNextGround.distance*.25f,targetJumpPos };
                transform.DOPath(jumpPath,_currentRunningSpeed*.7f,PathType.CatmullRom).SetSpeedBased(true).SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (Vector3.Distance(transform.position,targetJumpPos) < _currentRunningSpeed*.24f)
                    {
                        anim.SetTrigger("JumpEnd");
                        anim.ResetTrigger("JumpStart");
                    }
                })
                .OnComplete(() =>
                {
                    _isJumping = false;
                });
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,detectEnemyRange);
    }
    
}

