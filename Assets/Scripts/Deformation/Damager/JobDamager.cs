using System.Linq;
using Deformation;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;


public class JobDamager : Damager
{
    public override void Damage(ContactPoint contact, float impulse, MeshVertices[] temporaryVertices)
    {
        var nativeVertices = new NativeArray<Vector3>[temporaryVertices.Length];
        var results = new NativeArray<Vector3>[temporaryVertices.Length];

        var transformMatrices = new Matrix4x4[temporaryVertices.Length];
        for (var i = 0; i < temporaryVertices.Length; i++)
        {
            transformMatrices[i] = _filters[i].transform.localToWorldMatrix;
            nativeVertices[i] = new NativeArray<Vector3>(temporaryVertices[i].Vertices, Allocator.Persistent);
        }
        for (var i = 0; i < temporaryVertices.Length; i++)
        {
            results[i] = new NativeArray<Vector3>(temporaryVertices[i].Vertices.Length, Allocator.Persistent);
        }

        var damageJobHandles = new NativeArray<JobHandle>(temporaryVertices.Length, Allocator.Temp);

        for (var i = 0; i < temporaryVertices.Length; i++)
        {
            var job = new DamageJob
            {
                TransformMatrix = transformMatrices[i],
                Vertices = nativeVertices[i],
                Impulse = impulse,
                Radius = _settings.Radius,
                Multiplier = _settings.Multiplier,
                Results = results[i],
                Contact = contact,
            };

            damageJobHandles[i] = job.Schedule();
        }

        JobHandle.CompleteAll(damageJobHandles);

        for (var i = 0; i < temporaryVertices.Length; i++)
        {
            temporaryVertices[i].Vertices = results[i].ToArray();
        }

        for (var i = 0; i < nativeVertices.Length; i++)
        {
            nativeVertices[i].Dispose();
            results[i].Dispose();
        }
        
        damageJobHandles.Dispose();
    }
}