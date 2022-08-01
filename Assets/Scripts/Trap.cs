using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float intervalDmgTime;
    private float _curentDelayTimer = 0f;
    
    private void Update()
    {
        if (_curentDelayTimer > 0)
        {
            _curentDelayTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_curentDelayTimer <= 0)
        {
            PlayerController.Instance.OnDamage(damage);
            _curentDelayTimer = intervalDmgTime; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_curentDelayTimer <= 0)
        {
            PlayerController.Instance.OnDamage(damage);
            _curentDelayTimer = intervalDmgTime; 
        }
    }
}
