using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Teraflop.Components.Receivers;
using Teraflop.ECS;
using Teraflop.Primitives;
using Veldrid;

namespace Teraflop.Components.UI
{
    public class Surface : ResourceComponent, IFramebufferSize, IReady, IUpdatable, IResourceSet, IDrawAction
    {
        private GraphicsDevice _device;
        private Texture _texture;
        private TextureView _textureView;
        private Sampler _sampler;
        private Size _size = new Size(0, 0);

        public Surface() : base("UI Surface")
        {
            Resources.OnInitialize += (_, e) => {
                _device = e.GraphicsDevice;

                _size = new Size(
                    (int) _device.SwapchainFramebuffer.Width,
                    (int) _device.SwapchainFramebuffer.Height
                );

                _sampler = _device.LinearSampler;

                ResourceLayout = e.ResourceFactory.CreateResourceLayout(
                    new ResourceLayoutDescription(
                        new ResourceLayoutElementDescription(
                            "SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                        new ResourceLayoutElementDescription(
                            "SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

                CreateTexture();
            };
            Resources.OnDispose += (_, __) => {
                _textureView.Dispose();
                _texture.Dispose();
                ResourceSet.Dispose();
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

        public ResourceLayout ResourceLayout { get; private set; }

        public ResourceSet ResourceSet { get; private set; }

        public void Draw(Action drawDelegate)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Draw the GUI

            // TODO: Convert the pixel data to applicable format, update Veldrid texture
            // var sourceData = Bgra32ToRgba32(surface.Data, surface.Width, surface.Height);
            // var width = (uint) surface.Width;
            // var height = (uint) surface.Height;
            // var gcHandle = GCHandle.Alloc(sourceData, GCHandleType.Pinned);
            // _device?.UpdateTexture(_texture, gcHandle.AddrOfPinnedObject(), (uint) sourceData.Length, 0, 0, 0, width, height, 1, 0, 0);
            // gcHandle.Free();
        }

        private void CreateTexture()
        {
            var factory = _device.ResourceFactory;

            _texture?.Dispose();
            _texture = factory.CreateTexture(TextureDescription.Texture2D(
                (uint) _size.Width, (uint) _size.Height,
                1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Sampled));
            _textureView?.Dispose();
            _textureView = factory.CreateTextureView(_texture);

            ResourceSet?.Dispose();
            ResourceSet = factory.CreateResourceSet(new ResourceSetDescription(
                ResourceLayout, _textureView, _sampler
            ));
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
    }
}
