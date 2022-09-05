using UnityEngine;
using Signals;
using DG.Tweening;
using Lean.Common;
using Lean.Touch;

namespace Controllers
{
    public class LeanTouchController : MonoBehaviour
    {
        [SerializeField] private LeanManualTranslate leanManual;
        [SerializeField] private LeanMultiUpdate leanMulti;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel += OnPrepareLevel;
            CoreGameSignals.Instance.OnEnemyHolderDetected += OnEnemyHolderDetected;
            CoreGameSignals.Instance.OnPlayerMovement += OnPlayerMovement;
            CoreGameSignals.Instance.OnFinishLineDetected += OnFinishLineDetected;
        }


        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel -= OnPrepareLevel;
            CoreGameSignals.Instance.OnEnemyHolderDetected -= OnEnemyHolderDetected;
            CoreGameSignals.Instance.OnPlayerMovement -= OnPlayerMovement;
            CoreGameSignals.Instance.OnFinishLineDetected -= OnFinishLineDetected;
        }

        private void OnPrepareLevel()
        {
            leanManual.enabled = true;
            leanMulti.enabled = true;
        }

        private void OnEnemyHolderDetected()
        {
            leanManual.enabled = false;
            leanMulti.enabled = false;
        }


        private void OnPlayerMovement()
        {
            leanManual.enabled = true;
            leanMulti.enabled = true;
        }

        private void OnFinishLineDetected()
        {
            transform.DOLocalMoveX(0f, 1.5f);
            leanManual.enabled = false;
            leanMulti.enabled = false;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

    }

}

