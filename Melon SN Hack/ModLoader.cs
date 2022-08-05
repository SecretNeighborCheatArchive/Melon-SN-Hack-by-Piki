using System;
using System.IO;
using System.Reflection;

namespace Melon_SN_Hack
{
    class ModLoader
    {
        public static byte[] DecryptBytes(byte[] bytes)
        {
            for (int a = 0; a < bytes.Length; a++)
            {
                bytes[a] = (byte)(255 - bytes[a]);
            }
            Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] GetResourceBytes(string path)
        {
            byte[] assemblyBytes;
            using (Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            {
                byte[] buffer = new byte[str.Length];
                Console.WriteLine(str.Length);
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = str.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    assemblyBytes = ms.ToArray();
                }
            }
            return assemblyBytes;
        }
    }
}
