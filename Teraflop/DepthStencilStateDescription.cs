using OpenTK.Graphics.ES20;

namespace Teraflop
{
    public struct DepthStencilStateDescription
    {
        public bool DepthTestEnabled;
        public bool DepthWriteEnabled;
        public DepthFunction DepthComparison;

        public DepthStencilStateDescription(bool depthTestEnabled, bool depthWriteEnabled, DepthFunction comparisonKind)
        {
            DepthTestEnabled = depthTestEnabled;
            DepthWriteEnabled = depthWriteEnabled;
            DepthComparison = comparisonKind;
        }
    }
}
