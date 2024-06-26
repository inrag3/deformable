﻿using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public struct DamageJob : IJob
{
    [ReadOnly] public NativeArray<Vector3> Vertices;
    [WriteOnly] public NativeArray<Vector3> Results;
    public float Impulse;
    public float Radius;
    public float Multiplier;
    public Matrix4x4 TransformMatrix;
    public ContactPoint Contact;
    
    [BurstCompile]
    public void Execute()
    {
        var point = TransformMatrix.inverse.MultiplyPoint(Contact.point);
        for (var j = 0; j < Vertices.Length; j++)
        {
            Results[j] = Vertices[j];
            var distance = Vector3.Distance(point, Vertices[j]);

            if (!(distance <= Radius))
                continue;

            var pulse = Impulse;
            pulse -= pulse * Mathf.Clamp01(distance / Radius);
            
            var position = TransformMatrix.MultiplyPoint(Vertices[j]);;
 
            position += (Contact.normal * Multiplier * pulse) / 10f;

            Results[j] = TransformMatrix.inverse.MultiplyPoint(position);
        }
    }
}