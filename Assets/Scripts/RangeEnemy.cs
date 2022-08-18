using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    [SerializeField] private float delayAttack;
    [SerializeField] private Weapon myWeapon;
    
    private float _attackTimer = 0f;
    protected override void Start()
    {
        base.Start();
        _canAction = false;
        StartCoroutine(DelayAction());
    }

    protected override void Update()
    {
        if(!_canAction || hp <= 0 || myWeapon == null)
            return;
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }

        if (_attackTimer <= 0)
        {
            _attackTimer = delayAttack;
            myWeapon.Fire();
        }
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(1.8f);
        _attackTimer = delayAttack;
        _canAction = true;
    }
}
