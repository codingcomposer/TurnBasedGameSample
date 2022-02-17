using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG {
    public class Inventory
    {
        public enum ItemUsageContext { none, battle};
        public static List<Item> items = new List<Item>();
        static Inventory()
        {
            AddDisposableItemsForTest();
        }

        public static List<Item> GetUsableItems(ItemUsageContext context)
        {
            switch (context)
            {
                case ItemUsageContext.battle:
                    return GetBattleItems();
                default:
                    return null;
            }
        }

        public void UseUp(string itemType)
        {
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].itemType == itemType)
                {
                    items[i].itemQuantity--;
                    if(items[i].itemQuantity < 0)
                    {
                        items[i].itemQuantity = 0;
                    }
                    break;
                }
            }
        }

        private static List<Item> GetBattleItems()
        {
            List<Item> battleItems = new List<Item>();
            for(int i = 0; i < items.Count; i++)
            {
                if(items[i].itemQuantity > 0)
                {
                    string[] itemTypeChunks = items[i].itemType.Split(' ');
                    if (itemTypeChunks.Length > 1 && itemTypeChunks[1].Equals("BATTLE"))
                    {
                        battleItems.Add(items[i]);
                    }
                }
            }
            return battleItems;
        }

        private static void AddDisposableItemsForTest()
        {
            DisposableItem[] disposableItemAssets = LoadedData.itemBundle.LoadAllAssets<DisposableItem>();
            for (int i = 0; i < disposableItemAssets.Length; i++)
            {
                Item item = new Item() { itemType = disposableItemAssets[i].itemType, itemQuantity = 3 };
                items.Add(item);
            }
        }
    }
}
