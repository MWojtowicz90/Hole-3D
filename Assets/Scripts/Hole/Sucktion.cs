using UnityEngine;
using System.Collections.Generic;

public class Sucktion : MonoBehaviour
{
	[SerializeField] private float suctionForce;

	private List<Rigidbody> objectList = new List<Rigidbody>();
	private Transform center;

	public static Sucktion Instance;

    private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start ()
	{
		center = transform;
		objectList.Clear();
		Collisions.OnObjectConusmed += HandleOnObject;
	}

    private void OnDestroy()
    {
		Collisions.OnObjectConusmed -= HandleOnObject;
	}

    private void FixedUpdate ()
	{
        if (!GameManager.Instance.IsGameover() && GameManager.Instance.IsMoving())
        {
            foreach (Rigidbody rb in objectList)
            {
                rb.AddForce((center.position - rb.position) * suctionForce * Time.fixedDeltaTime);
            }
        }
    }

	private void OnTriggerEnter (Collider other)
	{
		if (CheckTriggerConditions(other))
		{
			AddToList(other.attachedRigidbody);
		}
	}

	private void OnTriggerExit (Collider other)
	{
		if (CheckTriggerConditions(other)) 
		{
			RemoveFromList(other.attachedRigidbody);
		}
	}

	private bool CheckTriggerConditions(Collider other)
    {
		if (!GameManager.Instance.IsGameover() && (other.CompareTag("Obstacle") || other.CompareTag("Object")))
		{
			return true;
		}

		return false;
	}

	private void AddToList(Rigidbody rb)
	{
		objectList.Add(rb);
	}

	private void RemoveFromList(Rigidbody rb)
	{
		objectList.Remove(rb);
	}

	private void HandleOnObject(Collider other)
    {
		RemoveFromList(other.attachedRigidbody);
	}
}
