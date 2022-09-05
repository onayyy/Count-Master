using UnityEngine;
using DG.Tweening;
using Managers;
using Signals;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem bloodParticle;

        private PlayerManager playerManager;

        private Rigidbody playerRigidBody;

        public ParticleSystem BloodParticle
        {
            get
            {
                return bloodParticle;
            }
        }

        private void Awake()
        {
            playerRigidBody = transform.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            playerManager = transform.parent.parent.GetComponent<PlayerManager>();
        }

        private void FixedUpdate()
        {
            if (!playerManager.IsFinishTrigger)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, Time.fixedDeltaTime * 2f);
            }
  
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Jump"))
            {
                playerRigidBody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                playerRigidBody.freezeRotation = true;
            }

            if (collision.gameObject.CompareTag("Floor"))
            {
                playerRigidBody.constraints = RigidbodyConstraints.FreezePositionY;
                playerRigidBody.freezeRotation = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Jump"))
            {
                transform.DOLocalMoveY(0f, 1.5f).SetDelay(0.25f);
            }
        }

    }
}


