
using System;
using System.IO;

namespace Imperialism2SaveEditor.Core
{
    public class ImperialismSaveFile
    {
        public string FilePath { get; private set; } = string.Empty;

        public byte[] Data { get; private set; } = Array.Empty<byte>();

        // Preserve an untouched copy of the original savegame.
        public byte[] OriginalData { get; private set; } = Array.Empty<byte>();

        public long FileSize => Data.LongLength;

        public bool IsLoaded => Data.Length > 0;

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "The selected save file does not exist.", filePath);
            }

            if (!string.Equals(
                Path.GetExtension(filePath),
                ".imp",
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException(
                    "Please select an Imperialism II .imp save file.");
            }

            byte[] fileData = File.ReadAllBytes(filePath);

            if (fileData.Length == 0)
            {
                throw new InvalidDataException(
                    "The selected save file is empty.");
            }

            Data = fileData;

            // Keep a separate, unchanged snapshot for verification.
            OriginalData = (byte[])fileData.Clone();

            FilePath = filePath;
        }

        public byte[] ReadBytes(int offset, int count)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("No save file loaded.");

            if (offset < 0 || count < 0 ||
                offset > Data.Length - count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset), "Invalid file offset or byte count.");
            }

            byte[] result = new byte[count];

            Array.Copy(Data, offset, result, 0, count);

            return result;
        }

        public ushort ReadUInt16(int offset)
        {
            byte[] bytes = ReadBytes(offset, 2);

            return System.Buffers.Binary.BinaryPrimitives
                .ReadUInt16LittleEndian(bytes);
        }

        public uint ReadUInt32(int offset)
        {
            byte[] bytes = ReadBytes(offset, 4);

            return System.Buffers.Binary.BinaryPrimitives
                .ReadUInt32LittleEndian(bytes);
        }

        public void WriteUInt32(int offset, uint value)
        {
            if (!IsLoaded)
                throw new InvalidOperationException(
                    "No savegame has been loaded.");

            if (offset < 0 || offset > Data.Length - 4)
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "The treasury offset is outside the savegame.");

            // Write a 32-bit unsigned integer in little-endian format.
            System.Buffers.Binary.BinaryPrimitives
                .WriteUInt32LittleEndian(
                    Data.AsSpan(offset, 4),
                    value);
        }


        public void WriteUInt16(int offset, ushort value)
        {
            if (!IsLoaded)
                throw new InvalidOperationException(
                    "No savegame has been loaded.");

            if (offset < 0 || offset > Data.Length - 2)
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    "The resource offset is outside the savegame.");

            System.Buffers.Binary.BinaryPrimitives
                .WriteUInt16LittleEndian(
                    Data.AsSpan(offset, 2),
                    value);
        }

    }
}
