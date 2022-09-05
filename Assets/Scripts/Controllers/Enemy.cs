using UnityEngine;
using Signals;

namespace Controllers
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private ParticleSystem bloodParticle;

        private EnemyController enemyController;

        private bool kill = false;

        private void Start()
        {
            enemyController = transform.parent.GetComponent<EnemyController>();
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, Time.fixedDeltaTime * 2f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && !kill)
            {
                bloodParticle.transform.SetParent(Garbage.Instance.GarbageCollector);
                bloodParticle.Play();
                gameObject.tag = "Untagged";
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                CoreGameSignals.Instance.OnEnemyDetected?.Invoke(collision.gameObject);
                enemyController.EnemyList.Remove(gameObject);
                gameObject.SetActive(false);
                enemyController.EnemyCountText.text = enemyController.EnemyList.Count.ToString();
                if (enemyController.EnemyList.Count <= 0)
                {
                    enemyController.gameObject.SetActive(false);
                    CoreGameSignals.Instance.OnPlayerMovement?.Invoke();
                }
                kill = true;
            }
        }
    }

}

