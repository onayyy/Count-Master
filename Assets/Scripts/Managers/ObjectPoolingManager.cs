using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extentions;

namespace Managers
{
    public class ObjectPoolingManager : MonoSingleton<ObjectPoolingManager>
    {
        [System.Serializable]
        public class Pool
        {
            public string Tag;
            public GameObject Prefab;
            public int Size;
        }

        public List<Pool> Pools;
        public Dictionary<string, Queue<GameObject>> PoolDictionary;

        private void Start()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in Pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                PoolDictionary.Add(pool.Tag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform refTransform)
        {
            GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

            objectToSpawn.transform.SetParent(refTransform);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
         

            objectToSpawn.SetActive(true);

            PoolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }



    }
}


