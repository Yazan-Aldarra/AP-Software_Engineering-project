using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project;

namespace Interfaces;

public interface IGameObject
{
   // public Vector2 Position { get; set; }
   // public bool IsGrounded { get; set; }
   public void Update(GameTime gameTime);
   public void Draw(SpriteBatch spriteBatch);
   public Texture2D Texture2D { get; set; }
   public float Scale { get; set; }
   public string Tag { get; set; }
   public GameObjectState State { get; set; }
}