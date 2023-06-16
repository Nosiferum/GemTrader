using GemTrader.Managers;
using TMPro;
using UnityEngine;

namespace GemTrader.UI
{
    public class GoldPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        private GoldManager _goldManager;

        private void Awake()
        {
            _goldManager = FindObjectOfType<GoldManager>();
        }

        private void Start()
        {
            coinText.text = _goldManager.TotalGold.ToString();
        }

        private void UpdateCoinText()
        {
            coinText.text = _goldManager.TotalGold.ToString();
        }

        private void OnEnable()
        {
            _goldManager.onMoneyTaken += UpdateCoinText;
        }

        private void OnDisable()
        {
            _goldManager.onMoneyTaken -= UpdateCoinText;
        }
    }
}
