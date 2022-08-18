using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerObject : MonoBehaviour
{
    protected bool _isTriggered = false;
    [HideInInspector] public UnityEvent OnTriggerEvent;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(_isTriggered)
            return;
        _isTriggered = true;
        if(OnTriggerEvent != null)
            OnTriggerEvent.Invoke();
    }
}
