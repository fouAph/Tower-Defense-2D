using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than 1 PoolSystem Instance or Script");
        }
        Singleton = this;

        Init();
    }

    public List<GameObject> allPooledObject;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    [System.Serializable]
    public class PoolItem
    {
        public string tag;
        // public ObjectPoolType poolType;
        // public bool useAutoDisable = false;
        // public float autoDisableAfter = .2f;
        public GameObject prefab;
        public Transform parent;
        public int size;
        // public bool canExpand;
        // public bool deactiveWhenImpact;
    }

    public List<PoolItem> poolItems;

    public void Init()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolItem item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            AddObjectToPooledObject(item.prefab, item.size, item.parent, item.tag);
        }
    }

    #region For Arrays
    public GameObject[] AddObjectToPooledObjects(string newTag, GameObject[] prefabs, int size, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(newTag))
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < prefabs.Length; j++)
                {

                    GameObject obj = Instantiate(prefabs[j]);
                    if (parent)
                    {
                        obj.transform.SetParent(parent);
                        // obj.transform.localPosition = Vector3.zero;
                    }
                    else
                        obj.transform.SetParent(transform);

                    obj.SetActive(false);
                    objPool.Enqueue(obj);
                }

            }
            poolDictionary.Add(newTag, objPool);
            return prefabs;
        }
        else return null;

    }

    public GameObject SpawnFromPools(string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPools(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    #endregion

    public GameObject AddObjectToPooledObject(GameObject prefab, int size, Transform transformParent = null, string newTag = null)
    {
        if (newTag == null)
        {
            newTag = prefab.name;
        }
        if (!poolDictionary.ContainsKey(newTag))
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < size; i++)
            {

                GameObject obj = Instantiate(prefab);
                if (transformParent)
                {
                    obj.transform.SetParent(transformParent);
                    // obj.transform.localPosition = Vector3.zero;
                }
                else
                    obj.transform.SetParent(transform);

                obj.SetActive(false);
                objPool.Enqueue(obj);

            }
            poolDictionary.Add(newTag, objPool);
            return prefab;
        }

        else return null;

    }
 
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        if (parent)
        {
            objectToSpawn.transform.SetParent(parent);
        }
        else
        {
            objectToSpawn.transform.SetParent(transform.parent);
        }

        objectToSpawn.SetActive(true);
        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDictionary.ContainsKey(prefab.name))
        {
            Debug.LogWarning("Pool with tag " + prefab.name + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[prefab.name].Dequeue();

        if (parent)
            objectToSpawn.transform.SetParent(parent);

        else
            objectToSpawn.transform.SetParent(transform.parent);

        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        objectToSpawn.SetActive(true);
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[prefab.name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}