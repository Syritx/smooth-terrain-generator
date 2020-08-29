using System;
using OpenTK;

namespace PerlinNoiseGenerator.Noise
{
    public class Grid
    {
        public Vector3[] heightMaps;

        static int vertexCount = 16;
        static int tileSize = 5;

        Vector2[] gradients;

        public Grid(int x, int y, Vector2[] gradients)
        {
            heightMaps = new Vector3[vertexCount * vertexCount];

            this.gradients = gradients;
            createNoise();
        }


        void createNoise() {
            int id = 0;
            for (int x = 0; x < vertexCount; x++) {
                for (int y = 0; y < vertexCount; y++) {

                    // ---------------------------------------------------------- //
                    // BILINEAR INTERPOLATION //
                    // ---------------------------------------------------------- //

                    float yFrac = (float)y / vertexCount, xFrac = (float)x / vertexCount;
                    float[] products = gridGradient(new Vector2(xFrac, yFrac));

                    float AB = products[0] + xFrac * (products[1] - products[0]);
                    float CD = products[2] + xFrac * (products[3] - products[2]);

                    float value = AB + yFrac * (CD - AB);

                    // ---------------------------------------------------------- //
                    // CREATING COORDINATES //
                    // ---------------------------------------------------------- //

                    heightMaps[id] = new Vector3((x*tileSize) - (vertexCount*tileSize) / 2, value * 50, (y * tileSize) - (vertexCount*tileSize) / 2);
                    id++;
                }
            }
        }

        // ---------------------------------------------------------- //
        // DOT PRODUCT //
        // ---------------------------------------------------------- //

        float[] gridGradient(Vector2 vectorA) {
            float[] products = new float[gradients.Length];
            int id = 0;

            foreach (Vector2 gradient in gradients) {
                float xNew = vectorA.X * gradient.X;
                float yNew = vectorA.Y * gradient.Y;

                products[id] = xNew + yNew;
                id++;
            }

            return products;
        }

        public static float[] getInformation() {
            float[] info = {
                tileSize, vertexCount
            };
            return info;
        }
    }
}
