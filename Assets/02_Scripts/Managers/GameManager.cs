using System;
using UnityEngine;

namespace GemTrader.Managers
{
    public class GameManager : MonoBehaviour
    {
        public int TotalGold { get; private set; }

        public static GameManager Instance = null;
        public Action onMoneyTaken;

        private const string COIN_KEY = "coin";

        private void Awake()
        {
            InitManager();
            TotalGold = LoadGold();
        }

        public void AddGold(int amount)
        {
            TotalGold += amount;
            onMoneyTaken?.Invoke();
        }

        private void InitManager()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private int LoadGold()
        {
            return PlayerPrefs.GetInt(COIN_KEY);
        }

        private void SaveGold()
        {
            PlayerPrefs.SetInt(COIN_KEY, TotalGold);
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            SaveGold();
        }

        private void OnApplicationQuit()
        {
            SaveGold();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveGold();
        }
    }
}
