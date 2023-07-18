using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get { return instance; }
    }
    public static Dictionary<string, List<GameObject>> poolDict { get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        poolDict = new Dictionary<string, List<GameObject>>();
    }
    public static void AddPool(string key, GameObject objectToPool, int poolSize, GameObject parent)
    {
        List<GameObject> pool = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < poolSize; i++)
        {
            temp = Instantiate(objectToPool, parent.transform);
            temp.SetActive(false);
            pool.Add(temp);
        }
        poolDict.Add(key, pool);
    }
    public static GameObject GetPooledObject(string key)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (poolDict[key][i].activeInHierarchy)
            {
                return poolDict[key][i];
            }
        }
        return null;
    }
    public static GameObject GetPooledObject(string key, Vector3 pos)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (poolDict[key][i].activeInHierarchy && Vector2.Distance(poolDict[key][i].transform.position, pos) < 0.01f)
            {
                return poolDict[key][i];
            }
        }
        return null;
    }
    public static bool IsObjectAvailable(string key)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (!poolDict[key][i].activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
    public static void PullFromPool(string key)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (!poolDict[key][i].activeInHierarchy)
            {
                poolDict[key][i].SetActive(true);
                return;
            }
        }
    }
    public static void PullFromPool(string key, Vector3 pos)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (!poolDict[key][i].activeInHierarchy)
            {
                poolDict[key][i].SetActive(true);
                poolDict[key][i].transform.position = new Vector3(pos.x, pos.y, poolDict[key][i].transform.position.z);
                return;
            }
        }
    }
    public static void ResetPool(string key)
    {
        for (int i = 0; i < poolDict[key].Count; i++)
        {
            if (poolDict[key][i].activeInHierarchy)
            {
                poolDict[key][i].SetActive(false);
            }
        }
    }
    public static void RemovePoolKey(string key)
    {
        poolDict.Remove(key);
    }
}
