using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathologicalGames;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BaseEnemy : MonoBehaviour,IHealth
{
    [SerializeField] protected Transform model;
    [SerializeField] protected GameObject explosionObj;
    public Animator animator; 
    [HideInInspector] public float lastTouchTime;
    [HideInInspector] public float hp;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Collider _myMainCollider;
    [SerializeField] protected Item dollarItem;
    [SerializeField] protected float attackRange = 5f;
    [HideInInspector] public MyMonsterEvent OnDeadEvent = new MyMonsterEvent();
    private Transform _target;
    private Collider[] _allMyCollider;
    protected bool _canAction = false;
    private Sequence mySequence;
    
    protected virtual void Start()
    {
        Init(PlayerController.Instance.transform,100);
        _canAction = true;
    }

    public void Init(Transform target,int hp)
    {
        //if(_allMyCollider == null)
          //  _allMyCollider = animator.GetComponentsInChildren<Collider>(true);
        _target = target;
        this.hp = hp;
    }


    /*public void OnActiveRagdoll(bool isActive)
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
        
    }*/

    protected virtual void Update()
    {
        if(!_canAction || hp <= 0)
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
        if (!_canAction || hp <= 0)
            return;
        if(mySequence != null)
            mySequence.Complete();
        mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOPunchScale(Vector3.one*.2f,.1f,1));
        mySequence.Play();
        hp -= amount;
        UIManager.Instance.CreateHpTextFloat(amount, transform.position + Vector3.up);
        if (hp <= 0)
        {
            Transform cloneExplosion = PoolManager.Pools["Pool"].Spawn("GrenadeExplosionFire", transform.position + Vector3.up, quaternion.identity);
            cloneExplosion.gameObject.SetActive(true);
            gameObject.SetActive(false);
            OnDeadEvent.Invoke(this);
            //DropItem();
            //OnActiveRagdoll(true);
            //StartCoroutine(DelayDeath());
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

[Serializable]
public class MyMonsterEvent : UnityEvent<BaseEnemy>
{
}
