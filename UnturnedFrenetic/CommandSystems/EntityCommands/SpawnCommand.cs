using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;
using System.Reflection;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    class SpawnCommand: AbstractCommand
    {
        public SpawnCommand()
        {
            Name = "spawn";
            Arguments = "<entity type> <location>";
            Description = "Spawns an entity at the location.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            AnimalAssetTag asset = AnimalAssetTag.For(entry.GetArgument(0));
            if (asset == null)
            {
                entry.Bad("Invalid entity type!");
                return;
            }
            LocationTag loc = LocationTag.For(entry.GetArgument(1));
            if (loc == null)
            {
                entry.Bad("Invalid location!");
                return;
            }
            AnimalManager.manager.addAnimal(asset.Internal.id, loc.ToVector3(), 0, false);
            Animal animal = AnimalManager.animals[AnimalManager.animals.Count - 1];
            foreach (SteamPlayer player in PlayerTool.getSteamPlayers())
            {
                AnimalManager.manager.channel.openWrite();
                AnimalManager.manager.channel.write((ushort)AnimalManager.animals.Count);
                AnimalManager.manager.channel.write(new object[]
                {
                    animal.id,
                    animal.transform.position,
                    MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y),
                    animal.isDead
                });
                AnimalManager.manager.channel.closeWrite("tellAnimals", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
            }
            // TODO: Maybe add the animal as a var to the current commandqueue.
            entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
        }
    }
}
