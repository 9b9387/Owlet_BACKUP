// AssetManifest.cs
// Author: shihongyang shihongyang@weile.com
// Data: 11/1/2022

using System.Collections.Generic;
using System.IO;

namespace Owlet
{
    public class AssetManifest
    {
        public long time;
        public long size;
        public List<AssetInfo> list = new List<AssetInfo>();

        public AssetManifest()
        {
        }

        public AssetManifest(byte[] data)
        {
            From(data);
        }

        public byte[] ToBytes()
        {
            var s = new MemoryStream();
            var writer = new BinaryWriter(s);
            writer.Write(time);
            writer.Write(size);
            writer.Write(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                var info = list[i];
                writer.Write(info.name);
                writer.Write(info.md5);
                writer.Write(info.time);
                writer.Write(info.size);
            }

            writer.Dispose();

            var bytes = s.ToArray();
            s.Dispose();
            return bytes;
        }

        public void From(byte[] data)
        {
            list.Clear();
            var s = new MemoryStream(data);
            var reader = new BinaryReader(s);
            time = reader.ReadInt64();
            size = reader.ReadInt64();
            var count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                var info = new AssetInfo();
                info.name = reader.ReadString();
                info.md5 = reader.ReadString();
                info.time = reader.ReadInt64();
                info.size = reader.ReadInt64();

                list.Add(info);
            }
        }
    }
}