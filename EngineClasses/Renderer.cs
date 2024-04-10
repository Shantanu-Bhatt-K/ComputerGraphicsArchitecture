using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class Renderer
    {
        public Texture2D texture;
        public Color colour=Color.White;
        public int Width
        { get { return texture.Width; } }
        public int Height
        { get { return texture.Height; } }
    }
}
