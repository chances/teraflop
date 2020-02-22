using System;
using OpenTK.Graphics.OpenGL;

namespace Teraflop.WebAssembly
{
    public class WebGlContext
    {
        // TODO: WebGL2 function bindings (https://github.com/DefinitelyTyped/DefinitelyTyped/blob/master/types/webgl2/index.d.ts)

        // https://webassembly.org/docs/dynamic-linking/
        // https://peterhuene.github.io/wasmtime.net/articles/intro.html#implementing-the-net-code

        [Import("getFragDataLocation")]
        public int GetFragDataLocation(int program, string name) =>
            GL.GetFragDataLocation(program, name);

        [Import("uniform1ui")]
        void Uniform1ui(int location, int v0) =>
            GL.Uniform1(location, v0);
        [Import("uniform2ui")]
        void Uniform2ui(int location, int v0, int v1) =>
            GL.Uniform2(location, v0, v1);
        [Import("uniform3ui")]
        void Uniform3ui(int location, int v0, int v1, int v2) =>
            GL.Uniform3(location, v0, v1, v2);
        [Import("uniform4ui")]
        void Uniform4ui(int location, int v0, int v1, int v2, int v3) =>
            GL.Uniform4(location, v0, v1, v2, v3);

        [Import("uniform1fv")]
        void Uniform1fv(int location, double[] data, int offset = 0) =>
            GL.Uniform1(location, data[0 + offset]);
        [Import("uniform2fv")]
        void Uniform2fv(int location, double[] data, int offset = 0) =>
            GL.Uniform2(location, data[0 + offset], data[1 + offset]);
        [Import("uniform3fv")]
        void Uniform3fv(int location, double[] data, int offset = 0) =>
            GL.Uniform3(location, data[0 + offset], data[1 + offset], data[2 + offset]);
        [Import("uniform4fv")]
        void Uniform4fv(int location, double[] data, int offset = 0) =>
            GL.Uniform4(location, data[0 + offset], data[1 + offset], data[2 + offset], data[3 + offset]);

        [Import("uniform1iv")]
        void Uniform1iv(int location, int[] data, int offset = 0) =>
            GL.Uniform1(location, data[0 + offset]);
        [Import("uniform2iv")]
        void Uniform2iv(int location, int[] data, int offset = 0) =>
            GL.Uniform2(location, data[0 + offset], data[1 + offset]);
        [Import("uniform3iv")]
        void Uniform3iv(int location, int[] data, int offset = 0) =>
            GL.Uniform3(location, data[0 + offset], data[1 + offset], data[2 + offset]);
        [Import("uniform4iv")]
        void Uniform4iv(int location, int[] data, int offset = 0) =>
            GL.Uniform4(location, data[0 + offset], data[1 + offset], data[2 + offset], data[3 + offset]);

        [Import("uniform1uiv")]
        void Uniform1uiv(int location, uint[] data, int offset = 0) =>
            GL.Uniform1(location, data[0 + offset]);
        [Import("uniform2uiv")]
        void Uniform2uiv(int location, uint[] data, int offset = 0) =>
            GL.Uniform2(location, data[0 + offset], data[1 + offset]);
        [Import("uniform3uiv")]
        void Uniform3uiv(int location, uint[] data, int offset = 0) =>
            GL.Uniform3(location, data[0 + offset], data[1 + offset], data[2 + offset]);
        [Import("uniform4uiv")]
        void Uniform4uiv(int location, uint[] data, int offset = 0) =>
            GL.Uniform4(location, data[0 + offset], data[1 + offset], data[2 + offset], data[3 + offset]);

        /* Vertex attribs */
        [Import("vertexAttribI4i")]
        void VertexAttribI4i(int index, int x, int y, int z, int w) =>
            GL.VertexAttrib4(index, x, y, z, w);
        [Import("vertexAttribI4iv")]
        void VertexAttribI4iv(int index, int[] values) =>
            GL.VertexAttrib4(index, values[0], values[1], values[2], values[3]);
        [Import("vertexAttribI4ui")]
        void VertexAttribI4ui(int index, int x, int y, int z, int w) =>
            GL.VertexAttrib4(index, x, y, z, w);
        [Import("vertexAttribI4uiv")]
        void VertexAttribI4uiv(int index, uint[] values) =>
            GL.VertexAttrib4(index, values[0], values[1], values[2], values[3]);
        [Import("vertexAttribIPointer")]
        void VertexAttribIPointer(int index, int size, int type, int stride, int offset) =>
            throw new NotImplementedException();
    }

    public class ImportAttribute : Attribute
    {
        public ImportAttribute(string name)
        {
        }
    }
}
