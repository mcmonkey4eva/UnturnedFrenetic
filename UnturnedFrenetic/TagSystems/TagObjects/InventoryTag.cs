using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class InventoryTag : TemplateObject
    {
        // <--[object]
        // @Type InventoryTag
        // @SubType TextTag
        // @Group Inventories
        // @Description Represents an inventory.
        // -->

        public Items[] Internal;

        public InventoryTag(params Items[] items)
        {
            Internal = items;
        }

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            switch (data[0])
            {
                case "items":
                    {
                        List<TemplateObject> list = new List<TemplateObject>();
                        foreach (Items items in Internal) {
                            for (byte i = 0; i < items.getItemCount(); i++)
                            {
                                ItemJar item = items.getItem(i);
                                list.Add(new ItemTag(item));
                            }
                        }
                        return new ListTag(list).Handle(data);
                    }
                case "item":
                    {
                        long index = IntegerTag.For(data, data.GetModifierObject(0)).Internal;
                        int curr = 0;
                        byte size;
                        while (index >= (size = Internal[curr].getItemCount()))
                        {
                            curr++;
                            if (curr == Internal.Length)
                            {
                                curr--;
                                index = Internal[curr].getItemCount() - 1;
                                break;
                            }
                            else
                            {
                                index -= size;
                            }
                        }
                        return new ItemTag(Internal[curr].getItem((byte)index));
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return "TODO"; // TODO
        }
    }
}
