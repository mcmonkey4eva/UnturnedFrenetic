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
            Instruction callTrackItem = Instruction.Create(OpCodes.Call, trackItemMethod);
            // (byte, byte, int)
            MethodReference untrackItemMethod = gamedef.ImportReference(GetMethod(itemTracker, "Untrack", 3));
            Instruction callUntrackItem = Instruction.Create(OpCodes.Call, untrackItemMethod);

            // For getting 'point' property from ItemSpawnpoint objects
            MethodDefinition getPointProperty = GetMethod(gamedef.GetType("SDG.Unturned.ItemSpawnpoint"), "get_point", 0);

            // Track dropItem
            MethodDefinition dropItemMethod = GetMethod(type, "dropItem", 5);
            InjectInstructions(dropItemMethod.Body, 88, new Instruction[]
            {
                // Item item
                Instruction.Create(OpCodes.Ldarg_0),
                // Vector3 point
                Instruction.Create(OpCodes.Ldarg_1),
                // ItemModelTracker.Track(item, point);
                callTrackItem
            });

            // Track generateItems
            MethodDefinition generateItemsMethod = GetMethod(type, "generateItems", 2);
            InjectInstructions(generateItemsMethod.Body, 68, new Instruction[]
            {
                // Item newItem
                Instruction.Create(OpCodes.Ldloc, generateItemsMethod.Body.Variables[7]),
                // Vector3 itemSpawnpoint2.point
                Instruction.Create(OpCodes.Ldloc_S, generateItemsMethod.Body.Variables[5]),
                Instruction.Create(OpCodes.Callvirt, getPointProperty),
                // ItemModelTracker.Track(newItem, itemSpawnpoint2.point);
                callTrackItem
            });

            // Track respawnItems
            MethodDefinition respawnItemsMethod = GetMethod(type, "respawnItems", 0);
            InjectInstructions(respawnItemsMethod.Body, 111, new Instruction[]
            {
                // Item item2
                Instruction.Create(OpCodes.Ldloc_3),
                // Vector3 itemSpawnpoint.point
                Instruction.Create(OpCodes.Ldloc_0),
                Instruction.Create(OpCodes.Callvirt, getPointProperty),
                // ItemModelTracker.Track(newItem, itemSpawnpoint2.point);
                callTrackItem
            });

            // Untrack askTakeItem
            MethodDefinition askTakeItemMethod = GetMethod(type, "askTakeItem", 4);
            InjectInstructions(askTakeItemMethod.Body, 95, new Instruction[]
            {
                // byte x
                Instruction.Create(OpCodes.Ldarg_2),
                // byte y
                Instruction.Create(OpCodes.Ldarg_3),
                // ushort num
                Instruction.Create(OpCodes.Ldloc_2),
                // ItemModelTracker.Untrack(x, y, num);
                callUntrackItem
            });

            // Untrack despawnItems
            MethodDefinition despawnItemsMethod = GetMethod(type, "despawnItems", 0);
            InjectInstructions(despawnItemsMethod.Body, 50, new Instruction[]
            {
                // ItemManager.despawnItems_X
                Instruction.Create(OpCodes.Ldsfld, GetField(type, "despawnItems_X")),
                // ItemManager.despawnItems_Y
                Instruction.Create(OpCodes.Ldsfld, GetField(type, "despawnItems_Y")),
                // int i
                Instruction.Create(OpCodes.Ldloc_0),
                // ItemModelTracker.Untrack(ItemManager.despawnItems_X, ItemManager.despawnItems_Y, i);
                callUntrackItem
            });
        }
    }
}
