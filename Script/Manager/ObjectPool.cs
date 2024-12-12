using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary; //MonsterController�� ������

    private void Awake()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            // ������ ���� Dictionary�� ���
            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;


        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }

    public void DestroyAllObjectsInPool()
    {
        foreach (var pool in PoolDictionary)
        {
            foreach (var obj in pool.Value)
            {
                Destroy(obj);
            }
            pool.Value.Clear(); // Ǯ�� ���ϴ�.
        }
    }
    public GameObject SpawnFromPool(string tag, MonsterSO monsterSO)
    {
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);
        obj.GetComponent<MonsterController>().monsterSO = monsterSO;

        obj.SetActive(true);
        return obj;
    }

    public void DisableAllObjectsInPool()
    {
        foreach (var pool in PoolDictionary)
        {
            foreach (var obj in pool.Value)
            {
                obj.SetActive(false);
            }
        }
    }
}