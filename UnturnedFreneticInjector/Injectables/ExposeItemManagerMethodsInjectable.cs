using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeItemManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "spawnItem" method in ItemManager for easier use.
            // Then, add all items spawned internally to the item manager's model list as physical entities.
            // This allows us to have better control over items and make them more interactive.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.ItemManager");
            MethodDefinition spawnItemMethod = GetMethod(type, "spawnItem", 7);
            spawnItemMethod.IsPrivate = false;
            spawnItemMethod.IsPublic = true;
            FieldDefinition managerField = GetField(type, "manager");
            managerField.IsPrivate = false;
            managerField.IsPublic = true;
            FieldDefinition fieldregions = GetField(type, "regions");
            fieldregions.IsPrivate = false;
            fieldregions.IsPublic = true;
            // Keep track of items by using models
            TypeDefinition itemTracker = moddef.GetType("UnturnedFrenetic.ItemModelTracker");
            // (Item, Vector3)
            MethodReference trackItemMethod = gamedef.ImportReference(GetMethod(itemTracker, "Track", 2));
            // (byte, byte, int)
            MethodReference untrackItemMethod = gamedef.ImportReference(GetMethod(itemTracker, "Untrack", 3));
            // (byte, byte, List<ItemData>)
            MethodReference resetItemsMethod = gamedef.ImportReference(GetMethod(itemTracker, "Reset", 3));
            // For getting 'point' property from ItemSpawnpoint objects
            MethodDefinition getPointProperty = GetMethod(gamedef.GetType("SDG.Unturned.ItemSpawnpoint"), "get_point", 0);
            // Track dropItem
            MethodDefinition dropItemMethod = GetMethod(type, "dropItem", 5);
            InjectInstructions(dropItemMethod.Body, 88, new Instruction[]
            {
                // Load: Item item
                Instruction.Create(OpCodes.Ldarg_0),
                // Load: Vector3 point
                Instruction.Create(OpCodes.Ldarg_1),
                // ItemModelTracker.Track(item, point);
                Instruction.Create(OpCodes.Call, trackItemMethod)
            });
            // Track generateItems
            MethodDefinition generateItemsMethod = GetMethod(type, "generateItems", 2);
            InjectInstructions(generateItemsMethod.Body, 123, new Instruction[]
            {
                // Load: byte x
                Instruction.Create(OpCodes.Ldarg_1),
                // Load: byte y
                Instruction.Create(OpCodes.Ldarg_2),
                // Load: List<ItemData> list
                Instruction.Create(OpCodes.Ldloc_0),
                // Call: ItemModelTracker.Reset(x, y, list);
                Instruction.Create(OpCodes.Call, resetItemsMethod)
            });
            // Track respawnItems
            MethodDefinition respawnItemsMethod = GetMethod(type, "respawnItems", 0);
            InjectInstructions(respawnItemsMethod.Body, 111, new Instruction[]
            {
                // Load: Item item2
                Instruction.Create(OpCodes.Ldloc_3),
                // Load: ItemSpawnpoint itemSpawnpoint
                Instruction.Create(OpCodes.Ldloc_0),
                // Call: 'get_point' on  itemSpawnpoint2 -> add the Vector3 result to the stack.
                Instruction.Create(OpCodes.Callvirt, getPointProperty),
                // Call: ItemModelTracker.Track(newItem, itemSpawnpoint2.point);
                Instruction.Create(OpCodes.Call, trackItemMethod)
            });
            // Untrack askTakeItem
            MethodDefinition askTakeItemMethod = GetMethod(type, "askTakeItem", 4);
            InjectInstructions(askTakeItemMethod.Body, 95, new Instruction[]
            {
                // Load: byte x
                Instruction.Create(OpCodes.Ldarg_2),
                // Load: byte y
                Instruction.Create(OpCodes.Ldarg_3),
                // Load: ushort num
                Instruction.Create(OpCodes.Ldloc_2),
                // Call: ItemModelTracker.Untrack(x, y, num);
                Instruction.Create(OpCodes.Call, untrackItemMethod)
            });
            // Untrack despawnItems
            MethodDefinition despawnItemsMethod = GetMethod(type, "despawnItems", 0);
            InjectInstructions(despawnItemsMethod.Body, 50, new Instruction[]
            {
                // Load: ItemManager.despawnItems_X
                Instruction.Create(OpCodes.Ldsfld, GetField(type, "despawnItems_X")),
                // Load: ItemManager.despawnItems_Y
                Instruction.Create(OpCodes.Ldsfld, GetField(type, "despawnItems_Y")),
                // Load: int i
                Instruction.Create(OpCodes.Ldloc_0),
                // Call: ItemModelTracker.Untrack(ItemManager.despawnItems_X, ItemManager.despawnItems_Y, i);
                Instruction.Create(OpCodes.Call, untrackItemMethod)
            });
        }
    }
}
