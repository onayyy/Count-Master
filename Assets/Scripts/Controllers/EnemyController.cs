using UnityEngine;
using Data.ValueObject;
using Data.UnityObject;
using Signals;
using Managers;
using System.Collections.Generic;
using TMPro;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Data")] public EnemyData EnemyData;

        [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

        [SerializeField] private TextMeshProUGUI enemyCountText;

        [SerializeField] private int enemyHolderID;
        [SerializeField] private bool enemyCanMove = false;

        private Transform targetTransform;

        public List<GameObject> EnemyList
        {
            get
            {
                return enemyList;
            }
        }

        public TextMeshProUGUI EnemyCountText
        {
            get
            {
                return enemyCountText;
            }
        }

        private void Awake()
        {
            EnemyData = GetEnemyData();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel += PrepareLevel;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel -= PrepareLevel;
        }

        private void FixedUpdate()
        {
            if (enemyCanMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.fixedDeltaTime * 5f);
            }
        }


        private void PrepareLevel()
        {
            SpawnEnemy();
        }

        private EnemyData GetEnemyData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[CoreGameSignals.Instance.OnGetLevelID() % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count].EnemyList[enemyHolderID];

        private void SpawnEnemy()
        {
            for (int i = 0; i < EnemyData.EnemyCount; i++)
            {
                GameObject prefabEnemy = ObjectPoolingManager.Instance.SpawnFromPool("Enemy", EnemyPosition(), Quaternion.identity, transform);
                enemyList.Add(prefabEnemy);
            }

            enemyCountText.text = enemyList.Count.ToString();
        }

        private Vector3 EnemyPosition()
        {
            Vector3 enemyPosition = Random.insideUnitSphere;
            Vector3 newEnemyPosition = transform.position + enemyPosition;
            newEnemyPosition.y = 0f;

            return newEnemyPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                for (int i = 0; i < enemyList.Count; i++)
                {
                    enemyList[i].GetComponent<Animator>().SetBool("Run", true);
                }
                gameObject.GetComponent<SphereCollider>().enabled = false;
                enemyCanMove = true;
                targetTransform = other.transform.parent;
                CoreGameSignals.Instance.OnEnemyHolderDetected?.Invoke();
            }
        }

        

    }

}

