using Microsoft.Xna.Framework.Graphics;

namespace Interfaces;
interface IGameObject
{
   public void Update(); 
   public void Draw(SpriteBatch spriteBatch);
}