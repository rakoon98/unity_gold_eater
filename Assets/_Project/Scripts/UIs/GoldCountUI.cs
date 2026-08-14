
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GoldEater
{
    public class GoldCountUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        private GoldInventory inventory;

        public void Bind(GoldInventory newInventory)
        {
            if (inventory != null)
                inventory.OnGoldChanged -= UpdateGoldText;

            inventory = newInventory;

            if (inventory != null)
            {
                inventory.OnGoldChanged += UpdateGoldText;
                Refresh();
            }
        }

        public void Refresh()
        {
            if (inventory == null)
                return;

            UpdateGoldText(inventory.GoldCount);
        }

        private void UpdateGoldText(int amount)
        {
            goldText.text = $"GOLD : {amount}";
        }

        private void OnDestroy()
        {
            if (inventory != null)
                inventory.OnGoldChanged -= UpdateGoldText;
        }
    }
}