using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Road : MonoBehaviour
{
    public int id;
    protected bool _isPlayerOnRoad = false;
    [HideInInspector] public MyIntFloatEvent OnSpawnRoadEvent;

    public virtual void OnPlayerTriggerOnRoad()
    {
        if(_isPlayerOnRoad)
            return;
        _isPlayerOnRoad = true;
    }

    public virtual void OnPlayerOutRoad()
    {
        if(!_isPlayerOnRoad)
            return;
        _isPlayerOnRoad = false;
    }
}



