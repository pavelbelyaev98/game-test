using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SomethingDownThere
{
    public sealed class UnsupportedSaveException : IOException
    { public UnsupportedSaveException() : base("This save needs a different game version. Its files have been kept.") { } }

    // Bounded, checksummed binary format. Version 1 remains a supported reader when
    // later tasks add state; migrate explicitly instead of regenerating an old world.
    public static class WorldSaveCodec
    {
        public const int Version = 2;
        private const int MaximumBytes = 72 * 1024 * 1024;
        private static readonly byte[] Magic = Encoding.ASCII.GetBytes("SDTSAVE\0");

        public static void Write(Stream destination, WorldSnapshot s)
        {
            s.Validate();
            using var packed = new MemoryStream();
            using (var zip = new GZipStream(packed, System.IO.Compression.CompressionLevel.Fastest, true))
            using (var w = new BinaryWriter(zip, Encoding.UTF8, true))
            {
                w.Write(s.Sequence); w.Write(s.UtcTicks); WriteString(w, s.SiteId);
                var g = s.Terrain;
                w.Write(g.Size.x); w.Write(g.Size.y); w.Write(g.Size.z); w.Write(g.CellSize);
                w.Write(g.Revision); w.Write(g.LowestCarvedY); w.Write(g.RemovedVolume);
                Write(w, s.TerrainPosition); Write(w, s.TerrainRotation);
                w.Write(s.ExcavationSeed); w.Write(s.DiscoverySeed);
                w.Write(s.InventoryCapacity); w.Write(s.Credits); w.Write(s.ShovelLevel); w.Write(s.SuccessfulStrokes);
                w.Write(s.BatteryCapacity); w.Write(s.BatteryCharge); w.Write(s.Pitch); w.Write(s.VerticalSpeed);
                Write(w, s.PlayerPosition); Write(w, s.PlayerRotation);
                w.Write(s.Finds.Length);
                foreach (var f in s.Finds)
                {
                    WriteString(w, f.ContentId); Write(w, f.Item); Write(w, f.Position); Write(w, f.Rotation); Write(w, f.Scale); w.Write(f.Collected);
                }
                w.Write(s.Inventory.Length);
                foreach (var item in s.Inventory) Write(w, item);
                w.Write(g.Density.Length);
                var bytes = new byte[g.Density.Length * sizeof(float)];
                Buffer.BlockCopy(g.Density, 0, bytes, 0, bytes.Length);
                w.Write(bytes);
                w.Write(s.CrouchAmount);
            }
            using var hash = SHA256.Create();
            byte[] payload = packed.ToArray();
            using var header = new BinaryWriter(destination, Encoding.UTF8, true);
            header.Write(Magic); header.Write(Version); header.Write(payload.Length);
            header.Write(hash.ComputeHash(payload)); header.Write(payload);
        }

        public static WorldSnapshot Read(Stream source)
        {
            using var header = new BinaryReader(source, Encoding.UTF8, true);
            var magic = ReadExact(header, Magic.Length);
            for (int i = 0; i < magic.Length; i++) WorldSnapshot.Require(magic[i] == Magic[i], "Unrecognized save file.");
            int version = header.ReadInt32();
            if (version < 1 || version > Version) throw new UnsupportedSaveException();
            int length = Count(header, MaximumBytes);
            byte[] expected = ReadExact(header, 32), payload = ReadExact(header, length);
            WorldSnapshot.Require(source.ReadByte() == -1, "Unexpected data after checkpoint.");
            using (var hash = SHA256.Create())
            {
                byte[] actual = hash.ComputeHash(payload);
                for (int i = 0; i < actual.Length; i++) WorldSnapshot.Require(actual[i] == expected[i], "The checkpoint is incomplete or damaged.");
            }
            using var packed = new MemoryStream(payload, false);
            using var zip = new GZipStream(packed, CompressionMode.Decompress);
            using var unpacked = new MemoryStream();
            var buffer = new byte[65536];
            int read;
            while ((read = zip.Read(buffer, 0, buffer.Length)) > 0)
            {
                WorldSnapshot.Require(unpacked.Length + read <= MaximumBytes, "Checkpoint exceeds the supported world size.");
                unpacked.Write(buffer, 0, read);
            }
            unpacked.Position = 0;
            using var r = new BinaryReader(unpacked, Encoding.UTF8, true);
            var s = new WorldSnapshot { Sequence = r.ReadInt64(), UtcTicks = r.ReadInt64(), SiteId = ReadString(r) };
            s.Terrain = new GridSnapshot { Size = new Vector3Int(r.ReadInt32(), r.ReadInt32(), r.ReadInt32()), CellSize = r.ReadSingle(),
                Revision = r.ReadInt32(), LowestCarvedY = r.ReadInt32(), RemovedVolume = r.ReadSingle() };
            s.TerrainPosition = ReadVector(r); s.TerrainRotation = ReadRotation(r);
            s.ExcavationSeed = r.ReadInt32(); s.DiscoverySeed = r.ReadInt32();
            s.InventoryCapacity = r.ReadInt32(); s.Credits = r.ReadInt32(); s.ShovelLevel = r.ReadInt32(); s.SuccessfulStrokes = r.ReadInt32();
            s.BatteryCapacity = r.ReadSingle(); s.BatteryCharge = r.ReadSingle(); s.Pitch = r.ReadSingle(); s.VerticalSpeed = r.ReadSingle();
            s.PlayerPosition = ReadVector(r); s.PlayerRotation = ReadRotation(r);
            s.Finds = new FindSnapshot[Count(r, 256)];
            for (int i = 0; i < s.Finds.Length; i++)
                s.Finds[i] = new FindSnapshot { ContentId = ReadString(r), Item = ReadItem(r), Position = ReadVector(r),
                    Rotation = ReadRotation(r), Scale = ReadVector(r), Collected = r.ReadBoolean() };
            s.Inventory = new ItemSnapshot[Count(r, 256)];
            for (int i = 0; i < s.Inventory.Length; i++) s.Inventory[i] = ReadItem(r);
            int samples = Count(r, 257 * 257 * 257);
            byte[] density = ReadExact(r, samples * sizeof(float));
            s.Terrain.Density = new float[samples];
            Buffer.BlockCopy(density, 0, s.Terrain.Density, 0, density.Length);
            // Version 1 had no stance and always used the standing capsule.
            s.CrouchAmount = version >= 2 ? r.ReadSingle() : 0f;
            WorldSnapshot.Require(unpacked.Position == unpacked.Length, "Unexpected checkpoint fields.");
            s.Validate();
            return s;
        }

        private static int Count(BinaryReader r, int maximum)
        { int n = r.ReadInt32(); WorldSnapshot.Require(n >= 0 && n <= maximum, "Invalid checkpoint length."); return n; }
        private static byte[] ReadExact(BinaryReader r, int n)
        { var bytes = r.ReadBytes(n); WorldSnapshot.Require(bytes.Length == n, "Interrupted checkpoint."); return bytes; }
        private static void WriteString(BinaryWriter w, string s)
        { byte[] bytes = Encoding.UTF8.GetBytes(s); WorldSnapshot.Require(bytes.Length <= 1024, "Checkpoint text is too long."); w.Write(bytes.Length); w.Write(bytes); }
        private static string ReadString(BinaryReader r) => new UTF8Encoding(false, true).GetString(ReadExact(r, Count(r, 1024)));
        private static void Write(BinaryWriter w, Vector3 v) { w.Write(v.x); w.Write(v.y); w.Write(v.z); }
        private static void Write(BinaryWriter w, Quaternion q) { w.Write(q.x); w.Write(q.y); w.Write(q.z); w.Write(q.w); }
        private static void Write(BinaryWriter w, ItemSnapshot i) { WriteString(w, i.Id); WriteString(w, i.Name); w.Write(i.Value); }
        private static Vector3 ReadVector(BinaryReader r) => new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        private static Quaternion ReadRotation(BinaryReader r) => new Quaternion(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        private static ItemSnapshot ReadItem(BinaryReader r) => new ItemSnapshot { Id = ReadString(r), Name = ReadString(r), Value = r.ReadInt32() };
    }
}
