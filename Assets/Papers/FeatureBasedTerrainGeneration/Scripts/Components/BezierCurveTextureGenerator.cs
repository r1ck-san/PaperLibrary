using Papers.FeatureBasedTerrainGeneration.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
    [RequireComponent(typeof(Image))]
    public class BezierCurveTextureGenerator : MonoBehaviour
    {
        [SerializeField] private FeatureBasedTerrainParameter parameter;
        [SerializeField] private Texture2D texture;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color lineColor = Color.blue;
        
        private void Start()
        {
            texture = new Texture2D(parameter.gridSize, parameter.gridSize, TextureFormat.RGBA32, false);

            var pixels = new Color[parameter.gridSize, parameter.gridSize];
            InitColor(ref pixels);
            SetBezierCurve(ref pixels);
            ApplyColor(ref pixels);
            texture.Apply();
            
            var image = GetComponent<Image>();
            var sprite = Sprite.Create(texture, new Rect(0, 0, parameter.gridSize, parameter.gridSize), Vector2.zero);
            image.sprite = sprite;
        }

        private void OnDestroy()
        {
            DestroyImmediate(texture);
        }

        private void InitColor(ref Color[,] pixels)
        {
            for (var x = 0; x < parameter.gridSize; ++x)
            {
                for (var y = 0; y < parameter.gridSize; ++y)
                {
                    pixels[x, y] = defaultColor;
                }
            }
        }

        private void SetBezierCurve(ref Color[,] pixels)
        {
            foreach (var param in parameter.controlParams)
            {
                var points = BezierCurveService.GeneratePoints(param.bezierPoints, parameter.bezierDivision);
                foreach (var point in points)
                {
                    var p = point * (parameter.gridSize - 1);
                    pixels[(int)p.x, (int)p.y] = lineColor;
                }
            }
        }

        private void ApplyColor(ref Color[,] pixels)
        {
            for (var x = 0; x < parameter.gridSize; ++x)
            {
                for (var y = 0; y < parameter.gridSize; ++y)
                {
                    texture.SetPixel(x, y, pixels[x, y]);
                }
            }
        }
    }
}