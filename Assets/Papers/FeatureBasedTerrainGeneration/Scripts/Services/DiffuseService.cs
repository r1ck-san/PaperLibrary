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
    }
}