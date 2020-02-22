using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Teraflop.Assets
{
    public class ShaderImporter : IAssetImporter<Dictionary<ShaderStages, string>>
    {
        public static ShaderImporter Instance = new ShaderImporter();

        public Dictionary<ShaderStages, string> Import(Stream assetData)
        {
            using (var stream = new MemoryStream())
            {
                // TODO: Use CopyToAsync when we need "Loading..." screen
                assetData.CopyTo(stream);
                var source = Encoding.UTF8.GetString(stream.ToArray());
                return GetDefinedStages(source)
                    .ToDictionary(stage => stage, stage => CompileShader(stage, source));
            }
        }

        private static string CompileShader(ShaderStages shaderStage, string shaderSource)
        {
            throw new System.NotImplementedException();
        }

        private static IEnumerable<ShaderStages> GetDefinedStages(string shaderSource)
        {
            var stages = new List<ShaderStages>();
            if (shaderSource.Contains("FragmentIn VS(VertexIn input)"))
            {
                stages.Add(ShaderStages.Vertex);
            }
            if (shaderSource.Contains("float4 FS(FragmentIn input)"))
            {
                stages.Add(ShaderStages.Fragment);
            }
            return stages;
        }

        private static string StageToEntryPoint(ShaderStages shaderStage)
        {
            switch (shaderStage)
            {
                case ShaderStages.Vertex:
                    return "VS";
                case ShaderStages.Fragment:
                    return "FS";
                default:
                    throw new System.NotSupportedException(
                        $"Given shader stage ({shaderStage}) not supported");
            }
        }
    }
}
