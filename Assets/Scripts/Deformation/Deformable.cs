using System;
using UnityEngine;

namespace Deformation
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
	public class Deformable : MonoBehaviour, IDeformable
	{
		private MeshVertices _vertices;
		public MeshVertices InitialVertices { get; private set; }
		public MeshFilter Filter { get; private set; }
		public MeshCollider Collider { get; private set; }
		public event Action<Collision, IDeformable> Entered;


		private void Awake()
		{
			Filter = GetComponent<MeshFilter>();
			Collider = GetComponent<MeshCollider>();
			//TODO вынести возможно куда-то это
			// Filter.sharedMesh = Instantiate(Filter.mesh);
			// Collider.sharedMesh = Filter.sharedMesh;
			InitialVertices = new MeshVertices(Filter.mesh.vertices);
		}

		
		private void OnCollisionEnter(Collision collision)
		{
			Entered?.Invoke(collision, this);
		}
	}
}
 