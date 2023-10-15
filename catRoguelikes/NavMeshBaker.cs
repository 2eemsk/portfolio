using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
	[SerializeField]
	private GameObject _mapPrefab;

	private Vector3 _generatePos = new Vector3(50, 0, 50);

	public void Init()
	{
	}

	private void Awake()
	{
		GenerateNavmesh();
	}

	private void GenerateNavmesh()
	{
		GameObject obj = Instantiate(_mapPrefab, _generatePos, Quaternion.identity, transform);
		_generatePos += new Vector3(50, 0, 50);

		NavMeshSurface[] surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();

		foreach (var s in surfaces)
		{
			s.RemoveData();
			s.BuildNavMesh();
		}

	}
}
