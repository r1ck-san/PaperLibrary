using Papers.FeatureBasedTerrainGeneration.Scripts.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
#if UNITY_EDITOR

    [CustomEditor(typeof(GradientMapGenerator))]
    public class GradientMapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("テクスチャを保存"))
            {
                var component = target as GradientMapGenerator;
                if (component != null)
                {
                    component.GenerateAndSave();
                }
            }
        }
    }

#endif
    
    [RequireComponent(typeof(Image))]
    public class GradientMapGenerator : MonoBehaviour
    {
        [SerializeField] private FeatureBasedTerrainParameter parameter;
        private Texture2D _texture;
        
        private void Start()
        {
            Generate();
            
            var image = GetComponent<Image>();
            var sprite = Sprite.Create(_texture, new Rect(0, 0, parameter.gridSize, parameter.gridSize), Vector2.zero);
            image.sprite = sprite;
        }

        private void OnDestroy()
        {
            DestroyImmediate(_texture);
            _texture = null;
        }

        public void GenerateAndSave()
        {
            Generate();
            System.IO.File.WriteAllBytes($"{Application.dataPath}/Papers/FeatureBasedTerrainGeneration/Textures/gradient.png",_texture.EncodeToPNG());
            DestroyImmediate(_texture);
            
            AssetDatabase.Refresh();
        }

        private void Generate()
        {
            _texture = new Texture2D(parameter.gridSize, parameter.gridSize, TextureFormat.RGBA32, false);
            SetHeights();
            _texture.Apply();
        }

        private void SetHeights()
        {
            var elevationMap = new float[parameter.gridSize, parameter.gridSize];
            
            for (var i = 0; i < parameter.gridSize; i++)
            {
                for (var j = 0; j < parameter.gridSize; j++)
                {
                    elevationMap[i, j] = parameter.initHeight;
                }
            }

            foreach (var param in parameter.controlParams)
            {
                var points = BezierCurveService.GenerateBezierPoints(param.bezierPoints, parameter.division);
                for (var i = 0; i < points.Count; i++)
                {
                    var position = points[i].Position;
                    var normal = points[i].Normal;
                    var h = parameter.initHeight + param.elevationConstraints.x;

                    var x = Mathf.RoundToInt(position.x * (parameter.gridSize - 1));
                    var y = Mathf.RoundToInt(position.y * (parameter.gridSize - 1));
                    elevationMap[x, y] = h;
                    
                    // calc r
                    var r = param.elevationConstraints.y;
                    for (var j = 0.0f; j < r; j += parameter.division)
                    {
                        var dn1 = normal * j;
                        var dx1 = Mathf.RoundToInt((position.x + dn1.x) * (parameter.gridSize - 1));
                        var dy1 = Mathf.RoundToInt((position.y + dn1.y) * (parameter.gridSize - 1));
                        if (0 <= dx1 && dx1 < parameter.gridSize && 0 <= dy1 && dy1 < parameter.gridSize)
                        {
                            elevationMap[dx1, dy1] = h;
                        }
                        
                        var dn2 = normal * -j;
                        var dx2 = Mathf.RoundToInt((position.x + dn2.x) * (parameter.gridSize - 1));
                        var dy2 = Mathf.RoundToInt((position.y + dn2.y) * (parameter.gridSize - 1));
                        if (0 <= dx2 && dx2 < parameter.gridSize && 0 <= dy2 && dy2 < parameter.gridSize)
                        {
                            elevationMap[dx2, dy2] = h;
                        }
                    }
                    
                    // calc a
                    var a = param.angleConstraints.x;
                    var theta = param.angleConstraints.z * Mathf.Deg2Rad;
                    for (var j = 0.0f; j < a; j += parameter.division)
                    {
                        var dn = -normal * (r + j * Mathf.Sin(theta));
                        var dx = Mathf.RoundToInt((position.x + dn.x) * (parameter.gridSize - 1));
                        var dy = Mathf.RoundToInt((position.y + dn.y) * (parameter.gridSize - 1));
                        if (0 <= dx && dx < parameter.gridSize && 0 <= dy && dy < parameter.gridSize)
                        {
                            elevationMap[dx, dy] = h - j * Mathf.Cos(theta);
                        }
                    }

                    // calc b
                    var b = param.angleConstraints.y;
                    var phi = param.angleConstraints.w * Mathf.Deg2Rad;
                    for (var j = 0.0f; j < b; j += parameter.division)
                    {
                        var dn = normal * (r + j * Mathf.Sin(phi));
                        var dx = Mathf.RoundToInt((position.x + dn.x) * (parameter.gridSize - 1));
                        var dy = Mathf.RoundToInt((position.y + dn.y) * (parameter.gridSize - 1));
                        if (0 <= dx && dx < parameter.gridSize && 0 <= dy && dy < parameter.gridSize)
                        {
                            elevationMap[dx, dy] = h - j * Mathf.Cos(phi);
                        }
                    }
                }
            }
            
            // generate gradient map
            var gradientMap = new Color[parameter.gridSize, parameter.gridSize];
            for (var i = 0; i < parameter.gridSize; ++i)
            {
                for (var j = 0; j < parameter.gridSize; ++j)
                {
                    var color = new Color(0.5f, 0.5f, 1);
                    // slop x
                    if (0 < i && i < parameter.gridSize - 1)
                    {
                        color.r = elevationMap[i + 1, j] - elevationMap[i - 1, j] + 0.5f;
                    }

                    // slop y
                    if (0 < j && j < parameter.gridSize - 1)
                    {
                        color.g = elevationMap[i, j + 1] - elevationMap[i, j - 1] + 0.5f;
                    }

                    gradientMap[i, j] = color;
                }
            }
            DiffuseService.Diffuse(parameter.gridSize, parameter.diffusionCount, parameter.diffusionConstant, ref gradientMap);
            
            for (var i = 0; i < parameter.gridSize; ++i)
            {
                for (var j = 0; j < parameter.gridSize; ++j)
                {
                    _texture.SetPixel(i, j, gradientMap[i, j]);
                }
            }
        }
    }
}