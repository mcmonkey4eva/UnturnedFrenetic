using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnturnedFrenetic
{
    public class ItemModelTracker
    {
        public static void Track(Item item, Vector3 point)
        {
            byte x;
            byte y;
            if (Regions.tryGetCoordinate(point, out x, out y))
            {
                ItemManager.manager.spawnItem(x, y, item.id, item.amount, item.quality, item.state, point);
            }
        }

        public static void Untrack(byte x, byte y, int index)
        {
            ItemRegion itemRegion = ItemManager.regions[x, y];
            GameObject.Destroy(itemRegion.models[index].gameObject);
            itemRegion.models.RemoveAt(index);
        }
    }
}
