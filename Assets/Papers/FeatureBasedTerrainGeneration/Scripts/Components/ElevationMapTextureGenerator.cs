using Papers.FeatureBasedTerrainGeneration.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
    [RequireComponent(typeof(Image))]
    public class ElevationMapTextureGenerator : MonoBehaviour
    {
        [SerializeField] private FeatureBasedTerrainParameter parameter;
        [SerializeField] private Texture2D texture;
        
        private void Start()
        {
            texture = new Texture2D(parameter.gridSize, parameter.gridSize, TextureFormat.RGBA32, false);
            
            var elevationMap = new float[parameter.gridSize, parameter.gridSize];
            SetHeights(ref elevationMap);
            ApplyColor(ref elevationMap);
            texture.Apply();
            
            var image = GetComponent<Image>();
            var sprite = Sprite.Create(texture, new Rect(0, 0, parameter.gridSize, parameter.gridSize), Vector2.zero);
            image.sprite = sprite;
        }

        private void OnDestroy()
        {
            DestroyImmediate(texture);
        }

        private void SetHeights(ref float[,] elevationMap)
        {
            for (var i = 0; i < parameter.gridSize; i++)
            {
                for (var j = 0; j < parameter.gridSize; j++)
                {
                    elevationMap[i, j] = parameter.initHeight;
                }
            }

            foreach (var param in parameter.controlParams)
            {
                var points = BezierCurveService.GeneratePoints(param.bezierPoints, parameter.bezierDivision);
                for (var i = 0; i < points.Count; i++)
                {
                    var x = Mathf.RoundToInt(points[i].x * (parameter.gridSize - 1));
                    var y = Mathf.RoundToInt(points[i].y * (parameter.gridSize - 1));
                    elevationMap[x, y] = 1f;
                }
            }

            DiffuseService.Diffuse(parameter.gridSize, parameter.diffusionCount, parameter.diffusionConstant, ref elevationMap);
        }
        
        private void ApplyColor(ref float[,] elevationMap)
        {
            for (var x = 0; x < parameter.gridSize; ++x)
            {
                for (var y = 0; y < parameter.gridSize; ++y)
                {
                    var height = elevationMap[x, y];
                    texture.SetPixel(x, y, new Color(height, height, height, 1.0f));
                }
            }
        }
    }
}