using System.Security.Cryptography;
using PathologicalGames;
using UnityEngine;
using UnityEngine.Events;

public class AutoDestroy : MonoBehaviour
{
    bool isDestroyed = false;
    [SerializeField] protected float timeDestroy;

    public float TimeDestroy
    {
        get { return timeDestroy; }
        set { timeDestroy = value;}
    }
    [SerializeField] DespawnType despawnType = DespawnType.DESPAWN;

    public bool keepAlive = false;
    
    float timeCounter = 0;

    public enum DespawnType
    { 
        DESPAWN,
        DESTROY,
        DEACTIVE
    }
    

    public void SetDespawnType(DespawnType type)
    {
        despawnType = type;
    }

    protected virtual void OnEnable()
    {
        isDestroyed = false;
        timeCounter = timeDestroy;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (timeDestroy > 0 && !isDestroyed && !keepAlive)
        {
            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
            }
            if (timeCounter <= 0)
            {
                Despawn();
            }
        }
    }

    public void DestroyImmediate(float _delay = 0f)
    {
        timeCounter = _delay;
        keepAlive = false;
    }
    protected virtual void Despawn()
    {
        if (isDestroyed)
            return;
        isDestroyed = true;
        if (despawnType == DespawnType.DESPAWN)
            PoolManager.Pools["Pool"].Despawn(transform);
        else if (despawnType == DespawnType.DESTROY)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
        
    }

    public void DespawnNow()
    {
        PoolManager.Pools["Pool"].Despawn(transform);
        gameObject.SetActive(false);
    }
}
