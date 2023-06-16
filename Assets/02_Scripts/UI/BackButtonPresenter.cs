using UnityEngine;
using UnityEngine.UI;

namespace GemTrader.UI
{
    public class BackButtonPresenter : MonoBehaviour
    {
        [SerializeField] protected Canvas responsibleCanvas;

        private Button _button;

        protected void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected virtual void HandleCanvas()
        {
            responsibleCanvas.gameObject.SetActive(false);
        }

        protected void OnEnable()
        {
            _button.onClick.AddListener(HandleCanvas);
        }

        protected void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleCanvas);
        }
    }
}