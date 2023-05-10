using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
#if UNITY_EDITOR

    [CustomEditor(typeof(FeatureBasedTerrainParameter))]
    public class FeatureBasedTerrainParameterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("パラメータ自動生成"))
            {
                var component = target as FeatureBasedTerrainParameter;
                if (component != null)
                {
                    component.GenerateRandomParam();
                }
            }
        }
    }

#endif
    
    [Serializable]
    public struct RidgeControlParam
    {
        public List<Vector2> bezierPoints;
        public Vector2 elevationConstraints;
        public Vector4 angleConstraints;
        public Vector2 noiseConstraints;
    }
    
    public class FeatureBasedTerrainParameter : MonoBehaviour
    {
        public int gridSize = 512;
        public float initHeight = 0.3f;
        public float division = 0.0001f;
        public float diffusionConstant = 0.05f;
        public int diffusionCount = 100;
        public List<RidgeControlParam> controlParams= new List<RidgeControlParam>();
        public int seed = 0;
        public int controlParamCount = 10;

        public void GenerateRandomParam()
        {
            controlParams.Clear();

            var random = new Random(seed);

            for (var i = 0; i < controlParamCount; ++i)
            {
                var param = new RidgeControlParam
                {
                    bezierPoints = new List<Vector2>(4)
                };
                for (var j = 0; j < 4; ++j)
                {
                    var x = random.NextDouble();
                    var y = random.NextDouble();
                    param.bezierPoints.Add(new Vector2((float)x, (float)y));
                }

                var h = (float)random.NextDouble() * (-0.7f - 0.3f) + 0.3f;
                param.elevationConstraints = new Vector2(h, 0.001f);
                var a = (float)random.NextDouble() * (0.5f - 0.1f) + 0.1f;
                var b = (float)random.NextDouble() * (0.5f - 0.1f) + 0.1f;
                param.angleConstraints = new Vector4(a, b, random.Next(0, 180), random.Next(0, 180));
                
                controlParams.Add(param);
            }
        }
    }
}