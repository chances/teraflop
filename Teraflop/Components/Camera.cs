using System;
using System.Drawing;
using System.Numerics;
using Teraflop.Buffers.Uniforms;
using Teraflop.Components.Geometry;
using Teraflop.Components.Receivers;
using Teraflop.ECS;

namespace Teraflop.Components {
	public class Camera : Resource, IFramebufferSize, IUpdatable {
		private UniformMatrix _viewProj;
		// TODO: Implement tweener from MonoGame.Extended.Tween
		//        TweeningComponent _tweener;

		public Camera() : base(nameof(Camera)) {
			//            _tweener = new TweeningComponent(game, new AnimationComponent(game));

			Resources.Initializing += (_, e) => {
				var factory = e.ResourceFactory;
				_viewProj = new UniformViewProjection(ViewProjection);
				_viewProj.Buffer.Initialize(factory, e.GraphicsDevice);
			};
			Resources.Disposed += (_, __) => _viewProj.Buffer.Dispose();
		}

		public Size FramebufferSize { get; set; } = new Size(960, 540);

		public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(Position, LookAt, Basis.Default.Up);

		public Matrix4x4 ProjectionMatrix {
			get {
				var fieldOfView = (float)Math.PI / 2.0f; // 90 degrees
				float nearClipPlane = 0.1f;
				float farClipPlane = 200;
				var aspectRatio = FramebufferSize.Width / (float)FramebufferSize.Height;

				return Matrix4x4.CreatePerspectiveFieldOfView(
					fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
			}
		}

		public Vector3 Position { get; set; } = Basis.Default.Up * 3f;

		public Vector3 LookAt { get; set; } = Vector3.Zero;

		public Basis Basis => new Basis(Position, LookAt);

		public UniformBuffer<Matrix4x4> ViewProjectionUniform => _viewProj.Buffer;

		private Matrix4x4 ViewProjection => Matrix4x4.Multiply(ViewMatrix, ProjectionMatrix);

		public virtual void Update(GameTime gameTime) {
			// TODO: Do tweening here with a tweener

			_viewProj.Buffer.UniformData = ViewProjection;
		}
	}
}
