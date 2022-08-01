using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class Item : MonoBehaviour
{
    protected bool _canCollect = false;

    private void OnEnable()
    {
        _canCollect = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.SqrMagnitude(PlayerController.Instance.transform.position - transform.position);
        if (distance < PlayerController.Instance.itemCollectRange)
            _canCollect = true;
        if (_canCollect)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerController.Instance.transform.position, .07f);
            if (distance <= 6)
            {
                PoolManager.Pools["Pool"].Despawn(transform);
            }
        }
    }
}
