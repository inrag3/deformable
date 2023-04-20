using System;
using Deformation;
using Detector;
using UnityEngine;

namespace Deformation
{
    [RequireComponent(typeof(MeshFilter), typeof(Detector.Detector))]
    [DisallowMultipleComponent]
    public class Deformable : MonoBehaviour, IDeformable
    {
        private IDetector<Collision> _detector;
        private MeshVertices _vertices;
        public MeshVertices InitialVertices { get; private set; }
        public MeshFilter Filter { get; private set; }

        public event Action<Collision> Entered;

        private void Awake()
        {
            _detector = GetComponent<IDetector<Collision>>();
            Filter = GetComponent<MeshFilter>();
            Filter.sharedMesh = Instantiate(Filter.mesh);
            InitialVertices = new MeshVertices(Filter.mesh.vertices);
        }

        private void OnEnable()
        {
            _detector.Detected += OnDetected;
        }

        private void OnDetected(Collision collision) =>
            Entered?.Invoke(collision);

        private void OnDisable()
        {
            _detector.Detected -= OnDetected;
        }
    }
}

