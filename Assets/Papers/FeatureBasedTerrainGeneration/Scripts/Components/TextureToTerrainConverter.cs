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
        }
    }
}