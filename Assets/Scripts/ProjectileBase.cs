using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private int damage;
    public float speed;
    [SerializeField] private GameObject destroyFx;
    public List<int> projectileProperties;
    private Transform _currentTarget;
    
    private void OnTriggerEnter(Collider other)
    {
        
        IHealth targetHeal = other.GetComponent<IHealth>();
        if (targetHeal != null)
        {
            TargetHit targetHit = new TargetHit();
            targetHit.Health = targetHeal;
            targetHit.Timer = 0.5f;
            targetHeal.OnDamage(damage);
            //animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>().AddForce((transform.position - _target.position).normalized*Random.Range(50f,100f) + Vector3.up*Random.Range(200f,500f),ForceMode.Impulse);
            _currentTarget = other.transform;
            if (projectileProperties.Contains(0))
            {
                FindNextBounceTarget();
            }
            if (projectileProperties.Contains(1))
            {
                //Pass throught target
            }
            if (projectileProperties.Contains(2))
            {
                //Pull target
            }

            
        }
    }

    void FindNextBounceTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20,LayerMask.GetMask("Enemy"));
        float minDist = 20;
        int indexMin = 0;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(_currentTarget == hitColliders[i].transform)
                continue;
            float dist = Vector3.SqrMagnitude(hitColliders[i].transform.position - transform.position);
            if (dist < minDist)
            {
                indexMin = i;
                minDist = dist;
            }
                
        }
        transform.rotation = Quaternion.LookRotation(hitColliders[indexMin].transform.position - transform.position);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward*speed;
    }

    private void OnDisable()
    {
        Transform cloneExplosion = PoolManager.Pools["Pool"].Spawn(destroyFx, transform.position, Quaternion.identity);
        cloneExplosion.gameObject.SetActive(true);
    }
}
