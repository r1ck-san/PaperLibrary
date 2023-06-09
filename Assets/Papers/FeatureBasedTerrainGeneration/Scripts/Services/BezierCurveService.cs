﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Papers.FeatureBasedTerrainGeneration.Scripts.Services
{
    public struct BezierPoint
    {
        public Vector2 Position;
        public Vector2 Normal;

        public BezierPoint(Vector2 position, Vector2 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
    
    public static class BezierCurveService
    {
        public static List<Vector2> GeneratePoints(List<Vector2> points, float div)
        {
            switch (points.Count)
            {
                case 2:
                    return GeneratePoints(points[0], points[1], div);
                case 3:
                    return GeneratePoints(points[0], points[1], points[2], div);
                case 4:
                    return GeneratePoints(points[0], points[1], points[2], points[3], div);
            }

            throw new NotImplementedException();
        }

        public static List<Vector2> GeneratePoints(Vector2 p0, Vector2 p1, float div)
        {
            var points = new List<Vector2>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetPoint(p0, p1, t));
            }

            return points;
        }

        public static List<Vector2> GeneratePoints(Vector2 p0, Vector2 p1, Vector2 p2, float div)
        {
            var points = new List<Vector2>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetPoint(p0, p1, p2, t));
            }

            return points;
        }

        public static List<Vector2> GeneratePoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float div)
        {
            var points = new List<Vector2>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetPoint(p0, p1, p2, p3, t));
            }

            return points;
        }
        
        public static List<BezierPoint> GenerateBezierPoints(List<Vector2> points, float div)
        {
            switch (points.Count)
            {
                case 2:
                    return GenerateBezierPoints(points[0], points[1], div);
                case 3:
                    return GenerateBezierPoints(points[0], points[1], points[2], div);
                case 4:
                    return GenerateBezierPoints(points[0], points[1], points[2], points[3], div);
            }

            throw new NotImplementedException();
        }

        public static List<BezierPoint> GenerateBezierPoints(Vector2 p0, Vector2 p1, float div)
        {
            var points = new List<BezierPoint>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetBezierPoint(p0, p1, t));
            }

            return points;
        }
        
        public static List<BezierPoint> GenerateBezierPoints(Vector2 p0, Vector2 p1, Vector2 p2, float div)
        {
            var points = new List<BezierPoint>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetBezierPoint(p0, p1, p2, t));
            }

            return points;
        }
        
        public static List<BezierPoint> GenerateBezierPoints(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float div)
        {
            var points = new List<BezierPoint>();
            for (var t = 0.0f; t < 1.0f; t += div)
            {
                points.Add(GetBezierPoint(p0, p1, p2, p3, t));
            }

            return points;
        }

        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, float t)
        {
            return Vector2.Lerp(p0, p1, t);
        }
        
        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);
            return Vector2.Lerp(a, b, t);
        }

        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);
            var c = Vector2.Lerp(p2, p3, t);

            var d = Vector2.Lerp(a, b, t);
            var e = Vector2.Lerp(b, c, t);
            
            return Vector2.Lerp(d, e, t);
        }
        
        public static Vector2 GetNormalVector(Vector2 p0, Vector2 p1, float t)
        {
            return Vector2.Perpendicular((p1 - p0).normalized);
        }
        
        public static Vector2 GetNormalVector(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);

            return Vector2.Perpendicular((b - a).normalized);
        }

        public static Vector2 GetNormalVector(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);
            var c = Vector2.Lerp(p2, p3, t);

            var d = Vector2.Lerp(a, b, t);
            var e = Vector2.Lerp(b, c, t);

            return Vector2.Perpendicular((e - d).normalized);
        }

        public static BezierPoint GetBezierPoint(Vector2 p0, Vector2 p1, float t)
        {
            return new BezierPoint(GetPoint(p0, p1, t), GetNormalVector(p0, p1, t));
        }
        
        public static BezierPoint GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);

            return new BezierPoint(Vector2.Lerp(a, b, t), Vector2.Perpendicular((b - a).normalized));
        }
        
        public static BezierPoint GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var a = Vector2.Lerp(p0, p1, t);
            var b = Vector2.Lerp(p1, p2, t);
            var c = Vector2.Lerp(p2, p3, t);

            var d = Vector2.Lerp(a, b, t);
            var e = Vector2.Lerp(b, c, t);

            return new BezierPoint(Vector2.Lerp(d, e, t), Vector2.Perpendicular((e - d).normalized));
        }
    }
}