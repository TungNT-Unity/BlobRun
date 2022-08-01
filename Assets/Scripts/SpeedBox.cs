using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBox : MonoBehaviour
{
    [SerializeField] private float speedBonus = 50f;
    private bool _isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!_isTriggered)
        {
            PlayerController.Instance.playerMovement.BuffRunningSpeed(speedBonus);
            _isTriggered = true;
        }
    }
}
