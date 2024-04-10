using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Content.Load<T>(Location);
        }
    }
}
