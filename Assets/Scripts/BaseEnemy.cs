using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseEnemy : MonoBehaviour,IHealth
{
    [SerializeField] private Transform model;
    [SerializeField] private GameObject explosionObj;
    public Animator animator; 
    [HideInInspector] public float lastTouchTime;
    [HideInInspector] public float hp;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _myMainCollider;
    [SerializeField] private Item dollarItem;
    [SerializeField] private float attackRange = 5f;
    private Transform _target;
    private Collider[] _allMyCollider;
    private void Start()
    {
        Init(PlayerController.Instance.transform,1);
    }

    public void Init(Transform target,int hp)
    {
        if(_allMyCollider == null)
            _allMyCollider = animator.GetComponentsInChildren<Collider>(true);
        _target = target;
        this.hp = hp;
        OnActiveRagdoll(false);
    }


    public void OnActiveRagdoll(bool isActive)
    {
        foreach (var col in _allMyCollider)
        {
            col.enabled = isActive;
            col.GetComponent<Rigidbody>().isKinematic = !isActive;
        }
        _myMainCollider.enabled = !isActive;
        _rigidbody.isKinematic = isActive;
        _rigidbody.useGravity = !isActive;
        animator.enabled = !isActive;
    }

    public void AddForce(Vector3 dir)
    {
        _rigidbody.AddForce(dir,ForceMode.Impulse);
        
    }

    private void Update()
    {
        if(hp <= 0)
            return;
        Vector3 dir =_target.position - transform.position;
        if (Vector3.SqrMagnitude(dir) < attackRange*attackRange)
        {
            model.rotation = Quaternion.LookRotation(dir);
            animator.SetInteger("Attack",Random.Range(1,4));
        }
    }

    public void OnDamage(int amount)
    {
        if (hp <= 0)
            return;
        hp -= amount;
        UIManager.Instance.CreateHpTextFloat(amount, transform.position + Vector3.up);
        if (hp <= 0)
        {
            //DropItem();
            OnActiveRagdoll(true);
            StartCoroutine(DelayDeath());
        }
    }

    IEnumerator DelayDeath()
    {
        animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>().AddForce((transform.position - _target.position).normalized*Random.Range(200f,400f) + Vector3.up*Random.Range(200f,500f),ForceMode.Impulse);
        yield return new WaitForSeconds(3f);
        PoolManager.Pools["Pool"].Despawn(transform);
    }

    protected void DropItem()
    {
        Transform cloneExplosion = PoolManager.Pools["Pool"].Spawn(dollarItem.transform, animator.GetBoneTransform(HumanBodyBones.Hips).position + Vector3.up, quaternion.identity);
        Vector3 euler = cloneExplosion.eulerAngles;
        euler.y = Random.Range(0, 180);
        cloneExplosion.eulerAngles = euler;
        cloneExplosion.gameObject.SetActive(true);
    }

    public void OnHealth(int amount)
    {
        hp += 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
