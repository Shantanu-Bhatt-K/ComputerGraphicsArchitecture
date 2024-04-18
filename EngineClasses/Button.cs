using ComputerGraphicsArchitecture.DebugClasses;
using ComputerGraphicsArchitecture.EngineClasses.InputManagement;
using ComputerGraphicsArchitecture.EngineClasses.StaticClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsArchitecture.EngineClasses
{
    internal class Button : GameObject
    {
        public Action OnClick;
        Rectangle rect;
        public override void Init(params object[] b)
        {
            base.Init(b);
            transform.scale = (Vector2)b[2];
            CommandManager.AddMouseBinding(MouseButton.LEFT, CheckButtonClick);
            CommandManager.AddMouseBinding(CheckMousePos);
            Point scale=new Point((int)(renderer.Width*transform.scale.X), (int)(renderer.Height* transform.scale.Y));
            rect = new Rectangle(transform.position.ToPoint() - new Point(scale.X / 2, scale.Y / 2), scale);

        }

        private void CheckButtonClick(eButtonState buttonState, Point position, Point delta)
        {
            if (rect.Contains(position) && buttonState==eButtonState.PRESSED)
                OnClick?.Invoke();
        }
        private void CheckMousePos(eButtonState buttonState, Point position, Point delta)
        {
            if(rect.Contains(position))
                renderer.colour = Color.LightGray;
            
            else
                renderer.colour = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(ref SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            base.Draw(ref spriteBatch, gameTime);
            //Primitives2D.DrawRectangle(spriteBatch, rect, Color.Red);
        }

        public void DestroyButton()
        {
            CommandManager.RemoveMouseButtons(MouseButton.LEFT, CheckButtonClick);
            CommandManager.RemoveMouseButtons( CheckMousePos);
        }


        
        

    }
}
