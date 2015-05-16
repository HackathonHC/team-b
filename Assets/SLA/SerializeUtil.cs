using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    static class SerializeUtil
    {
        // FIXME: あくまで開発中の暫定機能。BinaryFormatterは互換性に問題がある。
        static public byte[] Serialize(object serializableObject)
        {
#if UNITY_IOS
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            formatter.Serialize(stream, serializableObject);
            return stream.GetBuffer();
        }
        
        static public object Deserialize(byte[] src)
        {
#if UNITY_IOS
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream(src);
            return formatter.Deserialize(stream);
        }
    }
}
