using Cinemachine;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [ShowInInspector] private Vector3 _initialPosition;
        [ShowInInspector] private Quaternion _initialRotation;

        private void Awake()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            GetInitialPositionAndRotation();
            
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel += PrepareLevel;
            CoreGameSignals.Instance.OnPlay += SetCameraTarget;
            CoreGameSignals.Instance.OnSetCameraTarget += OnSetCameraTarget;
            CoreGameSignals.Instance.OnReset += OnReset;
            CoreGameSignals.Instance.OnFinishLineDetected += FinishLineDetected;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel -= PrepareLevel;
            CoreGameSignals.Instance.OnPlay -= SetCameraTarget;
            CoreGameSignals.Instance.OnSetCameraTarget -= OnSetCameraTarget;
            CoreGameSignals.Instance.OnReset -= OnReset;
            CoreGameSignals.Instance.OnFinishLineDetected -= FinishLineDetected;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }


        private void GetInitialPositionAndRotation()
        {
            _initialPosition = transform.localPosition;
            _initialRotation = transform.localRotation;
        }

        private void OnMoveToInitialPosition()
        {
            transform.localPosition = _initialPosition;
            transform.localRotation = _initialRotation;
        }

        private void PrepareLevel()
        {
            OnMoveToInitialPosition();
            SetCameraTarget();

        }

        private void SetCameraTarget()
        {
            CoreGameSignals.Instance.OnSetCameraTarget?.Invoke();
        }

        private void OnSetCameraTarget()
        {
            var playerTransform = FindObjectOfType<PlayerManager>().transform;
            virtualCamera.Follow = playerTransform.GetChild(1);
        }

        private void OnReset()
        {
            virtualCamera.Follow = null;
            virtualCamera.LookAt = null;
            OnMoveToInitialPosition();
        }

        private void FinishLineDetected()
        {
            transform.DORotate(new Vector3(25f, -21.5f, 0f), 3f).SetDelay(0.75f);
        }

    }
}