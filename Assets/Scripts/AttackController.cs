using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathologicalGames;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    public GameObject projectilePrefab;
    public float delayAttackTime = .5f;
    public float detectEnemyRange = 10f;
    
    private float _currentDelay = 0f;

    // Update is called once per frame
    void Update()
    {
        _currentDelay -= Time.deltaTime;
        int layerMask = 1 << 6;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(firePoint.position, transform.forward, out hit, detectEnemyRange, layerMask))
        {
            if (_currentDelay <= 0)
            {
                _currentDelay = delayAttackTime;
                Transform cloneProjectile = PoolManager.Pools["Pool"].Spawn(projectilePrefab, firePoint.position,Quaternion.LookRotation(firePoint.forward));
                cloneProjectile.localScale = Vector3.one*.3f;
                cloneProjectile.gameObject.SetActive(true);
                cloneProjectile.DOScale(Vector3.one, .3f);
            }
        }
        
        
    }
    
}
