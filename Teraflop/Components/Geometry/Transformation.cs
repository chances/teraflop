using System.Collections.Generic;
using System.Numerics;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Veldrid;

namespace Teraflop.Components.Geometry
{
    public class Transformation : ResourceComponent, ICameraViewProjection
    {
        public static Transformation Identity = new Transformation();
        private UniformModelTransformation _model =
            new UniformModelTransformation(Matrix4x4.Identity);

        public Transformation() : base(nameof(Transformation))
        {
            Resources.OnInitialize += (_, e) => {
              _model.Buffer.Initialize(e.ResourceFactory, e.GraphicsDevice);
            };
            Resources.OnDispose += (_, __) => _model.Buffer.Dispose();
        }

        public Matrix4x4 Value
        {
            get => _model.Buffer.UniformData;
            set => _model.Buffer.UniformData = value;
        }

        public Vector3 Translation
        {
            get => Value.Translation;
            set
            {
                var transformation = Value;
                transformation.Translation = value;
                Value = transformation;
            }
        }

        public Quaternion Rotation
        {
            get => Quaternion.CreateFromRotationMatrix(Value);
            set
            {
                var translation = Translation;
                Value = Matrix4x4.Transform(Matrix4x4.Identity, value);
                Translate(translation);
            }
        }

        #region Veldrid
        public UniformBuffer<Matrix4x4> CameraViewProjection { protected get; set; }
        public IEnumerable<ResourceLayoutElementDescription> ResourceLayoutElements =>
            new ResourceLayoutElementDescription[] {
                new ResourceLayoutElementDescription("Model", ResourceKind.UniformBuffer,
                    ShaderStages.Vertex),
                new ResourceLayoutElementDescription("ViewProj", ResourceKind.UniformBuffer,
                    ShaderStages.Vertex)
            };
        public IEnumerable<BindableResource> ResourceSet => new BindableResource[] {
          _model.Buffer.DeviceBuffer, CameraViewProjection.DeviceBuffer
        };
        #endregion

        public bool AreDependenciesSatisfied => CameraViewProjection?.Initialized ?? false;

        public void Translate(float x = 0, float y = 0, float z = 0)
        {
            var value = Value;
            value.Translation += new Vector3(x, y, z);
            Value = value;
        }

        public void Translate(Vector3 translation)
        {
            var value = Value;
            value.Translation += translation;
            Value = value;
        }

        public void Rotate(Quaternion rotation)
        {
            Value = Matrix4x4.Transform(Value, rotation);
        }
    }
}
