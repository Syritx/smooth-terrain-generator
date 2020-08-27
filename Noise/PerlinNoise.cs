using System;
using OpenTK;


namespace PerlinNoiseGenerator.Noise
{
    public class PerlinNoise
    {
        float[] information = Grid.getInformation();
        public Vector3[] heightMaps;

        Vector2[] gradients;
        Grid grid;

        public PerlinNoise(int tileCount)
        {
            gradients = new Vector2[(int)Math.Pow(tileCount + 1, 2)];

            int gradientID = 0;
            int offsetReach = 2;

            // ---------------------------------------------------------- //
            // CREATING GRADIENTS //
            // ---------------------------------------------------------- //

            for (int x = 0; x < tileCount + 1; x++) {
                for (int y = 0; y < tileCount + 1; y++) {

                    Random rx = new Random(),
                           ry = new Random();

                    int gx = rx.Next(-offsetReach, offsetReach + 1);
                    int gy = ry.Next(-offsetReach, offsetReach + 1);

                    gradients[gradientID] = new Vector2(gx, gy);
                    gradientID++;
                }
            }

            int gradientCollection = 0;

            // ---------------------------------------------------------- //
            // CREATING GRID //
            // ---------------------------------------------------------- //

            for (int x = 0; x < tileCount + 1; x++) {
                for (int y = 0; y < tileCount + 1; y++) {

                    Vector2[] gradient = {
                        new Vector2(0,0),
                        new Vector2(0,0),
                        new Vector2(0,0),
                        new Vector2(0,0),
                    };

                    gradient[0] = gradients[gradientCollection];
                    try {
                        gradient[1] = gradients[gradientCollection + 1];
                        gradient[2] = gradients[gradientCollection + tileCount];
                        gradient[3] = gradients[gradientCollection + tileCount + 1];
                    }
                    catch (Exception e) { }

                    gradientCollection++;
                    grid = new Grid(x, y, gradient);
                }
            }

            heightMaps = grid.heightMaps;
        }
    }

    class Grid
    {
        public Vector3[] heightMaps;

        static int tileCount = 16;
        static int tileSize = 5;

        int xWorld, yWorld;

        Vector2[] gradients;

        public Grid(int x, int y, Vector2[] gradients)
        {
            heightMaps = new Vector3[tileCount * tileCount];

            xWorld = x * tileCount;
            yWorld = y * tileCount;

            this.gradients = gradients;
            createNoise();
        }


        void createNoise()
        {
            int id = 0;
            for (int x = 0; x < tileCount; x++) {
                for (int y = 0; y < tileCount; y++) {

                    // ---------------------------------------------------------- //
                    // BILINEAR INTERPOLATION //
                    // ---------------------------------------------------------- //

                    float yFrac = (float)y/tileCount, xFrac = (float)x/tileCount;
                    float[] products = gridGradient(new Vector2(xFrac, yFrac));

                    float AB = products[0] + xFrac * (products[1] - products[0]);
                    float CD = products[2] + xFrac * (products[3] - products[2]);

                    float value = AB + yFrac * (CD - AB);

                    // ---------------------------------------------------------- //
                    // CREATING COORDINATES //
                    // ---------------------------------------------------------- //

                    heightMaps[id] = new Vector3((x*tileSize)-((tileCount/2)*tileSize), value * 50, (y*tileSize)- ((tileCount / 2) * tileSize));
                    id++;
                }
            }
        }

        // ---------------------------------------------------------- //
        // DOT PRODUCT //
        // ---------------------------------------------------------- //

        float[] gridGradient(Vector2 vectorA)
        {
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

        public static float[] getInformation()
        {
            float[] info = {
                tileSize, tileCount
            };
            return info;
        }
    }
}
