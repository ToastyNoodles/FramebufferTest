using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace DeferredShadingTest
{
    public class ProgramWindow : GameWindow
    {
        GBuffer gbuffer;
        Shader geometryShader;
        Shader fullscreenShader;
        Matrix4 projection, view, model;

        public ProgramWindow(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings())
        {
            Size = (width, height);
            Title = title;

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), 1280.0f / 720.0f, 0.01f, 1000.0f);
            view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -5.0f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.UnitY);
            model = Matrix4.Identity;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            gbuffer = new GBuffer(1280, 720);
            geometryShader = new Shader("Resources\\geometry.vert", "Resources\\geometry.frag");
            fullscreenShader = new Shader("Resources\\fullscreen.vert", "Resources\\fullscreen.frag");
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            //Render Scene to GBuffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, gbuffer.framebuffer);
            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2 };
            GL.DrawBuffers(attachments.Length, attachments);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            geometryShader.Bind();
            geometryShader.SetMat4("projection", projection);
            geometryShader.SetMat4("view", view);
            geometryShader.SetMat4("model", model);
            RenderQuad();

            //Render Fullscreen Quad with Gbuffer Texture
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            fullscreenShader.Bind();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, gbuffer.gPosition);
            fullscreenShader.SetInt("gPosition", 0);
            GL.Disable(EnableCap.DepthTest);
            RenderQuad();
            GL.Enable(EnableCap.DepthTest);

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private int quadVao = 0, quadVbo;
        private void RenderQuad()
        {
            if (quadVao == 0)
            {
                float[] quadVertices =
                {
                    -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
                    -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                     1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
                     1.0f, -1.0f, 0.0f, 1.0f, 0.0f
                };

                quadVao = GL.GenVertexArray();
                GL.BindVertexArray(quadVao);

                quadVbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, quadVbo);
                GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 20, quadVertices, BufferUsageHint.StaticDraw);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
            }

            GL.BindVertexArray(quadVao);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.BindVertexArray(0);
        }
    }
}
