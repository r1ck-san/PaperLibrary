using UnityEngine;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Services
{
    public static class DiffuseService
    {
        public static void Diffuse(int gridSize, int diffusionCount, float diffusionConstant, ref float[,] elevationMap)
        {
            for (var i = 0; i < diffusionCount; ++i)
            {
                Diffuse(gridSize, diffusionConstant, ref elevationMap);
            }
        }
        
        public static void Diffuse(int gridSize, int diffusionCount, float diffusionConstant, ref Color[,] gradientMap)
        {
            for (var i = 0; i < diffusionCount; ++i)
            {
                Diffuse(gridSize, diffusionConstant, ref gradientMap);
            }
        }
        
        private static void Diffuse(int gridSize, float diffusionConstant, ref float[,] elevationMap)
        {
            var newMap = new float[gridSize, gridSize];

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    var e = elevationMap[i, j];
                    float ne = 0;
                    float p = 0;

                    if (i > 0)
                    {
                        ne += elevationMap[i - 1, j];
                        ++p;
                    }

                    if (i < gridSize - 1)
                    {
                        ne += elevationMap[i + 1, j];
                        ++p;
                    }

                    if (j > 0)
                    {
                        ne += elevationMap[i, j - 1];
                        ++p;
                    }

                    if (j < gridSize - 1)
                    {
                        ne += elevationMap[i, j + 1];
                        ++p;
                    }

                    ne /= p;
                    ne += (e - ne) * diffusionConstant;
                    newMap[i, j] = ne;
                }
            }

            elevationMap = newMap;
        }
        
        private static void Diffuse(int gridSize, float diffusionConstant, ref Color[,] gradientMap)
        {
            var newMap = new Color[gridSize, gridSize];

            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    var e = gradientMap[i, j];
                    var ne = new Color(0, 0, 0);
                    float p = 0;

                    if (i > 0)
                    {
                        ne += gradientMap[i - 1, j];
                        ++p;
                    }

                    if (i < gridSize - 1)
                    {
                        ne += gradientMap[i + 1, j];
                        ++p;
                    }

                    if (j > 0)
                    {
                        ne += gradientMap[i, j - 1];
                        ++p;
                    }

                    if (j < gridSize - 1)
                    {
                        ne += gradientMap[i, j + 1];
                        ++p;
                    }

                    ne /= p;
                    ne += (e - ne) * diffusionConstant;
                    newMap[i, j] = ne;
                }
            }

            gradientMap = newMap;
        }
    }
}