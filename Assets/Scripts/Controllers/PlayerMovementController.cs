using System;
using Data.ValueObject;
using Keys;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {

        [SerializeField] private PlayerManager manager;

        [Header("Data")] [ShowInInspector] private PlayerMovementData _movementData;
        [ShowInInspector] private bool _isReadyToPlay;


        public void SetMovementData(PlayerMovementData dataMovementData)
        {
            _movementData = dataMovementData;
        }


        public void IsReadyToPlay(bool state)
        {
            _isReadyToPlay = state;
        }

        private void Update()
        {
            if (_isReadyToPlay)
            {
                Move();
            }
            else
                Stop();
        }

        private void Move()
        {
            transform.Translate(0f, 0f, _movementData.ForwardSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, 0f, 500f), _movementData.ForwardSpeed * Time.deltaTime);

        }


        private void Stop()
        {
            transform.Translate(0f, 0f, 0f);
        }

        public void OnReset()
        {
            Stop();
            _isReadyToPlay = false;
        }
    }
}