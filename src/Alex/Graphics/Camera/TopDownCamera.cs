using Microsoft.Xna.Framework;

namespace Alex.Graphics.Camera
{
    public class TopDownCamera : ThirdPersonCamera
    {
        public float CameraHeight { get; set; } = 128f;
        
        public override Vector3 Position
        {
            get => base.Position;
            set
            {
                base.Position = new Vector3(value.X, CameraHeight, value.Z);
                Target = value;
            }
        }

        public override Vector3 Rotation
        {
            get => base.Rotation;
            set
            {
                base.Rotation = new Vector3(0f, value.Y, 0f);
            } 
        }

        public TopDownCamera(int renderDistance, Vector3 position, Vector3 rotation) : base(16, position, rotation)
        {
        }

        public override void UpdateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.CreateOrthographic(256f, 256f * AspectRatio, NearDistance, FarDistance);
        }

        protected override void UpdateViewMatrix()
        {
            Matrix rotationMatrix = //Matrix.CreateRotationX(-Rotation.Z) * //Pitch
                                    Matrix.CreateRotationY(Rotation.Y);  //Yaw
            
            Target = Position;

            Direction = Vector3.Transform(Vector3.Backward, rotationMatrix);

            var up = Vector3.Transform(Vector3.Forward, rotationMatrix);
            ViewMatrix = Matrix.CreateLookAt(Position, Target, up);
        }
        
    }
}
