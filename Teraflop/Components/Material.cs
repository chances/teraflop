using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Teraflop.Assets;
using Teraflop.ECS;
using JetBrains.Annotations;
using LiteGuard;
using Veldrid;
using Veldrid.SPIRV;
using System.Threading.Tasks;

namespace Teraflop.Components
{
    public class Material : Resource, IAssetSink, IDependencies
    {
        private static bool _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private byte[] _vertexShaderSource, _fragmentShaderSource;

        public Material([NotNull] string name, string shaderFilename) : base(name)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(shaderFilename), shaderFilename);
            ShaderFilename = shaderFilename;
            DepthStencilState = DefaultDepthStencilState;
            CullMode = FaceCullMode.Back;
            FillMode = PolygonFillMode.Solid;
            DepthClipEnabled = true;
            BlendState = DefaultBlendState;

            Resources.OnInitialize += (_, e) => {
                var factory = e.ResourceFactory;
                // Compile shaders
                try {
                    var vsDescription = new ShaderDescription(ShaderStages.Vertex, _vertexShaderSource, "VS");
                    var fsDescription = new ShaderDescription(ShaderStages.Fragment, _fragmentShaderSource, "FS");

                    if (_isWindows && !(e.GraphicsDevice.BackendType == GraphicsBackend.Vulkan))
                    {
                        var vs = factory.CreateShader(vsDescription);
                        var fs = factory.CreateShader(fsDescription);
                        Shaders = new Shader[] { fs, vs };
                    }
                    else
                    {
                        Shaders = factory.CreateFromSpirv(vsDescription, fsDescription);
                    }
                } finally {
                    _vertexShaderSource = null;
                    _fragmentShaderSource = null;
                }
            };
            Resources.OnDispose += (_, __) => {
                foreach (var shader in Shaders)
                {
                    shader.Dispose();
                }
            };
        }

        public static readonly DepthStencilStateDescription DefaultDepthStencilState = new DepthStencilStateDescription(
            depthTestEnabled: true, depthWriteEnabled: true, comparisonKind: ComparisonKind.LessEqual);
        public static readonly BlendStateDescription DefaultBlendState = BlendStateDescription.SingleOverrideBlend;

        public string ShaderFilename { get; }
        public Shader[] Shaders { get; private set; }
        public DepthStencilStateDescription DepthStencilState { get; }
        public FaceCullMode CullMode {get; set; }
        public PolygonFillMode FillMode { get; set; }
        public bool DepthClipEnabled { get; }
        public BlendStateDescription BlendState { get; }

        public bool AreDependenciesSatisfied =>
            _vertexShaderSource != null && _fragmentShaderSource != null;

        public async Task LoadAssets(IAssetSource assetSource)
        {
            var shaderFileName = assetSource.GetAbsolutePath(ShaderFilename);
            var shaderFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(shaderFileName);
            var compiledShadersExist = new string[] {
                $"{shaderFilenameWithoutExtension}.vs.spirv",
                $"{shaderFilenameWithoutExtension}.fs.spirv"
            }.Aggregate(true, (shadersExist, fileName) =>
                shadersExist && assetSource.Exists(assetSource.GetAbsolutePath(fileName))
            );

            var isVulkan = Game.Services.Resolve<Services.GraphicsDeviceFeatures>().BackendType == GraphicsBackend.Vulkan;

            if ((!_isWindows || isVulkan) && compiledShadersExist) {
                // TODO: Wait for https://github.com/mellinoe/veldrid-spirv/pull/2 and remove this? Or keep em for mobile?
                _vertexShaderSource = await ShaderImporter.Instance.Import(assetSource.Load(
                    AssetType.Shader,
                    assetSource.GetAbsolutePath($"{shaderFilenameWithoutExtension}.vs.spirv")
                ));
                _fragmentShaderSource = await ShaderImporter.Instance.Import(assetSource.Load(
                    AssetType.Shader,
                    assetSource.GetAbsolutePath($"{shaderFilenameWithoutExtension}.fs.spirv")
                ));
            }
            else if (assetSource.Exists(shaderFileName))
            {
                var shaderSource = await ShaderImporter.Instance.Import(
                    assetSource.Load(AssetType.Shader, shaderFileName)
                );
                _vertexShaderSource = _fragmentShaderSource = shaderSource;
            }
            else
            {
                throw new FileNotFoundException($"Shader not found: {ShaderFilename}");
            }
        }
    }
}
