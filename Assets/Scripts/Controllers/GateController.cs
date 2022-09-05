using Data.UnityObject;
using Data.ValueObject;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class GateController : MonoBehaviour
    {
        [Header("Data")] public GateData GateData;

        [SerializeField] private int gateID;
        [SerializeField] private TextMeshProUGUI gateText;

        [SerializeField] private GateBase gateBase;

        private void Awake()
        {
            GateData = GetGateData();
            SetGateText();
        }


        private GateData GetGateData() => Resources.Load<CD_Level>("Data/CD_Level").Levels[CoreGameSignals.Instance.OnGetLevelID() % Resources.Load<CD_Level>("Data/CD_Level").Levels.Count].GateList[gateID];

        private void SetGateText()
        {
            gateText.text = GateData.GateValueText + GateData.GateValue;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(false);
                gateBase.LeftGate.GetComponent<BoxCollider>().enabled = false;
                gateBase.RightGate.GetComponent<BoxCollider>().enabled = false;
                CoreGameSignals.Instance.OnGateDetected?.Invoke(GateData.GateValue, GateData.SymbolType);
            }
        }


    }


}

