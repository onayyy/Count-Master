using Enums;
using Extentions;
using UnityEngine.Events;

namespace Signals
{
    public class UISignals : MonoSingleton<UISignals>
    {
        public UnityAction<UIPanels> OnOpenPanel;
        public UnityAction<UIPanels> OnClosePanel;
    }
}