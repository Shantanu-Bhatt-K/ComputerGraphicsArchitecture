using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputerGraphicsArchitecture.EngineClasses.StaticClasses
{
    public static class FileReader
    {
        static ContentManager Content;


        public static void  Init(ContentManager _content)
        {
            Content= _content;
        }

        public static  T ReadContent<T>(string Location)
        {
           
            try
            {
                T obj = Content.Load<T>(Location);
                return obj;
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: File could not be found!");
                Console.WriteLine("Exception Message: " + e.Message);
                return default;
            }
        }
        public static T ReadXML<T>(string Location)
        {
            try
            {
                using StreamReader reader = new StreamReader(Location);
                return (T)new XmlSerializer(typeof(T)).Deserialize(reader.BaseStream);
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
                return default(T);
            }

        }
        public static void WriteXML<T>(string Location,T Data) 
        {

            try
            {
                using (StreamWriter writer = new StreamWriter(Location))
                {
                    new XmlSerializer(typeof(T)).Serialize(writer.BaseStream, Data);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error writing XML: " + ex.Message);
                // You can also log the exception or handle it based on your application's requirements
            }


        }
    }
}
