using System;
using System.Collections.Generic;
using UnityEngine;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Components
{
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
        public float bezierDivision = 0.0001f;
        public float diffusionConstant = 0.05f;
        public int diffusionCount = 100;
        public List<RidgeControlParam> controlParams= new List<RidgeControlParam>();
    }
}