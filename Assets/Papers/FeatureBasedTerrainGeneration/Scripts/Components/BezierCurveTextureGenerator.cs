using System.Collections.Generic;
using Papers.FeatureBasedTerrainGeneration.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
#if UNITY_EDITOR

    [CustomEditor(typeof(BezierCurveTextureGenerator))]
    public class BezierCurveTextureGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("テクスチャを保存"))
            {
                var component = target as BezierCurveTextureGenerator;
                if (component != null)
                {
                    component.GenerateAndSave();
                }
            }
        }
    }

#endif
    
    [RequireComponent(typeof(Image))]
    public class BezierCurveTextureGenerator : MonoBehaviour
    {
        [SerializeField] private FeatureBasedTerrainParameter parameter;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color lineColor = Color.blue;
        
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
            
            System.IO.File.WriteAllBytes($"{Application.dataPath}/Papers/FeatureBasedTerrainGeneration/Textures/bezier.png",_texture.EncodeToPNG());
            DestroyImmediate(_texture);
            
            AssetDatabase.Refresh();
        }

        private void Generate()
        {
            _texture = new Texture2D(parameter.gridSize, parameter.gridSize, TextureFormat.RGBA32, false);

            var pixels = new List<Color>(parameter.gridSize * parameter.gridSize);
            // init
            for (var i = 0; i < parameter.gridSize * parameter.gridSize; ++i)
            {
                pixels.Add(defaultColor);
            }
            
            // calc bezier points
            foreach (var param in parameter.controlParams)
            {
                foreach (var point in BezierCurveService.GeneratePoints(param.bezierPoints, parameter.division))
                {
                    var p = point * (parameter.gridSize - 1);
                    pixels[(int)p.y * parameter.gridSize + (int)p.x] = lineColor;
                }
            }
            
            // set
            _texture.SetPixels(pixels.ToArray());
            _texture.Apply();
        }
    }
}