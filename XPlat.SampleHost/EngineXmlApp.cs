using System.Numerics;
using SDL2;
using XPlat.Core;
using XPlat.Engine;
using XPlat.Engine.Components;
using XPlat.Engine.Serialization;
using XPlat.Graphics;
using XPlat.NanoVg;

namespace XPlat.SampleHost
{


    public class EngineXmlApp : ISdlApp
    {
        private Scene scene;
        private Renderer renderer;
        private SceneVisualizer sceneViz;
        private readonly IPlatform platform;
        private readonly ISdlPlatformEvents events;
        int i = 0;

        public EngineXmlApp(IPlatform platform, ISdlPlatformEvents events)
        {
            //platform.AutoSwap = false;
            this.platform = platform;
            this.events = events;

            events.Subscribe(SDL.SDL_EventType.SDL_KEYUP, OnKeyUp);

            this.sceneViz = new SceneVisualizer(platform);
        }

        public void OnKeyUp(SDL.SDL_EventType type, ref SDL.SDL_Event ev)
        {
            if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_F5)
            {
                LoadScene();
            }
        }

        private void LoadScene()
        {
            renderer = new Renderer(platform);
            if(scene != null) scene.Dispose();
            scene = SceneReader.Load("assets/gltf_scene_2.xml");
            scene.Init();
        }

        public void Init()
        {
            LoadScene();
        }

        public void Update()
        {
            scene.Update();
            renderer.Render(scene);
            sceneViz.Draw(scene);
        }
    }

    [SceneElement("xsample-control")]
    public class ControlComponent : Behaviour
    {
        private Vector3 r;

        public override void Init()
        {
            this.r = Node.Transform.RotationDeg;
        }

        public override void Update()
        {
            var t = Node.GetGlobalTransform();

            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_w)){
                Node.Transform.Translation -= (Node.Transform.Forward * 0.1f);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_s)){
                Node.Transform.Translation += (Node.Transform.Forward * 0.1f);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_d)){
                Node.Transform.Translation += (Node.Transform.Right * 0.1f);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_a)){
                Node.Transform.Translation -= (Node.Transform.Right * 0.1f);
            }

            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_LEFT)){
                r += new Vector3(0,1,0);
                Node.Transform.RotateDeg(r);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_RIGHT)){
                r += new Vector3(0,-1,0);
                Node.Transform.RotateDeg(r);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_UP)){
                r += new Vector3(1,0,0);
                Node.Transform.RotateDeg(r);
            }
            if(Input.IsKeyDown(SDL.SDL_Keycode.SDLK_DOWN)){
                r += new Vector3(-1,0,0);
                Node.Transform.RotateDeg(r);
            }


        }
    }

    [SceneElement("xsample-cube")]
    public class CubeComponent : Behaviour
    {
        private float r;
        private Vector3 v;

        public override void Init()
        {
            this.r = 0;
            this.v = Node.Transform.RotationDeg;
        }

        // Rotate the cube
        public override void Update()
        {
            v.Y = Time.RunningTime * 50;
            Node.Transform.RotateDeg(v);
            //Node.Transform.Translation += new Vector3(0, 0, -0.01f);
        }

    }

    public class SceneVisualizer
    {
        private NVGcontext vg;
        private IPlatform platform;

        public SceneVisualizer(IPlatform platform)
        {
            this.platform = platform;
            this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS |
                        NVGcreateFlags.NVG_STENCIL_STROKES);
        }

        public void Draw(Scene scene)
        {
            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
            DrawBackdrop();
            
            vg.FontFace("sans");
            vg.FontSize(18);
            vg.FillColor("#fff");

            float x = 30, y = 30;
            DrawNode(scene.RootNode, x, ref y);

            vg.EndFrame();
        }

        private void DrawNode(Node n, float x, ref float y){
            vg.Text(x, y, n.Name);
            x += 15;
            foreach (var c in n.Children)
            {
                y += 20;
                DrawNode(c, x, ref y);
            }
        }

        private void DrawBackdrop(){
            vg.RoundedRect(10, 10, 200, 600, 10);
            vg.FillColor("#00000088");
            vg.Fill();
        }
    }
}