using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using PerlinNoiseGenerator.Noise;

namespace PerlinNoiseGenerator
{
    public class Game : GameWindow
    {
        Vector3[] heightMaps;
        PerlinNoise perlinNoise;

        double yAverage = 0, yOffset = 10;

        public Game(int width, int height) : base(width,height,GraphicsMode.Default,"Perlin Noise")
        {
            perlinNoise = new PerlinNoise(1);
            heightMaps = perlinNoise.heightMaps;

            foreach (Vector3 c in heightMaps) {
                yAverage += c.Y;
            }

            yAverage /= heightMaps.Length;
            yAverage += yOffset;

            Console.WriteLine(yAverage);

            start();
        }


        void start()
        {
            GL.Translate(0, -yAverage, 0);
            RenderFrame += render;
            Resize += resize;
            Load += load;

            Run(60);
        }

        void render(object sender, EventArgs e)
        {
            GL.Rotate(1, 0, 1, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < heightMaps.Length; i++)
            {
                GL.Begin(PrimitiveType.Quads); // CHANGE THIS TO QUAD

                GL.Color3((double)114 / 255, (double)179 / 255, (double)29 / 255);
                GL.Vertex3(heightMaps[i]);

                try {
                    //GL.Color3(0, 0, 0);
                    GL.Vertex3(heightMaps[i + 1]);

                    int x1 = (int)heightMaps[i + 16 + 1].X, x2 = (int)heightMaps[i + 16].X;
                    if (x1 == x2) {
                        //GL.Color3(0.5, 0.5, 0.5);
                        GL.Vertex3(heightMaps[i + 16 + 1]);
                        GL.Vertex3(heightMaps[i + 16]);
                    }
                }
                catch (Exception ex) { }

                GL.End();
            }

            GL.Enable(EnableCap.Fog);

            // Fog
            float[] colors = { 230, 230, 230 };
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, colors);

            GL.Fog(FogParameter.FogStart, (float)1000 / 60.0f);
            GL.Fog(FogParameter.FogEnd, 40.0f);

            SwapBuffers();
        }

        void resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspectiveMatrix =
                Matrix4.CreatePerspectiveFieldOfView(1, Width / Height, 1.0f, 200.0f);

            GL.LoadMatrix(ref perspectiveMatrix);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.End();
        }

        void load(object sender, EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}
