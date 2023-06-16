
namespace GemTrader.UI
{
    public class PopUpButtonPresenter : BackButtonPresenter
    {
        protected override void HandleCanvas()
        {
            responsibleCanvas.gameObject.SetActive(true);
        }
    }

}

