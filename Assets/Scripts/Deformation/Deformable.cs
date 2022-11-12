using System;
using UnityEngine;

namespace Deformation
{
	[RequireComponent(typeof(Mesh), typeof(MeshCollider), typeof(Rigidbody))]
	public class Deformable : MonoBehaviour, IDeformable
	{
		
		private Vector3[] _vertices;
		private Vector3[] _startVertices;

		public MeshVertices InitialVertices { get; private set; }
		public MeshFilter Filter { get; private set; }
		public MeshCollider Collider { get; private set; }
		public event Action<Collision, IDeformable> Entered;


		private void Awake()
		{
			Collider = GetComponent<MeshCollider>();
			Filter = GetComponent<MeshFilter>();
			InitialVertices = new MeshVertices(Filter.mesh.vertices);
		}

		private void OnCollisionEnter(Collision collision)
		{
			Entered?.Invoke(collision, this);
		}
	}
}
 