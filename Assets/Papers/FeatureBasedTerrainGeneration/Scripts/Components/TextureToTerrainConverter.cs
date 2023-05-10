using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
#if UNITY_EDITOR

    [CustomEditor(typeof(TextureToTerrainConverter))]
    public class TextureToTerrainConverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Terrain変換"))
            {
                var component = target as TextureToTerrainConverter;
                if (component != null)
                {
                    component.Generate();
                }
            }
        }
    }

#endif
    public class TextureToTerrainConverter : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;
        [SerializeField] private Texture2D texture;

        private void Start()
        {
            Generate();
        }

        public void Generate()
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