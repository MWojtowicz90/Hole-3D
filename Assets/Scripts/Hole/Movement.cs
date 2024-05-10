using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Movement : MonoBehaviour
{
	[Header ("Mesh")]
	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private MeshCollider meshCollider;

	[Header ("Vertices")]
	[SerializeField] private Vector2 moveLimits;
	[SerializeField] private float radius;
	[SerializeField] private Transform center;

	[Header("Settings")]
	[SerializeField] float moveSpeed;

	private Mesh mesh;
	private List<int> verticieList;
	private List<Vector3> offsetList;
	private int verticeCount;

	private Vector3 mouse, targetPos;

	private void Start ()
	{
		Initialize();
	}

	private void Initialize()
    {
		GameManager.Instance.SetMovingState(false);
		GameManager.Instance.SetGameState(false);

		verticieList = new List<int>();
		offsetList = new List<Vector3>();

		mesh = meshFilter.mesh;
		FindVertices();
	}

	private void Update ()
	{
#if UNITY_EDITOR
		GameManager.Instance.SetMovingState(Input.GetMouseButton(0));
#else
		GameManager.Instance.SetMovingState(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved);
#endif

		if (!GameManager.Instance.IsGameover() && GameManager.Instance.IsMoving())
		{
			Move();
			UpdateVertices();
		}
	}

	private void Move()
	{
		float x = Input.GetAxis ("Mouse X");
		float y = Input.GetAxis ("Mouse Y");

		mouse = Vector3.Lerp 
			(
			center.position,
			center.position + new Vector3 (x, 0f, y), 
			moveSpeed * Time.deltaTime
			);

		targetPos = new Vector3(
			Mathf.Clamp (mouse.x, -moveLimits.x, moveLimits.x),
			mouse.y,
			Mathf.Clamp (mouse.z, -moveLimits.y, moveLimits.y)
			);

		center.position = targetPos;
	}

	private void UpdateVertices()
	{
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < verticeCount; i++) 
		{
			vertices [verticieList[i]] = center.position + offsetList[i];
		}

		mesh.vertices = vertices;
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}

	private void FindVertices()
	{
		for (int i = 0; i < mesh.vertices.Length; i++) 
		{
			float distance = Vector3.Distance(center.position, mesh.vertices[i]);

			if (distance < radius) 
			{
				verticieList.Add (i);
				offsetList.Add(mesh.vertices [i] - center.position);
			}
		}

		verticeCount = verticieList.Count;
	}
}
