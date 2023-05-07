using UnityEngine;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
    public class TextureToTerrainConverter : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;
        [SerializeField] private Texture2D texture;

        private void Start()
        {
            var data = terrain.terrainData;
            var heightMap = new float[texture.width, texture.height];
            for (var i = 0; i < texture.width; i++)
            {
                for (var j = 0; j < texture.height; ++j)
                {
                    heightMap[i, j] = texture.GetPixel(i, j).r;
                }
            }

            data.SetHeights(0, 0, heightMap);
            //
            // var alphaMap = new float[data.alphamapResolution, data.alphamapResolution, data.alphamapLayers];
            // for (var z = 0; z < data.alphamapResolution; ++z)
            // {
            //     for (var x = 0; x < data.alphamapResolution; ++x)
            //     {
            //         alphaMap[x, z, 0] = 1.0f;
            //     }
            // }
            // data.SetAlphamaps(0,0,alphaMap);
        }
    }
}