using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Teraflop.Primitives;
using OpenTK.Graphics.ES30;

namespace Teraflop.Components.UI
{
    public class Surface : ResourceComponent, IFramebufferSize, IReady, IUpdatable, IResource, IResourceSet, IDrawAction
    {
        private int? _texture = null;
        private Size _size = new Size(0, 0);

        public Surface() : base("UI Surface")
        {
            Resources.OnInitialize = () => {
                CreateTexture();
            };
            Resources.OnDispose = () => {
                if (_texture.HasValue)
                {
                    GL.DeleteTexture(_texture.Value);
                }
            };
        }

        public static readonly MeshData Mesh =
            MeshBuilder.TexturedUnitQuad("UI Surface Mesh");

        public bool IsReady => true;

        public Size FramebufferSize
        {
            set
            {
                _size = value;

                // Recreate the UI's backing texture, sampler, and surface
                CreateTexture();
            }
        }

        public IEnumerable<ResourceLayoutElementDescription> ResourceLayout { get; private set; }

        public void Draw(Action drawDelegate)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Draw the GUI

            // TODO: Convert the pixel data to applicable format, update Veldrid texture
            // var sourceData = Bgra32ToRgba32(surface.Data, surface.Width, surface.Height);
            // var gcHandle = GCHandle.Alloc(sourceData, GCHandleType.Pinned);
            // GL.TexImage2D(_texture.Value, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, gcHandle.AddrOfPinnedObject());
            // gcHandle.Free();
        }

        private void CreateTexture()
        {
            this.ResourceLayout = new ResourceLayoutElementDescription[] {
                new ResourceLayoutElementDescription(
                    "SurfaceTexture", ResourceKind.Texture2D, ShaderStages.Fragment)
            };

            if (_texture.HasValue)
            {
                GL.DeleteTexture(_texture.Value);
            }
            var textureHandle = (_texture = GL.GenTexture()).Value;
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexImage2D<byte>((All) _texture.Value, 0, (All) PixelInternalFormat.Rgba, _size.Width, _size.Height, 0, (All) PixelFormat.Rgba, (All) PixelType.UnsignedByte, new byte[] {});
        }

        private static byte[] Bgra32ToRgba32(IReadOnlyList<byte> source, int width, int height)
        {
            var destination = new byte[source.Count];

            for (var pixel = 0; pixel < source.Count; pixel += 4)
            {
                byte b = source[pixel],
                    g = source[pixel + 1],
                    r = source[pixel + 2],
                    a = source[pixel + 3];
                destination[pixel] = r;
                destination[pixel + 1] = g;
                destination[pixel + 2] = b;
                destination[pixel + 3] = a;
            }

            return destination;
        }

        public void BindResourceSet(int shaderHandle)
        {
            throw new NotImplementedException();
        }
    }
}
