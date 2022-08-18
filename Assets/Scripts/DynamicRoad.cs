using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DynamicRoad : Road
{
    [SerializeField] private GameObject loopRoadPrefab;
    [SerializeField] private Transform endRoadGameObject;
    [SerializeField] private BattleTrigger battleTrigger;
    
    [SerializeField] private List<Transform> listRoadActive;
    [SerializeField] private List<Transform> listRoadDeactive;
    private float _originY;
    private float _loopRoadScaleZ;

    private void Start()
    {
        endRoadGameObject.gameObject.SetActive(false);
        battleTrigger.OnTriggerEvent.AddListener(OnPlayerTriggerOnRoad);
        battleTrigger.OnEndBattleEvent.AddListener(OnPlayerOutRoad);
        _loopRoadScaleZ = listRoadActive[0].localScale.z;
        _originY = listRoadActive[0].position.y;
        
    }

    public override void OnPlayerTriggerOnRoad()
    {
        base.OnPlayerTriggerOnRoad();
    }
    

    public override void OnPlayerOutRoad()
    {
        base.OnPlayerOutRoad();
        battleTrigger.OnEndBattleEvent.RemoveListener(OnPlayerOutRoad);
        Transform lastestActiveRoad = listRoadActive[listRoadActive.Count - 1];
        Vector3 targetPos = lastestActiveRoad.position + lastestActiveRoad.forward * (_loopRoadScaleZ*.5f + endRoadGameObject.transform.localScale.z*.5f);
        targetPos.y = _originY;
        endRoadGameObject.position = new Vector3(targetPos.x,_originY - 10f,targetPos.z);
        endRoadGameObject.gameObject.SetActive(true);
        endRoadGameObject.DOMove(targetPos, .3f);
    }

    private void FixedUpdate()
    {
        if(!_isPlayerOnRoad)
            return;
        if (Vector3.Distance(PlayerController.Instance.transform.position, listRoadActive[listRoadActive.Count - 1].position) < 30f)
        {
            SpawnNextRoad(1);
        }

        int countActive = listRoadActive.Count;
        for (int i = countActive - 1; i >= 0; i--)
        {
            Transform trans = listRoadActive[i];
            if (trans.position.z < PlayerController.Instance.transform.position.z &&
                Vector3.Distance(trans.position, PlayerController.Instance.transform.position) > 30f)
            {
                trans.gameObject.SetActive(false);
                listRoadDeactive.Add(trans);
                listRoadActive.Remove(trans);
            }
        }
    }

    void SpawnNextRoad(int number)
    {
        for (int i = 0; i < number; i++)
        {
            Transform cloneRoad = listRoadDeactive[0];
            Transform lastestActiveRoad = listRoadActive[listRoadActive.Count - 1];
            Vector3 targetPos = lastestActiveRoad.position + lastestActiveRoad.forward * _loopRoadScaleZ;
            targetPos.y = _originY;
            cloneRoad.position = new Vector3(targetPos.x,_originY - 10f,targetPos.z);
            cloneRoad.gameObject.SetActive(true);
            cloneRoad.DOMove(targetPos, .3f).SetDelay(i*0.1f);
            listRoadActive.Add(cloneRoad);
            listRoadDeactive.Remove(cloneRoad);
            OnSpawnRoadEvent.Invoke(id,_loopRoadScaleZ);
        }
        
    }
}
