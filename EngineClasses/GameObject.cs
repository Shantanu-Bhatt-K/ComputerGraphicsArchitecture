using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class GameObject
    {
        public List<GameObject> children;
        public bool isActive;
        public  Transform transform=new();
        public Renderer renderer=new();
        

        public virtual void Init(params object[] b)
        {
            transform.position = (Vector2)b[0];
            renderer.texture = FileReader.ReadContent<Texture2D>((string)b[1]);


        }
        
        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(ref SpriteBatch spriteBatch,GameTime gameTime)
        {
           
            spriteBatch.Draw(renderer.texture,transform.position, null, renderer.colour, transform.rotation,  new Vector2(renderer.Width/2,renderer.Height/2), transform.scale, SpriteEffects.None, 0f);
        }
    }
}
