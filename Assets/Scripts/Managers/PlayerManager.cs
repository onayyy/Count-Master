using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Signals;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using TMPro;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Data")] public PlayerData Data;

        [Space] [SerializeField] private PlayerMovementController movementController;

        [SerializeField] private GameObject playerCharacterMesh;
        [SerializeField] private GameObject playerCharacter;
        [SerializeField] private GameObject playerPieceParent;
        [SerializeField] private GameObject CM_CameraLook;
        [SerializeField] private GameObject horde;
        
        [SerializeField] private List<GameObject> playerList;
        [SerializeField] private List<GameObject> tempPlayerList = new List<GameObject>();
        [SerializeField] private List<GameObject> playerPieceParentList = new List<GameObject>();

        [SerializeField] private TextMeshProUGUI playerCountText;

        private int playerCount;

        private float rowOffset = 0.5f;
        private float offsetY = 0f;
        private float offsetX = -1f;

        private bool isFinishTrigger = false;
        public bool IsFinishTrigger
        {
            get
            {
                return isFinishTrigger;
            }
        }

        private void Awake()
        {
            Data = GetPlayerData();
            SendPlayerDataToControllers();
            playerCount = playerList.Count;
        }

        private void Update()
        {
            if (PathController.Instance.levelImageFill.fillAmount <= 1)
            {
                PathController.Instance.levelImageFill.fillAmount = transform.position.z / FinishLine.Instance.FinishLineTransform.position.z;
            }
            
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>("Data/CD_Player").Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(Data.MovementData);
        }


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel += PrepareLevel;
            CoreGameSignals.Instance.OnPlay += OnPlay;
            CoreGameSignals.Instance.OnReset += OnReset;
            CoreGameSignals.Instance.OnLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.OnLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.OnGateDetected += OnGateDetected;
            CoreGameSignals.Instance.OnObstacleDetected += OnObstacleDetected;
            CoreGameSignals.Instance.OnEnemyDetected += OnEnemyDetected;
            CoreGameSignals.Instance.OnEnemyHolderDetected += OnEnemyHolderDetected;
            CoreGameSignals.Instance.OnPlayerMovement += OnPlayerMovement;
            CoreGameSignals.Instance.OnFinishLineDetected += OnFinishDetected;
            CoreGameSignals.Instance.OnFinishCubeDetected += OnFinishCubeDetected;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel -= PrepareLevel;
            CoreGameSignals.Instance.OnPlay -= OnPlay;
            CoreGameSignals.Instance.OnReset -= OnReset;
            CoreGameSignals.Instance.OnLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.OnLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.OnGateDetected -= OnGateDetected;
            CoreGameSignals.Instance.OnObstacleDetected -= OnObstacleDetected;
            CoreGameSignals.Instance.OnEnemyDetected -= OnEnemyDetected;
            CoreGameSignals.Instance.OnEnemyHolderDetected -= OnEnemyHolderDetected;
            CoreGameSignals.Instance.OnPlayerMovement -= OnPlayerMovement;
            CoreGameSignals.Instance.OnFinishLineDetected -= OnFinishDetected;
            CoreGameSignals.Instance.OnFinishCubeDetected -= OnFinishCubeDetected;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void PrepareLevel()
        {
            playerCountText.text = playerList.Count.ToString();
        }
        
        private void OnPlay()
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<Animator>().SetBool("Run", true);
            }
            movementController.IsReadyToPlay(true);
        }

        private void OnLevelSuccessful()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnLevelFailed()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnStageSuccessful()
        {
            movementController.IsReadyToPlay(true);
        }

        private void OnStageAreaReached()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnReset()
        {
            movementController.OnReset();
        }

        private void OnGateDetected(int gateValue, Symbols symbol)
        {
            switch (symbol)
            {
                case Symbols.Multiply:

                    var tempPlayerCount = playerCount;
                    playerCount *= gateValue;
                    for (int i = 0; i < playerCount - tempPlayerCount; i++)
                    {
                        GameObject prefabPlayer = ObjectPoolingManager.Instance.SpawnFromPool("Player", playerCharacterMesh.transform.position, Quaternion.identity, playerCharacterMesh.transform);
                        prefabPlayer.transform.position = PlayerPosition(playerCharacterMesh.transform);
                        prefabPlayer.GetComponent<Animator>().SetBool("Run", true);
                        playerList.Add(prefabPlayer);
                        playerCountText.text = playerList.Count.ToString();
                    }
                    break;
                case Symbols.Add:

                    playerCount += gateValue;
                    for (int i = 0; i < gateValue; i++)
                    {
                        GameObject prefabPlayer = ObjectPoolingManager.Instance.SpawnFromPool("Player", playerCharacterMesh.transform.position, Quaternion.identity, playerCharacterMesh.transform);
                        prefabPlayer.transform.position = PlayerPosition(playerCharacterMesh.transform);
                        prefabPlayer.GetComponent<Animator>().SetBool("Run", true);
                        playerList.Add(prefabPlayer);
                        playerCountText.text = playerList.Count.ToString();
                    }
                    break;
                default:
                    break;
            }
        }


        private Vector3 PlayerPosition(Transform characterTransform)
        {
            Vector3 pos = Random.insideUnitSphere * 0.1f;
            Vector3 newPos = characterTransform.position + pos;
            newPos.y = 0f;

            return newPos;
        }


        private void OnObstacleDetected(GameObject obj)
        {
            obj.GetComponent<PlayerPhysicsController>().BloodParticle.transform.SetParent(Garbage.Instance.GarbageCollector);
            obj.GetComponent<PlayerPhysicsController>().BloodParticle.Play();
            playerList.Remove(obj);
            playerCount = playerList.Count;
            obj.SetActive(false);
            playerCountText.text = playerList.Count.ToString();
            if (playerList.Count <= 0)
            {
                horde.SetActive(false);
                movementController.IsReadyToPlay(false);
                CoreGameSignals.Instance.OnLevelFailed?.Invoke();
                Debug.Log("GameOver");
            }
        }

        private void OnEnemyDetected(GameObject obj)
        {
            obj.GetComponent<PlayerPhysicsController>().BloodParticle.transform.SetParent(Garbage.Instance.GarbageCollector);
            obj.GetComponent<PlayerPhysicsController>().BloodParticle.Play();
            obj.tag = "Untagged";
            obj.transform.GetComponent<CapsuleCollider>().enabled = false;
            playerList.Remove(obj);
            playerCount = playerList.Count;
            obj.SetActive(false);
            playerCountText.text = playerList.Count.ToString();
            if (playerList.Count <= 0)
            {
                horde.SetActive(false);
                movementController.IsReadyToPlay(false);
                CoreGameSignals.Instance.OnLevelFailed?.Invoke();
                Debug.Log("GameOver");
            }
        }

        private void OnEnemyHolderDetected()
        {
            movementController.IsReadyToPlay(false);
        }

        private void OnPlayerMovement()
        {
            movementController.IsReadyToPlay(true);
        }

        private void OnFinishCubeDetected(GameObject obj)
        {
            obj.transform.SetParent(Garbage.Instance.GarbageCollector);
            obj.GetComponent<Animator>().SetBool("Run", false);
            var index = tempPlayerList.IndexOf(obj);
            tempPlayerList.RemoveAt(index);

            if (tempPlayerList.Count == 0)
            {
                movementController.IsReadyToPlay(false);
                StartCoroutine(LevelCompleted());
                Debug.Log("Successfully");
            }
        }

        private IEnumerator LevelCompleted()
        {
            yield return new WaitForSeconds(2f);
            CoreGameSignals.Instance.OnLevelSuccessful?.Invoke();
        }

        private void OnFinishDetected()
        {
            horde.SetActive(false);
            isFinishTrigger = true;

            for (int i = 0; i < playerList.Count; i++)
            {
                playerList[i].GetComponent<Rigidbody>().isKinematic = true;
            }

            StartCoroutine(TriangleFormation());
        }

        private IEnumerator TriangleFormation()
        {
            Vector3 targetPosition = Vector3.left;

            yield return new WaitForSeconds(0.75f);

            for (int i = 1; i <= playerList.Count; i++)
            {
                GameObject obj = Instantiate(playerPieceParent, playerCharacterMesh.transform);

                for (int j = 0; j < i; j++)
                {
                    if (j == 0 && i == 1)
                    {
                        CM_CameraLook.transform.DOLocalMove(new Vector3(15f, 30f, -30f), 3f);
                    }

                    if (playerList.Count <= j)
                    {
                        targetPosition = new Vector3(targetPosition.x + offsetX, 0f, transform.position.z);
                        playerList[0].transform.position = targetPosition;
                        playerList[0].transform.SetParent(obj.transform);
                        tempPlayerList.Add(playerList[0]);
                        playerList.RemoveAt(0);
                    }
                    else if (playerList.Count > j)
                    {
                        targetPosition = new Vector3(targetPosition.x + offsetX, 0f, transform.position.z);
                        playerList[j].transform.position = targetPosition;
                        playerList[j].transform.SetParent(obj.transform);
                        tempPlayerList.Add(playerList[j]);
                        playerList.RemoveAt(j);
                    }
                }

                targetPosition = new Vector3((rowOffset * i) - 1f, 0f, transform.position.z);

                playerPieceParentList.Add(obj);

            }

            for (int z = playerPieceParentList.Count - 1; z >= 0; z--)
            {
 
                if (z == playerPieceParentList.Count - 1)
                {
                    playerPieceParentList[z].transform.localPosition = new Vector3(2f, offsetY, 0f);
                }

                if (z < playerPieceParentList.Count - 1)
                {
                    playerPieceParentList[z].transform.localPosition = new Vector3(2f, offsetY, 0f);
                    playerPieceParentList[z + 1].transform.localPosition = new Vector3(2f, offsetY - 1.5f, 0f);
                }
                offsetY += 1.5f;

                if (z == 0)
                {
                    for (int r = 0; r < playerList.Count; r++)
                    {
                        playerList[r].gameObject.SetActive(false);
                    }
                    playerList.Clear();
                }

                yield return new WaitForSeconds(0.05f);

            }

        }

    }
}