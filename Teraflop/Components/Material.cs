using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Teraflop.Assets;
using Teraflop.ECS;
using JetBrains.Annotations;
using LiteGuard;
using OpenTK.Graphics.ES20;

namespace Teraflop.Components
{
    public class Material : ResourceComponent, IAsset, IDependencies
    {
        private static bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private string _vertexShaderSource, _fragmentShaderSource;
        private int[] _shaderHandles;

        public Material([NotNull] string name, string shaderFilename) : base(name)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(shaderFilename), shaderFilename);
            ShaderFilename = shaderFilename;
            DepthStencilState = DefaultDepthStencilState;
            CullMode = CullFaceMode.Back;
            DepthClipEnabled = true;
            BlendState = DefaultBlendState;

            Resources.OnInitialize = () => {
                // Compile shaders
                try {
                    if (_isWindows)
                    {
                        ShaderProgramHandle = GL.CreateProgram();

                        var vs = GL.CreateShader(ShaderType.VertexShader);
                        GL.ShaderSource(vs, _vertexShaderSource);
                        var fs = GL.CreateShader(ShaderType.FragmentShader);
                        GL.ShaderSource(fs, _fragmentShaderSource);

                        _shaderHandles = new int[] { fs, vs };
                        foreach (var shaderHandle in _shaderHandles)
                        {
                            GL.CompileShader(shaderHandle);
                            string infoLog = GL.GetShaderInfoLog(shaderHandle);
                            if (!string.IsNullOrEmpty(infoLog))
                            {
                                System.Console.Error.WriteLine(infoLog);
                                throw new ShaderException(
                                    "Could not compile shader", new System.Exception(infoLog));
                            }
                            GL.AttachShader(ShaderProgramHandle, shaderHandle);
                        }

                        GL.LinkProgram(ShaderProgramHandle);
                    }
                    else
                    {
                        // TODO: Convert SPIR-V to OpenTk/OpenGL ES 2 compatible shader akin to Veldrid.SPIRV, i.e. SPIR-V Cross
                        throw new System.NotImplementedException("TODO: Convert SPIR-V to OpenTk/OpenGL ES 2 compatible shader");
                        // Shaders = factory.CreateFromSpirv(vsDescription, fsDescription);
                    }
                } finally {
                    _vertexShaderSource = null;
                    _fragmentShaderSource = null;
                }
            };
            Resources.OnDispose = () => {
                foreach (var shaderHandle in _shaderHandles)
                {
                    GL.DetachShader(ShaderProgramHandle, shaderHandle);
                    GL.DeleteShader(shaderHandle);
                    GL.DeleteProgram(ShaderProgramHandle);
                }
            };
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: DepthFunction.Lequal);
        // https://veldrid.dev/api/Veldrid.BlendAttachmentDescription.html#Veldrid_BlendAttachmentDescription_OverrideBlend
        public static readonly BlendingFactor DefaultBlendState = BlendingFactor.Src1Color;

        public string ShaderFilename { get; }
        public int ShaderProgramHandle { get; private set; }
        public DepthStencilStateDescription DepthStencilState { get; }
        public CullFaceMode CullMode {get; set; }
        public bool DepthClipEnabled { get; }
        public BlendingFactorSrc BlendStateSource { get; }
        public BlendingFactorDest BlendStateDestination { get; }
        public BlendingFactor BlendState { get; }

        public bool AreDependenciesSatisfied =>
            _vertexShaderSource != null && _fragmentShaderSource != null;

        public void LoadAssets(AssetDataLoader assetDataLoader)
        {
            var shaderFilenameWithoutExtension = ShaderFilename.Split('.').FirstOrDefault();
            var compiledShadersExist = new string[] {
                $"{shaderFilenameWithoutExtension}.vs.spirv",
                $"{shaderFilenameWithoutExtension}.fs.spirv"
            }.Aggregate(true, (shadersExist, filename) =>
                shadersExist && assetDataLoader.Exists(AssetType.Shader, filename)
            );

            if (!_isWindows && compiledShadersExist) {
                // TODO: Compile shaders from SPIR-V, i.e. https://github.com/mellinoe/veldrid-spirv
                throw new System.NotImplementedException("TODO: Compile shaders from SPIR-V");
                // _vertexShaderSource = ShaderImporter.Instance.Import(assetDataLoader.Load(
                //     AssetType.Shader,
                //     $"{shaderFilenameWithoutExtension}.vs.spirv"
                // ));
                // _fragmentShaderSource = ShaderImporter.Instance.Import(assetDataLoader.Load(
                //     AssetType.Shader,
                //     $"{shaderFilenameWithoutExtension}.fs.spirv"
                // ));
            }
            else if (assetDataLoader.Exists(AssetType.Shader, ShaderFilename))
            {
                var shaderSource = ShaderImporter.Instance.Import(
                    assetDataLoader.Load(AssetType.Shader, ShaderFilename)
                );
                var shaderSourceString = System.Text.Encoding.UTF8.GetString(shaderSource);
                _vertexShaderSource = _fragmentShaderSource = shaderSourceString;
            }
            else
            {
                throw new FileNotFoundException($"Shader not found: {ShaderFilename}");
            }
        }
    }

    public class ShaderException : System.Exception
    {
        public ShaderException() { }
        public ShaderException(string message) : base(message) { }
        public ShaderException(string message, System.Exception inner) : base(message, inner) { }
    }
}
