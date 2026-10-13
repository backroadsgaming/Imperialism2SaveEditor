
using System;
using System.Collections.Generic;
using System.Buffers.Binary;
using Imperialism2SaveEditor.Core;
using Imperialism2SaveEditor.Models;

namespace Imperialism2SaveEditor.Services
{
    public class WarehouseReader
    {

        private static readonly (string Name, int Offset)[] ResourceFields =
        {
            ("Wool", 0),
            ("Timber", 2),
            ("Tin", 4),
            ("Copper", 6),
            ("Iron Ore", 8),
            ("Coal", 10),
            ("Cotton", 12),
            ("Raw Sugar", 14),
            ("Tobacco", 16),
            ("Fur", 18),
            ("Horses", 20),
            ("Fabric", 22),
            ("Lumber", 24),
            ("Paper", 26),
            ("Bronze", 28),
            ("Iron", 30),
            ("Steel", 32),
            ("Sugar", 34),
            ("Cigars", 36),
            ("Hats", 38),
            ("Grain", 40),
            ("Fish", 42),
            ("Meat", 44)
        };

        public List<GameResource> ReadResources(
            ImperialismSaveFile save,
            int warehouseOffset)
        {
            if (!save.IsLoaded)
                throw new InvalidOperationException(
                    "No save file has been loaded.");

            List<GameResource> resources = new();

            foreach (var field in ResourceFields)
            {
                int absoluteOffset = checked(
                    warehouseOffset + field.Offset);

                byte[] bytes = save.ReadBytes(absoluteOffset, 2);

                int quantity = BinaryPrimitives.ReadUInt16LittleEndian(bytes);

                resources.Add(new GameResource(
                    field.Name,
                    quantity,
                    absoluteOffset));
            }

            return resources;
        }

        public int DetectWarehouseOffset(ImperialismSaveFile save)
        {
            if (!save.IsLoaded)
                throw new InvalidOperationException(
                    "No savegame is loaded.");

            const int resourceBlockSize = 46;

            // Marker verified against seven savegames.
            // 21 zero bytes, 01 01, eight zero bytes, 01 00 00.
            byte[] marker = new byte[34];
            marker[21] = 0x01;
            marker[22] = 0x01;
            marker[32] = 0x01;

            int matchCount = 0;
            int warehouseOffset = -1;

            for (int offset = resourceBlockSize;
                 offset <= save.Data.Length - marker.Length;
                 offset++)
            {
                bool matches = true;

                for (int i = 0; i < marker.Length; i++)
                {
                    if (save.Data[offset + i] != marker[i])
                    {
                        matches = false;
                        break;
                    }
                }

                if (!matches)
                    continue;

                matchCount++;
                warehouseOffset = offset - resourceBlockSize;
            }

            if (matchCount != 1)
                throw new InvalidOperationException(
                    $"Warehouse detection found {matchCount} matches. " +
                    "Expected exactly one.");

            return warehouseOffset;
        }

    }
}
