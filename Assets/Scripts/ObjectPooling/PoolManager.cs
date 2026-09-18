using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    // Map the object prefab to its pool
    // Gives a prefab, get its pool
    private Dictionary<GameObject, IObjectPool<GameObject>> pools = new();

    // Map the object instance to its PooledObject component
    // Gives a instance, get its PooledObject component
    private Dictionary<GameObject, PooledObject> instanceMap = new();



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    #region Spawn API
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (prefab == null) 
        {
            return null;
        }

        IObjectPool<GameObject> pool = GetOrCreatePool(prefab);
        GameObject instance = pool.Get();

        instance.transform.SetPositionAndRotation(position, rotation);
        if (parent != null)
        {
            instance.transform.SetParent(parent);
        }

        if (instanceMap.TryGetValue(instance, out PooledObject pooledObj))
        {
            pooledObj.TriggerSpawn();
        }

        return instance;
    }
    #endregion


    #region Release API
    public void Release(GameObject instance)
    {
        if (instance == null)
        {
            return;
        }

        if (instanceMap.TryGetValue(instance, out PooledObject pooledObj))
        {
            pooledObj.ReleaseToPool();
        }
        else
        {
            Destroy(instance);
        }
    }
    #endregion


    #region Pooling Callbacks
    private IObjectPool<GameObject> GetOrCreatePool(GameObject prefab)
    {
        if (pools.TryGetValue(prefab, out IObjectPool<GameObject> existingPool))
        {
            return existingPool;
        }

        IObjectPool<GameObject> newPool = null;

        newPool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject instance = Instantiate(prefab);

                if (instance.TryGetComponent(out PooledObject pooledObject))
                {
                    pooledObject.OriginPool = newPool;
                } else
                {
                    PooledObject newPooledObject = instance.AddComponent<PooledObject>();
                    newPooledObject.OriginPool = newPool;
                }

                instanceMap[instance] = pooledObject;

                return instance;
            },
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPoolObject,
            collectionCheck: true
        );

        pools.Add(prefab, newPool);
        return newPool;
    }


    private void OnGetFromPool(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }


    private void OnReleaseToPool(GameObject pooledObject)
    {
        if (instanceMap.TryGetValue(pooledObject, out PooledObject pooledObj))
        {
            pooledObj.TriggerDespawn();
        }

        pooledObject.SetActive(false);
    }


    private void OnDestroyPoolObject(GameObject pooledObject)
    {
        instanceMap.Remove(pooledObject);
        Destroy(pooledObject);
    }
    #endregion
}
