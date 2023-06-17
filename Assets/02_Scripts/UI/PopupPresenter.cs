using GemTrader.Control;
using GemTrader.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GemTrader.UI
{
    public class PopupPresenter : MonoBehaviour
    {
        [SerializeField] private ContainerItem containerItemTemplate;

        private GridLayoutGroup _container;
        private GemController _gemController;
        private GemTypeDataHolder _gemTypeDataHolder;
        private GameObject[] _createdContainerItems;

        private void Awake()
        {
            _gemController = FindObjectOfType<GemController>();
            _container = GetComponentInChildren<GridLayoutGroup>();
            _gemTypeDataHolder = Resources.Load<GemTypeDataHolder>("GemTypeDataHolder");
        }

        private void Start()
        {
            _createdContainerItems = InstantiateContainerItems();
            gameObject.SetActive(false);
        }

        private GameObject[] InstantiateContainerItems()
        {
            GameObject[] containerItems = new GameObject[_gemTypeDataHolder.gems.Length];

            for (int i = 0; i < _gemTypeDataHolder.gems.Length; i++)
            {
                GameObject containerItem = Instantiate(containerItemTemplate.gameObject, _container.transform);
                containerItems[i] = containerItem;
                
                containerItem.transform.GetChild(0).GetComponent<Image>().sprite = _gemTypeDataHolder.gems[i].Icon;
                containerItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>()
                    .text = _gemTypeDataHolder.gems[i].GemName;
            }

            return containerItems;
        }

        private void HandlePopupText()
        {
            for (int i = 0; i < _createdContainerItems.Length; i++)
            {
                if (_gemController.GemCountDict.TryGetValue(_gemTypeDataHolder.gems[i].GemName, out int value))
                {
                    _createdContainerItems[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>()
                        .text = $"Collected Sum: {value}";
                }
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