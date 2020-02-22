using OpenTK.Graphics.OpenGL;
using Wasmtime;

namespace Teraflop.WebAssembly
{
    public class WebGlHost : IHost
    {
        public Instance Instance { get; set; }

        // TODO: WebGL2 function bindings (https://github.com/DefinitelyTyped/DefinitelyTyped/blob/master/types/webgl2/index.d.ts)

        // https://webassembly.org/docs/dynamic-linking/
        // https://peterhuene.github.io/wasmtime.net/articles/intro.html#implementing-the-net-code

        [Import("getFragDataLocation", Module="webgl")]
        public int GetFragDataLocation(int program, int namePtr, int nameLength)
        {
            var name = Instance.Externs.Memories[0].ReadString(namePtr, nameLength);
            return GL.GetFragDataLocation(program, name);
        }

        [Import("uniform1ui", Module="webgl")]
        void Uniform1ui(int location, int v0)
        {
            GL.Uniform1(location, v0);
        }
        [Import("uniform2ui", Module="webgl")]
        void Uniform2ui(int location, int v0, int v1)
        {
            GL.Uniform2(location, v0, v1);
        }
        [Import("uniform3ui", Module="webgl")]
        void Uniform3ui(int location, int v0, int v1, int v2)
        {
            GL.Uniform3(location, v0, v1, v2);
        }
        [Import("uniform4ui", Module="webgl")]
        void Uniform4ui(int location, int v0, int v1, int v2, int v3)
        {
            GL.Uniform4(location, v0, v1, v2, v3);
        }

        [Import("uniform1fv", Module="webgl")]
        void Uniform1fv(int location, int dataPtr, int offset)
        {
            var v0 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset);
            var v1 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double));
            var v2 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double) * 2);
            var v3 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double) * 3);
            GL.Uniform1(location, v0);
        }
        [Import("uniform2fv", Module="webgl")]
        void Uniform2fv(int location, int dataPtr, int offset)
        {
            var v0 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset);
            var v1 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double));
            GL.Uniform2(location, v0, v1);
        }
        [Import("uniform3fv", Module="webgl")]
        void Uniform3fv(int location, int dataPtr, int offset)
        {
            var v0 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset);
            var v1 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double));
            var v2 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double) * 2);
            GL.Uniform3(location, v0, v1, v2);
        }
        [Import("uniform4fv", Module="webgl")]
        void Uniform4fv(int location, int dataPtr, int offset)
        {
            var v0 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset);
            var v1 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double));
            var v2 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double) * 2);
            var v3 = Instance.Externs.Memories[0].ReadDouble(dataPtr + offset + sizeof(double) * 3);
            GL.Uniform4(location, v0, v1, v2, v3);
        }
    }
}
