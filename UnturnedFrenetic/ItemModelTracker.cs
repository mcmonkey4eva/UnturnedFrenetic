using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace UnturnedFrenetic
{
    public class ItemModelTracker
    {
        private static Dictionary<int, int>[,] IndexConversion = new Dictionary<int, int>[ItemManager.regions.GetLength(0), ItemManager.regions.GetLength(1)];

        public static void Track(ItemData itemData, Vector3 point)
        {
            if (!Dedicator.isDedicated)
            {
                return;
            }
            byte x;
            byte y;
            if (Regions.tryGetCoordinate(point, out x, out y))
            {
                Item item = itemData.item;
                ItemManager.manager.spawnItem(x, y, item.id, item.amount, item.quality, item.state, point, itemData.instanceID);
                ItemRegion region = ItemManager.regions[x, y];
                if (IndexConversion[x, y] == null)
                {
                    IndexConversion[x, y] = new Dictionary<int, int>();
                }
                IndexConversion[x, y][region.items.Count - 1] = region.drops.Count - 1;
            }
        }

        public static void Untrack(byte x, byte y, int index)
        {
            if (!Dedicator.isDedicated)
            {
                return;
            }
            ItemRegion itemRegion = ItemManager.regions[x, y];
            index = IndexConversion[x, y][index];
            IndexConversion[x, y].Remove(index);
            GameObject.Destroy(itemRegion.drops[index].model.gameObject);
            itemRegion.drops.RemoveAt(index);
        }

        public static void Reset(byte x, byte y, List<ItemData> data)
        {
            ItemRegion itemRegion = ItemManager.regions[x, y];
            for (int i = itemRegion.drops.Count - 1; i >= 0; i--)
            {
                Untrack(x, y, i);
            }
            foreach (ItemData item in data)
            {
                Track(item, item.point);
            }
        }
    }
}
