using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public class BattleUseItemPanelBehaviour : MonoBehaviour, ISlotHandler
    {
        public GameObject itemSlotPrefab;
        public Transform content;
        private List<ItemSlotBehaviour> itemSlotBehaviours = new List<ItemSlotBehaviour>();


        private void Awake()
        {
            List<Item> usableItems = Inventory.GetUsableItems(Inventory.ItemUsageContext.battle);
            for(int i = 0; i < usableItems.Count; i++)
            {
                ItemSlotBehaviour itemSlot = Instantiate(itemSlotPrefab, content).GetComponent<ItemSlotBehaviour>();
                itemSlot.Set(usableItems[i]);
                itemSlot.SetHandler(this);
                itemSlotBehaviours.Add(itemSlot);
            }
        }

        private void OnPropertyChanged()
        {
            List<Item> usableItems = Inventory.GetUsableItems(Inventory.ItemUsageContext.battle);
            for(int i = 0; i < usableItems.Count; i++)
            {
                ItemSlotBehaviour itemSlot = null;
                for (int j = 0; j < itemSlotBehaviours.Count; j++)
                {
                    if(usableItems[i].itemType == itemSlotBehaviours[i].ItemType)
                    {
                        itemSlot = itemSlotBehaviours[j];
                        break;
                    }
                }
                if (itemSlot)
                {
                    itemSlot.Set(usableItems[i]);
                }
                else
                {
                    ItemSlotBehaviour newSlot = Instantiate(itemSlotPrefab, content).GetComponent<ItemSlotBehaviour>();
                    newSlot.Set(usableItems[i]);
                    newSlot.SetHandler(this);
                    itemSlotBehaviours.Add(newSlot);
                }
            }
        }

        public void SlotClicked(BaseSlot slot)
        {
            ItemSlotBehaviour itemSlot = (slot as ItemSlotBehaviour);
            if (itemSlot)
            {

            }
        }
    }
}
