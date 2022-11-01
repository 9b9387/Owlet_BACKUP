// AssetInfo.cs
// Author: shihongyang shihongyang@weile.com
// Data: 11/1/2022

namespace Owlet
{
    public class AssetInfo
    {
        public string name;
        public string md5;
        public long time;
        public long size;

        public override string ToString()
        {
            return $"{name}({md5}) time:{time} size:{size}";
        }
    }
}