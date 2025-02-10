using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace DeferredShadingTest
{
    public class Shader
    {
        private int program;

        public Shader(string vertexFilepath, string fragmentFilepath)
        {
            string vertexShaderSource = File.ReadAllText(vertexFilepath);
            string fragmentShaderSource = File.ReadAllText(fragmentFilepath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexSuccess);
            if (vertexSuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentSuccess);
            if (fragmentSuccess == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }

            program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int programSuccess);
            if (programSuccess == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            Console.WriteLine($"Loaded Shader {vertexFilepath}");
            Console.WriteLine($"Loaded Shader {fragmentFilepath}");
        }

        public void Bind()
        {
            GL.UseProgram(program);
        }

        public void SetInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(program, name), value);
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(program, name), value);
        }

        public void SetVec3(string name, Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(program, name), value);
        }

        public void SetMat4(string name, Matrix4 value)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(program, name), false, ref value);
        }
    }
}
