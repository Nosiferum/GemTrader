using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DG.Tweening;
using GemTrader.Environment;
using GemTrader.Managers;
using UnityEngine;

namespace GemTrader.Control
{
    [Serializable]
    [RequireComponent(typeof(PlayerController))]
    public class GemController : MonoBehaviour
    {
        [SerializeField] private Transform stackTransform;
        [SerializeField] private float gemStackLerpTime = 5f;

        public Dictionary<string, int> GemCountDict { get; private set; } = new();
        public Action onGemAdded;

        private readonly List<BaseGem> _gems = new();
        private static readonly string GemSaveLocation = "/GemCount.dat";

        private void Awake()
        {
            LoadGems();
        }

        private void Start()
        {
            onGemAdded?.Invoke();
        }

        private void Update()
        {
            StackGems();
        }

        public void SellGems(Transform salePoint)
        {
            if (_gems.Count > 0)
            {
                //this somewhat new C# feature gets the last element from a container 
                BaseGem gem = _gems[^1];

                gem.transform.DOMove(salePoint.transform.position, 0.1f).SetLink(gem.gameObject).OnComplete(delegate
                {
                    GoldManager.Instance.AddGold(gem.GetSellPrice());

                    _gems.Remove(gem);
                    ManageGemDict(gem);
                    onGemAdded?.Invoke();
                    Destroy(gem.gameObject);
                });
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponentInParent<GridSystem>())
            {
                BaseGem gem = other.GetComponent<BaseGem>();
                AddGems(gem);
            }
        }

        private void AddGems(BaseGem gem)
        {
            if (gem.IsReadyToHarvest)
            {
                gem.GetComponentInParent<GridSystem>()
                    .RespawnGem(gem, gem.CellCoordinateX, gem.CellCoordinateY);

                Transform gemTransform = gem.transform;

                gemTransform.DOKill();

                //scaled down the gems to stack them easily on the back
                gemTransform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                _gems.Add(gem);
            }
        }

        private void ManageGemDict(BaseGem gem)
        {
            string gemName = gem.Name;

            if (GemCountDict.ContainsKey(gemName))
            {
                GemCountDict[gemName]++;
            }
            else
            {
                GemCountDict.Add(gemName, 1);
            }
        }

        private void StackGems()
        {
            for (int i = 0; i < _gems.Count; i++)
            {
                Vector3 gemPos = _gems[i].transform.position;
                Vector3 stackPos = stackTransform.position;

                if (_gems.Count > 1 && i > 0)
                {
                    stackPos = _gems[i - 1].transform.position + new Vector3(0, 0.25f, 0);
                }

                gemPos = new Vector3(Mathf.Lerp(gemPos.x, stackPos.x, Time.deltaTime * gemStackLerpTime),
                    Mathf.Lerp(gemPos.y, stackPos.y, Time.deltaTime * gemStackLerpTime),
                    Mathf.Lerp(gemPos.z, stackPos.z, Time.deltaTime * gemStackLerpTime));

                _gems[i].transform.position = gemPos;
            }
        }

        private void SaveGems()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string savePath = Application.persistentDataPath + GemSaveLocation;
            FileStream fileStream = File.Create(savePath);
            formatter.Serialize(fileStream, GemCountDict);
            fileStream.Close();
        }

        private void LoadGems()
        {
            string savePath = Application.persistentDataPath + GemSaveLocation;
            
            if (!File.Exists(savePath)) return;
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(savePath, FileMode.Open);
            GemCountDict = (Dictionary<string, int>)formatter.Deserialize(fileStream);
            fileStream.Close();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SaveGems();
        }

        private void OnApplicationQuit()
        {
            SaveGems();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
           SaveGems();
        }
    }
}