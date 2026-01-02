using System;
using System.Numerics;
using Teraflop.Components;
using Teraflop.Components.Receivers;
using Teraflop.Geometry;
using Teraflop.Input;

namespace Teraflop.Examples {
	public class OrbitCamera : Camera, IUserInput {
		public static readonly float DefaultZoom = 3.0f;
		public static readonly float DefaultYaw = 90f.DegToRad();

		private float _zoom = DefaultZoom;
		private const float ZoomPerSecond = 10;
		private readonly float OrbitPerSecond = 180f.DegToRad();

		/// <summary>
		/// Rotation around Z axis.
		/// </summary>
		public float Yaw { get; set; } = DefaultYaw;
		/// <summary>
		/// Tilt around the local right axis.
		/// </summary>
		public float Pitch { get; set; }

		public float Zoom {
			get => _zoom;
			set {
				_zoom = value;
				UpdatePosition();
			}
		}

		public Vector3 FocalPoint { get => LookAt; set => LookAt = value; }

		public float MinZoom = 1;
		public float MaxZoom = 200;
		public float MinTilt = 5f.DegToRad();
		public float MaxTilt = 85f.DegToRad();

		public MouseState MouseState { private get; set; }
		public KeyboardState KeyboardState { private get; set; }

		private bool ShouldZoomIn {
			get {
				var viaMouse = MouseState.ScrollWheelValue > 0;
				var viaKeyboard = KeyboardState.IsKeyDown(Key.Plus) || KeyboardState.IsKeyDown(Key.KeypadPlus);
				return viaMouse || viaKeyboard;
			}
		}
		private bool ShouldZoomOut {
			get {
				var viaMouse = MouseState.ScrollWheelValue < 0;
				var viaKeyboard = KeyboardState.IsKeyDown(Key.Minus) || KeyboardState.IsKeyDown(Key.KeypadMinus);
				return viaMouse || viaKeyboard;
			}
		}

		private bool ShouldOrbitLeft => KeyboardState.IsKeyDown(Key.Left) && !KeyboardState.IsKeyDown(Key.Right);
		private bool ShouldOrbitRight => KeyboardState.IsKeyDown(Key.Right) && !KeyboardState.IsKeyDown(Key.Left);
		private bool ShouldOrbitUp => KeyboardState.IsKeyDown(Key.Up) && !KeyboardState.IsKeyDown(Key.Down);
		private bool ShouldOrbitDown => KeyboardState.IsKeyDown(Key.Down) && !KeyboardState.IsKeyDown(Key.Up);

		public override void Update(GameTime gameTime) {
			var zoom = (float)gameTime.ElapsedGameTime.TotalSeconds * ZoomPerSecond;
			var orbit = (float)gameTime.ElapsedGameTime.TotalSeconds * OrbitPerSecond;

			var zoomIn = ShouldZoomIn && !ShouldZoomOut;
			var zoomOut = ShouldZoomOut && !ShouldZoomIn;

			zoom *= zoomIn ? -1 : zoomOut ? 1 : 0;
			Zoom = Math.Clamp(Zoom += zoom, MinZoom, MaxZoom);

			var orbitYaw = orbit * (ShouldOrbitLeft ? -1 : ShouldOrbitRight ? 1 : 0);
			var orbitPitch = orbit * (ShouldOrbitDown ? -1 : ShouldOrbitUp ? 1 : 0);
			Yaw += orbitYaw;
			// Clamp pitch to prevent gimbal lock
			Pitch = Math.Clamp(Pitch + orbitPitch, MinTilt, MaxTilt);

			if (KeyboardState.IsKeyDown(Key.Home)) {
				Zoom = DefaultZoom;
				Yaw = DefaultYaw;
				Pitch = 0;
			}

			UpdatePosition();

			base.Update(gameTime);
		}

		private void UpdatePosition() {
			// Build rotation quaternion with Z-up convention
			Quaternion yawRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, Yaw);
			Quaternion pitchRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, Pitch);

			// Combine rotations: Apply pitch first, then yaw.
			var rotation = yawRotation * pitchRotation;

			// Look-at direction is +X pointing towards the origin
			var direction = Vector3.Transform(Components.Geometry.Basis.Default.Right, rotation);
			Position = FocalPoint - direction * Zoom;
		}
	}
}
