using GemTrader.Managers;
using TMPro;
using UnityEngine;

namespace GemTrader.UI
{
    public class GoldPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            coinText.text = _gameManager.TotalGold.ToString();
        }

        private void UpdateCoinText()
        {
            coinText.text = _gameManager.TotalGold.ToString();
        }

        private void OnEnable()
        {
            _gameManager.onMoneyTaken += UpdateCoinText;
        }

        private void OnDisable()
        {
            _gameManager.onMoneyTaken -= UpdateCoinText;
        }
    }
}
