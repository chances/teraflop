## Caveats

Install shader compilation tools, [shaderc](https://github.com/google/shaderc#downloads).

Running examples on Veldrid in Ubuntu:

`sudo apt install libsdl2-dev`

Running examples on OpenTk in Ubuntu:

`sudo apt install libgbm-dev libegl1-mesa-dev libxi-dev`

## Debugging Shader Compilation

Print human-readable SPIR-V:

- `glslangValidator -S vert -e VS -D -H examples/triangle/Content/Shaders/flat.hlsl`
- `glslangValidator -S frag -e FS -D -H examples/triangle/Content/Shaders/flat.hlsl`
