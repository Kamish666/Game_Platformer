using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{

    [System.Serializable]
    private class Pool
    {
        public string tag;
        public GameObject bullet;
        public int amount;
    }

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
            Queue<GameObject> objectDictionfry = new Queue<GameObject>();
            for (int i = 0; i < pool.amount; i++)
            {
                GameObject obj = Instantiate(pool.bullet);
                obj.SetActive(false);
                objectDictionfry.Enqueue(obj);
            }
            //_poolDictionary[pool.tag] = objectDictionfry;
            _poolDictionary.Add(pool.tag, objectDictionfry);
        }
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
