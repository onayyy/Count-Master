using UnityEngine.Events;
using Enums;
using Extentions;
using Keys;
using System;
using UnityEngine;

namespace Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<GameStates> OnChangeGameState = delegate { };
        public UnityAction<SaveGameDataParams> OnSaveGameData = delegate { };
        public UnityAction OnLevelInitialize = delegate { };
        public UnityAction OnClearActiveLevel = delegate { };
        public UnityAction OnLevelFailed = delegate { };
        public UnityAction OnLevelSuccessful = delegate { };
        public UnityAction OnNextLevel = delegate { };
        public UnityAction OnRestartLevel = delegate { };
        public UnityAction OnPrepareLevel = delegate { };
        public UnityAction OnPlay = delegate { };
        public UnityAction OnReset = delegate { };
        public UnityAction<int, Symbols> OnGateDetected = delegate { };
        public UnityAction<GameObject> OnObstacleDetected = delegate { };
        public UnityAction<GameObject> OnEnemyDetected = delegate { };
        public UnityAction<GameObject> OnPlayerDetected = delegate { };
        public UnityAction OnEnemyHolderDetected = delegate { };
        public UnityAction OnPlayerMovement = delegate { };
        public UnityAction OnFinishLineDetected = delegate { };
        public UnityAction<GameObject> OnFinishCubeDetected = delegate { };


        public UnityAction OnSetCameraTarget = delegate { };

        public Func<int> OnGetLevelID = delegate { return 0; };

        public void PrepareLevel()
        {
            OnPrepareLevel?.Invoke();
        }

    }

    
}
