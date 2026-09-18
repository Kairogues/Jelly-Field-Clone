using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public class PooledObject : MonoBehaviour
{
    private IObjectPool<GameObject> originPool;
    public IObjectPool<GameObject> OriginPool
    {
        get => originPool;
        set => originPool = value;
    }

    private IPoolable[] poolablesInChildren;



    private void Awake()
    {
        poolablesInChildren = GetComponentsInChildren<IPoolable>(true);
    }


    public void TriggerSpawn()
    {
        for (int i = 0; i < poolablesInChildren.Length; i++)
        {
            poolablesInChildren[i].OnSpawn();
        }
    }


    public void TriggerDespawn()
    {
        for (int i = 0; i < poolablesInChildren.Length; i++)
        {
            poolablesInChildren[i].OnDespawn();
        }
    }


    public void ReleaseToPool()
    {
        if (originPool != null)
        {
            originPool.Release(gameObject);
        }
        else
        {
            Debug.Log("No pool found, calling Destroy() instead!");
            TriggerDespawn();
            Destroy(gameObject);
        }
    }
}