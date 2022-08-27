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
    [SerializeField] private Collider col;
    [SerializeField] private Rigidbody rig;
    private float _currentIntervalStepTime = 0f;
    
    private Vector3 playerVelocity;
    private bool _groundedPlayer;
    Vector3 _lastMouseDownPos = Vector3.zero;
    private bool _onHold = false;
    private bool _isJumping = false;
    private float _currentRunningSpeed;
    private Vector3 targetJumpPos;
    private void Start()
    {
        _currentRunningSpeed = playerSpeed;
    }
    
    public void BuffRunningSpeed(float amount)
    {
        speedBuffFx.SetActive(true);
        _currentRunningSpeed += amount;
    }

    private void Update()
    {
        GroundCheck();
        if (_isJumping)
        {
            if (Vector3.Distance(transform.position,targetJumpPos) < _currentRunningSpeed*.24f)
            {
                anim.SetTrigger("JumpEnd");
                anim.ResetTrigger("JumpStart");
                _isJumping = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            _onHold = true;
            _lastMouseDownPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _onHold = false;
        }
    }

    void FixedUpdate()
    {
        if (!_isJumping)
        {
            JumpBoxSensor();
            if(!_isJumping)
                Movement();
        }

    }

    void GroundCheck()
    {
        int layerMask = 1 << 10;
        RaycastHit hit;
        _groundedPlayer = Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, .6f, layerMask);
        Debug.DrawLine(transform.position + Vector3.up,transform.position + Vector3.down*.6f,Color.green);
    }

    public void JumpPhysic(Vector3 targetPos, float initialAngle)
    {
        rig.useGravity = true;
        Vector3 p = targetPos;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        
        // Fire!
        rig.velocity = finalVelocity;
    }

    void Jump(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;

// Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = _currentRunningSpeed * _currentRunningSpeed + Vector3.Dot(toTarget, Physics.gravity);    
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

// Check whether the target is reachable at max speed or less.
        if(discriminant < 0) {
            // Target is too far away to hit at this speed.
            // Abort, or fire at max speed in its general direction?
        }

        float discRoot = Mathf.Sqrt(discriminant);

// Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

// Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

// Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f/gSquared));

        float T = T_lowEnergy;// choose T_max, T_min, or some T in-between like T_lowEnergy

// Convert from time-to-hit to a launch velocity:
            Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;

// Apply the calculated velocity (do not use force, acceleration, or impulse modes)
        rig.velocity = velocity;
    }


    void Movement()
    {
        /*Vector3 currentPos = transform.position;
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
        transform.position = Vector3.Lerp(transform.position,currentPos,.2f);*/
        Vector3 currentVelocity = rig.velocity;
        currentVelocity.z = _currentRunningSpeed;
        if (_onHold)
        {
            float deltaHorizontal = Input.mousePosition.x - _lastMouseDownPos.x;
            currentVelocity.x = deltaHorizontal * horizontalSpeed;
            _lastMouseDownPos = Input.mousePosition;
        }
        //rig.velocity = Vector3.Lerp(rig.velocity ,currentVelocity,.2f);
        rig.velocity = currentVelocity;
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
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.forward, out hit, 5, layerMask))
        {
            int layerMaskNextGround = 1 << 10;
            RaycastHit hitNextGround;
            if (Physics.Raycast(transform.position + Vector3.down + Vector3.forward * 2f, Vector3.forward, out hitNextGround, 500, layerMaskNextGround))
            {
                _isJumping = true;
                anim.SetTrigger("JumpStart");
                anim.ResetTrigger("JumpEnd");
                anim.SetFloat("Distance",hitNextGround.distance);
                Vector3 startJumPos = transform.position + Vector3.up;
                targetJumpPos = new Vector3(0, 0, hitNextGround.point.z + 5f);
                Jump(targetJumpPos);
            }
        }
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,detectEnemyRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetJumpPos,2f);
    }
    
}

