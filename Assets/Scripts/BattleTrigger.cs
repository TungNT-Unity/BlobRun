using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleTrigger : TriggerObject
{
    public List<MonsterInBattleInfo> listEnemy;
    private int _numberMonster = 0;
    [HideInInspector] public UnityEvent OnEndBattleEvent;
    
    private void Start()
    {
        foreach (var enemy in listEnemy)
        {
            enemy.monster.OnDeadEvent.AddListener(OnMonsterDead);
        }
        _numberMonster = listEnemy.Count;
        OnTriggerEvent.AddListener(EnableBattleMode);
        OnEndBattleEvent.AddListener(OnBattleEnd);
    }

    public void OnBattleEnd()
    {
        OnEndBattleEvent.RemoveListener(OnBattleEnd);
        _isTriggered = false;
        PlayerController.Instance.ChangeCam();
    }

    public void OnMonsterDead(BaseEnemy _baseEn)
    {
        if(!_isTriggered)
            return;
        _baseEn.OnDeadEvent.RemoveListener(OnMonsterDead);
        _numberMonster -= 1;
        if (_numberMonster == 0)
        {
            OnEndBattleEvent.Invoke();
        }
    }

    public void EnableBattleMode()
    {
        OnTriggerEvent.RemoveListener(EnableBattleMode);
        PlayerController.Instance.ChangeCam();
        foreach (var enemy in listEnemy)
        {
            Vector3 pos = new Vector3(0,0,PlayerController.Instance.transform.position.z) + enemy.pos;
            enemy.monster.transform.position = pos;
            enemy.monster.transform.SetParent(PlayerController.Instance.transformFollowPlayer);
            enemy.monster.gameObject.SetActive(true);
        }
    }
    
    
}

[Serializable]
public struct MonsterInBattleInfo
{
    public BaseEnemy monster;
    public Vector3 pos;
}
