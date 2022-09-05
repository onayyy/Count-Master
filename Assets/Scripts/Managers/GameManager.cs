using UnityEngine;
using Enums;
using Keys;
using Signals;

public class GameManager : MonoBehaviour
{
    public GameStates States;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }


    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.OnChangeGameState += OnChangeGameState;
        CoreGameSignals.Instance.OnSaveGameData += OnSaveGame;
    }
    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.OnChangeGameState -= OnChangeGameState;
        CoreGameSignals.Instance.OnSaveGameData -= OnSaveGame;
    }

    private void OnChangeGameState(GameStates newState)
    {
        States = newState;
    }

    private void OnSaveGame(SaveGameDataParams saveDataParams)
    {
        if (saveDataParams.Level != null)
        {
            ES3.Save("Level", saveDataParams.Level);
        }

        if (saveDataParams.Coin != null) ES3.Save("Coin", saveDataParams.Coin);
        if (saveDataParams.SFX != null) ES3.Save("SFX", saveDataParams.SFX);
        if (saveDataParams.VFX != null) ES3.Save("VFX", saveDataParams.VFX);
        if (saveDataParams.Haptic != null) ES3.Save("Haptic", saveDataParams.Haptic);

    }

}
