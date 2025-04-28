using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BulletPooler;

public class BulletPooler : MonoBehaviour
{
    [SerializeField] private List<Pool> _pools = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public static BulletPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        foreach (Pool pool in _pools)
        {
            AddBullets(pool);
        }
    }

    public void FindBulletTag(string tag, int amount)
    {
        Pool necessaryPool = new Pool();

        foreach (Pool pool in _pools)
        {
            if (pool.tag == tag)
            {
                necessaryPool = pool;
                break;
            }
        }

        necessaryPool.amount = amount;

        AddBullets(necessaryPool);
    }

    private void AddBullets(Pool pool)
    {
        Queue<GameObject> objectDictionary = new Queue<GameObject>();
        for (int i = 0; i < pool.amount; i++)
        {
            GameObject obj = Instantiate(pool.bullet);
            obj.SetActive(false);
            objectDictionary.Enqueue(obj);
        }
        //_poolDictionary[pool.tag] = objectDictionfry;
        if (_poolDictionary.ContainsKey(pool.tag))
            foreach (var obj in objectDictionary)
            {
                _poolDictionary[pool.tag].Enqueue(obj);
            }
        else
            _poolDictionary.Add(pool.tag, objectDictionary);
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Ошибка. Тег не найден");
            return null;
        }


        GameObject objSpawn = _poolDictionary[tag].Dequeue();

        objSpawn.SetActive(true);
        objSpawn.transform.position = position;
        objSpawn.transform.rotation = rotation;

        _poolDictionary[tag].Enqueue(objSpawn);

        return objSpawn;
    }
}


[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject bullet;
    public int amount;
}