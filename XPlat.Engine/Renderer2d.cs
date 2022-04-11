using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;

namespace XPlat.Engine
{

    public class Renderer2d
    {
       private readonly IPlatform platform;
        private SpriteBatch batch;

        public Renderer2d(IPlatform platform)
       {
           this.platform = platform;
           this.batch = new SpriteBatch();
       }

       public void Render(Scene scene){
           GL.ClearColor(0, 0, 0, 1);
           GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

           batch.Begin((int)platform.WindowSize.X, (int)platform.WindowSize.Y);
           Visit(scene.RootNode);
           batch.End();
       }

       private void Visit(Node node){
           var transform = node.Transform;

           foreach(var c in node.Components){
               switch(c){
                   case SpriteComponent sprite:
                        if(sprite.Sprite != null) {
                            batch.SetSprite(sprite.Sprite);
                            batch.Draw(ref node._globalMatrix);
                        }
                       break; 
               }
           }

           foreach (var n in node.Children){
               Visit(n);
           }
       }

    }
}