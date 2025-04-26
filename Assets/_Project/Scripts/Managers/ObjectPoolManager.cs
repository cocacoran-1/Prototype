using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [SerializeField] private List<Pool> pools;
        private Dictionary<string, Queue<GameObject>> poolDictionary;
        private Dictionary<string, Transform> poolParents;
        private float lastExpandTime;
        private float expandCooldown = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            poolParents = new Dictionary<string, Transform>();

            foreach (Pool pool in pools)
            {
                GameObject parentObj = new GameObject($"{pool.tag}Pool");
                parentObj.transform.SetParent(transform);
                poolParents.Add(pool.tag, parentObj.transform);

                Queue<GameObject> objectPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(parentObj.transform);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            if (poolDictionary[tag].Count == 0)
            {
                Debug.Log($"Expanding pool {tag}");
                ExpandPool(tag);
            }

            GameObject obj = poolDictionary[tag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            Debug.Log($"Spawned {tag} at {position}, Active = {obj.activeSelf}, Position = {obj.transform.position}");

            if (poolParents.TryGetValue(tag, out Transform parent))
            {
                obj.transform.SetParent(parent);
            }

            poolDictionary[tag].Enqueue(obj);
            return obj;
        }
        private void ExpandPool(string tag)
        {
            if (Time.time - lastExpandTime < expandCooldown) return;
            lastExpandTime = Time.time;
            foreach (Pool pool in pools)
            {
                if (pool.tag == tag)
                {
                    GameObject parentObj = poolParents[tag].gameObject;
                    for (int i = 0; i < pool.size / 2; i++) // 기존 크기의 절반만큼 추가
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.SetActive(false);
                        obj.transform.SetParent(parentObj.transform);
                        poolDictionary[tag].Enqueue(obj);
                    }
                    Debug.Log($"Expanded pool {tag} by {pool.size / 2}");
                    break;
                }
            }
        }
    }
}