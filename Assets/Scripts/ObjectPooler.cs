using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public GameObject storeHere;
    public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPool;
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> pooledObjectsByTag;


    void Awake()
    {
        SharedInstance = this;
    }

    void OnEnable()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            GameObject holder = item.storeHere;
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.parent = holder.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    private void OnDisable()
    {
        pooledObjects = null;
        pooledObjectsByTag = null;
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag.Contains(tag))
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag.Contains(tag))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }

    public List<GameObject> GetAllPooledObjectsByTag(string tag)
    {

        pooledObjectsByTag = new List<GameObject>();
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy && pooledObjects[i].tag.Contains(tag))
        {
                pooledObjectsByTag.Add(pooledObjects[i]);
            }
        }

        return pooledObjectsByTag;
    }
}