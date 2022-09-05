using UnityEngine;
using Enums;
using Signals;
using DG.Tweening;

namespace Controllers
{
    public class ObstacleController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CoreGameSignals.Instance.OnObstacleDetected?.Invoke(other.gameObject);
            }
        }


    }
}


