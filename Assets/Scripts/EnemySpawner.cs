using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathologicalGames;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform playerTrans;

    [SerializeField] private float intervalSpawnTime = .2f;
    
    [SerializeField] public FormationRenderer formation;

    private float _currentIntervalTime = 0f;

    private List<Vector3> _listPoints;
    // Start is called before the first frame update
    void Start()
    {
        _currentIntervalTime = 0f;
        _listPoints = formation.Formation.EvaluatePoints().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        _currentIntervalTime += Time.deltaTime;
        if (_currentIntervalTime > intervalSpawnTime)
        {
            Transform cloneEnemy = PoolManager.Pools["Pool"].Spawn(enemyPrefab);
            cloneEnemy.localScale = Vector3.one*1.5f;
            cloneEnemy.position = playerTrans.position + _listPoints[Random.Range(0, _listPoints.Count)];
            cloneEnemy.GetComponent<BaseEnemy>().Init(playerTrans,1);
            _currentIntervalTime = 0;
            cloneEnemy.gameObject.SetActive(true);
        }
    }
}
