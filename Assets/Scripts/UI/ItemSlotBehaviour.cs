using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnBasedRPG
{
    public class ItemSlotBehaviour : BaseSlot
    {
        public TMPro.TextMeshProUGUI nameText;
        public TMPro.TextMeshProUGUI quantityText;
        public string ItemType { get; private set; }
        public int ItemQuantity { get; private set; }
        public void Set(Item item)
        {
            ItemType = item.itemType;
            ItemQuantity = item.itemQuantity;
            nameText.text = ItemType;
            quantityText.text = ItemQuantity.ToString();
        }


    }
}
