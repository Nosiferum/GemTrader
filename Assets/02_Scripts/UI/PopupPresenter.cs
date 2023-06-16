using GemTrader.Control;
using TMPro;
using UnityEngine;

namespace GemTrader.UI
{
    public class PopupPresenter : MonoBehaviour
    {
        [Header("Collected Sums")] [SerializeField]
        private TextMeshProUGUI collectedSum1;

        [SerializeField] private TextMeshProUGUI collectedSum2;
        [SerializeField] private TextMeshProUGUI collectedSum3;

        private GemController _gemController;

        private void Awake()
        {
            _gemController = FindObjectOfType<GemController>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void HandlePopupText()
        {
            if (_gemController.GemCountDict.TryGetValue("Green Gem", out int value))
            {
                collectedSum1.text = $"Collected Sum: {value}";
            }

            if (_gemController.GemCountDict.TryGetValue("Pink Gem", out int value2))
            {
                collectedSum2.text = $"Collected Sum: {value2}";
            }

            if (_gemController.GemCountDict.TryGetValue("Yellow Gem", out int value3))
            {
                collectedSum3.text = $"Collected Sum: {value3}";
            }
        }

        private void OnEnable()
        {
            _gemController.onGemAdded += HandlePopupText;
        }

        private void OnDestroy()
        {
            _gemController.onGemAdded -= HandlePopupText;
        }
    }
}