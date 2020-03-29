using Alex.API.Graphics.Textures;
using Alex.API.Gui.Elements;
using Alex.API.Gui.Graphics;
using Alex.API.Utils;
using Alex.GameStates;
using Alex.Graphics.Camera;
using Alex.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RocketUI;

namespace Alex.Gui
{
    public class GuiMiniMap : GuiElement
    {
        public GuiMiniMapCamera Camera { get; }
        public PlayerLocation PlayerLocation { get; set; }

        private ChunkManager ChunkManager { get; }

        private RenderTarget2D _mapTexture;
        private Rectangle _previousBounds;
        private bool _canRender;

        public GuiMiniMap(ChunkManager chunkManager)
        {
            ChunkManager = chunkManager;
            PlayerLocation = new PlayerLocation(Vector3.Zero);
            Background = GuiTextures.PanelGeneric;
            
            AutoSizeMode = AutoSizeMode.None;
            Width = 128;
            Height = 128;
            Margin = new Thickness(10, 10);
            Anchor = Alignment.TopRight;

            Camera = new GuiMiniMapCamera(Vector3.Zero);
        }

        protected override void OnInit(IGuiRenderer renderer)
        {
            base.OnInit(renderer);

            InitMiniMap(Alex.Instance.GraphicsDevice);
            Background.Texture = (TextureSlice2D) _mapTexture;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            if (_canRender)
            {
                var bounds = Screen.RenderBounds;

                if (bounds != _previousBounds)
                {
                    //var c = bounds.Center;
                    //Camera.RenderPosition = new Vector3(c.X, c.Y, 0.0f);
                    //Camera.UpdateProjectionMatrix();
                    //Camera.UpdateAspectRatio((float) bounds.Width / (float) bounds.Height);
                    Camera.UpdateAspectRatio(bounds.Width / (float) bounds.Height);
                    _previousBounds = bounds;
                }

                var updateArgs = new UpdateArgs()
                {
                    GraphicsDevice = Alex.Instance.GraphicsDevice,
                    GameTime       = gameTime,
                    Camera         = Camera
                };

                Camera.Update(updateArgs, PlayerLocation);
            }
        }

        protected override void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            base.OnDraw(graphics, gameTime);

            if (_canRender)
            {
                var renderArgs = new RenderArgs()
                {
                    GraphicsDevice = graphics.SpriteBatch.GraphicsDevice,
                    SpriteBatch    = graphics.SpriteBatch,
                    GameTime       = gameTime,
                    Camera         = Camera,
                };

                using (var context = graphics.BranchContext(BlendState.AlphaBlend, DepthStencilState.Default,
                                                            RasterizerState.CullClockwise, SamplerState.PointWrap))
                {
                    var bounds = RenderBounds;

                    bounds.Inflate(-3, -3);

                    var p  = graphics.Project(bounds.Location.ToVector2());
                    var p2 = graphics.Project(bounds.Location.ToVector2() + bounds.Size.ToVector2());

                    var newViewport = Camera.Viewport;
                    newViewport.X      = (int) p.X;
                    newViewport.Y      = (int) p.Y;
                    newViewport.Width  = (int) (p2.X - p.X);
                    newViewport.Height = (int) (p2.Y - p.Y);

                    Camera.Viewport = newViewport;
                    Camera.UpdateProjectionMatrix();

                    context.Viewport = Camera.Viewport;
                    
                    graphics.Begin();
                    
                    ChunkManager.Draw(renderArgs);
                    
                    graphics.End();
                }
            }
            
            if (_mapTexture == null) return;

            var device = graphics.Context.GraphicsDevice;
            device.SetRenderTarget(_mapTexture);

            RenderMiniMap(device, graphics.SpriteBatch, gameTime);

            device.SetRenderTarget(null);
        }

        
        private void InitMiniMap(GraphicsDevice device)
        {
            _mapTexture = new RenderTarget2D(device, Width, Height, false, SurfaceFormat.Color, DepthFormat.None);

        }

        private void RenderMiniMap(GraphicsDevice device, SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = _mapTexture.Bounds;
            var center = bounds.Center;

            var renderArgs = new RenderArgs()
            {
                Camera         = Camera,
                GameTime       = gameTime,
                SpriteBatch    = spriteBatch,
                GraphicsDevice = device
            };

            ChunkManager.Draw(renderArgs);
        }
        
        public class GuiMiniMapCamera : Camera
        {
            public Viewport Viewport { get; set; }
            
            public float PositionY { get; set; } = 256f;

            public GuiMiniMapCamera(Vector3 basePosition) : base(2)
            {
                Viewport = new Viewport(128, 128, 128, 128, 0.01f, 16.0f);
                Position = basePosition;
                Rotation = Vector3.Zero;
                FOV      = 25.0f;

                UpdateAspectRatio(Viewport.AspectRatio);
            }
            
            protected override void UpdateViewMatrix()
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(Rotation.Y);

                Vector3 forward = Vector3.Transform(Vector3.Forward, rotationMatrix);

                Target = Position;

                Direction = Vector3.Down;

                var pos = new Vector3(Position.X, PositionY, Position.Z);
                ViewMatrix = Matrix.CreateWorld(pos, Vector3.Down, forward);
                //ViewMatrix = Matrix.CreateLookAt(new Vector3(Position.X, PositionY, Position.Z), Target, lookAtOffset);
            }

            public override void UpdateProjectionMatrix()
            {
                //ProjectionMatrix = Matrix.CreatePerspectiveOffCenter(Viewport.RenderBounds, NearDistance, FarDistance);
                // ProjectionMatrix =
                //     Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Viewport.AspectRatio,
                //                                         NearDistance, FarDistance);
                
                ProjectionMatrix =
                    Matrix.CreateOrthographicOffCenter(Viewport.Bounds, Viewport.MinDepth, Viewport.MaxDepth);
            }
        }
    }
}
