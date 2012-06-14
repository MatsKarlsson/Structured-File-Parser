using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FlatFileConsole.Lib
{
    public static class XmlUtils
    {
        public static T FromXml<T>(string data) where T : class, new()
        {
            T retVal = default(T);
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
                {
                    var xmlSer = new XmlSerializer(typeof(T));
                    retVal = (T)xmlSer.Deserialize(ms);
                    Debug.Assert(retVal != null, "Unable to deserialize");
                }
                return retVal;
            }
            catch (Exception)
            {
                return retVal;
            }
        }

        public static string ToXml<T>(T obj)
        {
            try
            {
                using (var ms = new MemoryStream())
                using (var sr = new StreamReader(ms))
                {
                    var xmlSer = new XmlSerializer(typeof(T));
                    xmlSer.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    var ret = sr.ReadToEnd();
                    return ret;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
