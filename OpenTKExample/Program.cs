using OpenTK.Windowing.Desktop;

namespace DeferredShadingTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (ProgramWindow window = new ProgramWindow(1280, 720, "FramebufferTest"))
            {
                window.Run();
            }
        }
    }
}
