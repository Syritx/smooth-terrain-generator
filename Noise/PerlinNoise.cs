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
}
