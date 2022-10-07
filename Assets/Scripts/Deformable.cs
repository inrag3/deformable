using UnityEngine;

[RequireComponent(typeof(Mesh), typeof(MeshCollider), typeof(Rigidbody))]
public class Deformable : MonoBehaviour
{
	[SerializeField] [Min(2)] private float _minimumImpulse;
	[SerializeField] [Min(0.05f)] private float _malleability;
	[SerializeField] [Min(0)] private float _radius = 0.1f;
	private Mesh _mesh;
	private MeshCollider _collider;
	private Vector3[] _vertices;
	private Vector3[] _startVertices;
	
	private void Start()
	{
		_mesh = GetComponent<MeshFilter>().mesh;
		_collider = GetComponent<MeshCollider>();
		_startVertices = _mesh.vertices;
	}

	private void OnCollisionEnter(Collision collision)
	{
		var contact = collision.GetContact(0);
		var point = transform.InverseTransformPoint(contact.point);
		var normal = transform.InverseTransformDirection(contact.normal);
		var impulse = collision.impulse.magnitude;
		if (impulse < _minimumImpulse)
			return;
		_vertices = _mesh.vertices;
		for (var i = 0; i < _vertices.Length; i++)
		{
			var scale = Mathf.Clamp(_radius - (point - _vertices[i]).magnitude, 0, _radius);
			_vertices[i] += normal * impulse * scale * _malleability;
		}
		_mesh.vertices = _vertices;
		_collider.sharedMesh = _mesh;
		_mesh.RecalculateNormals();
		_mesh.RecalculateBounds();
	}

	private void OnApplicationQuit()
	{
		_mesh.vertices = _startVertices;
	}
}
 