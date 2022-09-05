using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using Keys;
using Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {

        [Header("Data")] public LevelData Data;

        [Space] [SerializeField] private GameObject levelHolder;
        [SerializeField] private LevelLoaderCommand levelLoader;
        [SerializeField] private ClearActiveLevelCommand levelClearer;
        [SerializeField] private TextMeshProUGUI levelIndexText;

        [ShowInInspector] private int _levelID;

        private void Awake()
        {
            _levelID = GetActiveLevel();
            Data = GetLevelData();
        }

        private int GetActiveLevel()
        {
            if (!ES3.FileExists()) return 0;
            return ES3.KeyExists("Level") ? ES3.Load<int>("Level") : 0;
        }

        private LevelData GetLevelData()
        {
            var newLevelData = _levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count;
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[newLevelData];
        }


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel += PrepareLevel;
            CoreGameSignals.Instance.OnLevelInitialize += OnInitializeLevel;
            CoreGameSignals.Instance.OnClearActiveLevel += OnClearActiveLevel;
            CoreGameSignals.Instance.OnNextLevel += OnNextLevel;
            CoreGameSignals.Instance.OnRestartLevel += OnRestartLevel;
            CoreGameSignals.Instance.OnGetLevelID += OnGetLevelID;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.OnPrepareLevel -= PrepareLevel;
            CoreGameSignals.Instance.OnLevelInitialize -= OnInitializeLevel;
            CoreGameSignals.Instance.OnClearActiveLevel -= OnClearActiveLevel;
            CoreGameSignals.Instance.OnNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.OnRestartLevel -= OnRestartLevel;
            CoreGameSignals.Instance.OnGetLevelID -= OnGetLevelID;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }


        private void Start()
        {
            OnInitializeLevel();
            CoreGameSignals.Instance.PrepareLevel();
        }

        private void PrepareLevel()
        {
            levelIndexText.text = "Level " + (_levelID + 1).ToString();
        }

        private void OnNextLevel()
        {
            _levelID++;
            levelIndexText.text = "Level " + (_levelID + 1).ToString();
            CoreGameSignals.Instance.OnClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.OnReset?.Invoke();
            CoreGameSignals.Instance.OnSaveGameData?.Invoke(new SaveGameDataParams()
            {
                Level = _levelID
            });
            CoreGameSignals.Instance.OnLevelInitialize?.Invoke();
            CoreGameSignals.Instance.PrepareLevel();
        }

        private void OnRestartLevel()
        {
            levelIndexText.text = "Level " + (_levelID + 1).ToString();
            CoreGameSignals.Instance.OnClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.OnReset?.Invoke();
            CoreGameSignals.Instance.OnSaveGameData?.Invoke(new SaveGameDataParams()
            {
                Level = _levelID
            });
            CoreGameSignals.Instance.OnLevelInitialize?.Invoke();
            CoreGameSignals.Instance.PrepareLevel();
        }

        private int OnGetLevelID()
        {
            return _levelID;
        }


        private void OnInitializeLevel()
        {
            var newLevelData = _levelID % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count;
            levelLoader.InitializeLevel(newLevelData, levelHolder.transform);
        }

        private void OnClearActiveLevel()
        {
            levelClearer.ClearActiveLevel(levelHolder.transform);
        }
    }
}