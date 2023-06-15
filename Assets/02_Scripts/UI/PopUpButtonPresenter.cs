using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GemTrader.UI
{
    public class PopUpButtonPresenter : BackButtonPresenter
    {
        protected override void HandleCanvas()
        {
            parentCanvas.gameObject.SetActive(true);
        }
    }

}

