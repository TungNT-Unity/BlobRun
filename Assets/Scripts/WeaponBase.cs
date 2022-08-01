using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int damage;
    private Dictionary<int,TargetHit> _listTargetHit = new Dictionary<int,TargetHit>();


    private void OnTriggerEnter(Collider other)
    {
        IHealth targetHeal = other.GetComponent<IHealth>();
        int id = other.gameObject.GetInstanceID();
        if (targetHeal != null && !_listTargetHit.ContainsKey(id))
        {
            TargetHit targetHit = new TargetHit();
            targetHit.Health = targetHeal;
            targetHit.Timer = 0.5f;
            targetHeal.OnDamage(damage);
            _listTargetHit.Add(other.gameObject.GetInstanceID(),targetHit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<int> removeListKey = new List<int>();
        foreach (int idKey in _listTargetHit.Keys)
        {
            _listTargetHit[idKey].Timer -= Time.deltaTime;
            if (_listTargetHit[idKey].Timer <= 0)
            {
                removeListKey.Add(idKey);
            }
        }
        foreach (int idKey in removeListKey)
        {
            _listTargetHit.Remove(idKey);
        }
        removeListKey.Clear();
    }
}

public class TargetHit
{
    public IHealth Health;
    public float Timer;
}
