using Controllers;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private UIPanelController uiPanelController;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            UISignals.Instance.OnOpenPanel += OnOpenPanel;
            UISignals.Instance.OnClosePanel += OnClosePanel;
            CoreGameSignals.Instance.OnPlay += OnPlay;
            CoreGameSignals.Instance.OnLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.OnLevelSuccessful += OnLevelSuccessful;
        }

        private void UnsubscribeEvents()
        {
            UISignals.Instance.OnOpenPanel -= OnOpenPanel;
            UISignals.Instance.OnClosePanel -= OnClosePanel;
            CoreGameSignals.Instance.OnPlay -= OnPlay;
            CoreGameSignals.Instance.OnLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.OnLevelSuccessful -= OnLevelSuccessful;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }


        private void OnOpenPanel(UIPanels panelParam)
        {
            uiPanelController.OpenPanel(panelParam);
        }

        private void OnClosePanel(UIPanels panelParam)
        {
            uiPanelController.ClosePanel(panelParam);
        }


        private void OnPlay()
        {
            UISignals.Instance.OnClosePanel?.Invoke(UIPanels.StartPanel);
        }

        private void OnLevelFailed()
        {
            UISignals.Instance.OnClosePanel?.Invoke(UIPanels.LevelPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.FailPanel);
        }

        private void OnLevelSuccessful()
        {
            UISignals.Instance.OnClosePanel?.Invoke(UIPanels.LevelPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.WinPanel);
        }

        public void Play()
        {
            CoreGameSignals.Instance.OnPlay?.Invoke();
        }

        public void NextLevel()
        {
            CoreGameSignals.Instance.OnNextLevel?.Invoke();
            UISignals.Instance.OnClosePanel?.Invoke(UIPanels.WinPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.LevelPanel);
        }

        public void RestartLevel()
        {
            CoreGameSignals.Instance.OnRestartLevel?.Invoke();
            UISignals.Instance.OnClosePanel?.Invoke(UIPanels.FailPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.StartPanel);
            UISignals.Instance.OnOpenPanel?.Invoke(UIPanels.LevelPanel);
        }
    }
}