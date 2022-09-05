using UnityEngine;
using Signals;
using TMPro;

namespace Controllers
{
    public class FinishCubeController : MonoBehaviour
    {
        public TextMeshProUGUI TextMeshFinishCube;
        public Renderer MaterialFinishCube;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CoreGameSignals.Instance.OnFinishCubeDetected?.Invoke(collision.gameObject);
            }
        }
    }
}


