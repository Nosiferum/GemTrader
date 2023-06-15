using System;
using GemTrader.Control;
using UnityEngine;

namespace GemTrader.Environment
{
    public class SalePointController : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                playerController.SellGems(transform);
            }
        }
    }
}
