
using System;
using System.Collections.Generic;
using System.Text;
using Imperialism2SaveEditor.Core;
using Imperialism2SaveEditor.Models;

namespace Imperialism2SaveEditor.Services
{
    public class NationDetector
    {
        private static readonly string[] NationNames =
        {
            "Holland",
            "Portugal",
            "Spain",
            "France",
            "Sweden",
            "England"
        };

        private const int TreasuryDistance = 86;

        public List<GameNation> FindCandidates(
            ImperialismSaveFile save)
        {
            if (!save.IsLoaded)
                throw new InvalidOperationException(
                    "No savegame is loaded.");

            List<GameNation> candidates = new();

            foreach (string name in NationNames)
            {
                byte[] nameBytes = Encoding.ASCII.GetBytes(name);

                for (int offset = 0;
                     offset <= save.Data.Length - nameBytes.Length;
                     offset++)
                {
                    bool matches = true;

                    for (int i = 0; i < nameBytes.Length; i++)
                    {
                        if (save.Data[offset + i] != nameBytes[i])
                        {
                            matches = false;
                            break;
                        }
                    }

                    if (!matches)
                        continue;

                    int treasuryOffset =
                        offset + nameBytes.Length + TreasuryDistance;

                    if (treasuryOffset > save.Data.Length - 4)
                        continue;

                    uint treasury = save.ReadUInt32(treasuryOffset);

                    // Diagnostic filter only, not proof of identity.
                    if (treasury > 100_000_000)
                        continue;

                    candidates.Add(new GameNation
                    {
                        Name = name,
                        TreasuryOffset = treasuryOffset
                    });
                }
            }

            return candidates;
        }

        public GameNation DetectPlayerNation(ImperialismSaveFile save)
        {
            if (!save.IsLoaded)
                throw new InvalidOperationException(
                    "No savegame is loaded.");

            const int playerNameOffset = 0x1980;
            const int maxNameLength = 32;

            byte[] nameBytes = save.ReadBytes(
                playerNameOffset, maxNameLength);

            int nameLength = Array.IndexOf(nameBytes, (byte)0);

            if (nameLength <= 0)
                throw new InvalidOperationException(
                    "Unable to identify the player nation.");

            string nationName = Encoding.ASCII.GetString(
                nameBytes, 0, nameLength);

            if (Array.IndexOf(NationNames, nationName) < 0)
                throw new InvalidOperationException(
                    $"Unrecognized player nation: {nationName}");

            return new GameNation
            {
                Name = nationName,
                IsHumanControlled = true
            };
        }


        public int DetectPlayerTreasuryOffset(
            ImperialismSaveFile save)
        {
            GameNation player = DetectPlayerNation(save);

            // Player-controlled nation records contain a leading
            // space and two consecutive length-prefixed names.
            string storedName = " " + player.Name;
            byte[] nameBytes = Encoding.ASCII.GetBytes(storedName);

            int nameLength = nameBytes.Length;
            int recordLength = (nameLength + 1) * 2;

            List<int> matches = new();

            for (int offset = 0;
                 offset <= save.Data.Length - recordLength;
                 offset++)
            {
                bool valid = true;

                for (int copy = 0; copy < 2 && valid; copy++)
                {
                    int nameOffset = offset + copy * (nameLength + 1);

                    if (save.Data[nameOffset] != nameLength)
                    {
                        valid = false;
                        break;
                    }

                    for (int i = 0; i < nameLength; i++)
                    {
                        if (save.Data[nameOffset + 1 + i] != nameBytes[i])
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                if (!valid)
                    continue;

                int treasuryOffset = offset + recordLength + TreasuryDistance;

                if (treasuryOffset > save.Data.Length - 4)
                    continue;

                matches.Add(treasuryOffset);
            }

            if (matches.Count != 1)
                throw new InvalidOperationException(
                    $"Expected one treasury record for {player.Name}, " +
                    $"but found {matches.Count}.");

            return matches[0];
        }


    }
}
