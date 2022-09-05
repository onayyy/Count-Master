using UnityEngine;
using Signals;
using Extentions;

namespace Controllers
{
    public class FinishLine : MonoSingleton<FinishLine>
    {
        public Transform FinishLineTransform;

        private BoxCollider boxCollider;

        private void Awake()
        {
            FinishLineTransform = transform;
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                boxCollider.enabled = false;
                CoreGameSignals.Instance.OnFinishLineDetected?.Invoke();
            }
        }
    }

}

