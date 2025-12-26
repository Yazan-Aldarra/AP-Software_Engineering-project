using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using project;

namespace Interfaces;

public interface IGameObject: ICollider
{
   public void Update(GameTime gameTime);
   public void Draw(SpriteBatch spriteBatch);
   public Texture2D Texture2D { get; set; }
   public float Scale { get; set; }
   public GameObjectState State { get; set; }
   public Vector2 GetGameObjectPos();
}