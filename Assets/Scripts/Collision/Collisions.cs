using UnityEngine;
using DG.Tweening;
using System;

public class Collisions : MonoBehaviour
{
	public static Action OnLevelCompleted;
	public static Action OnLevelFailed;
	public static Action<Collider> OnObjectConusmed;

	private void OnTriggerEnter (Collider other)
	{
		if (!GameManager.Instance.IsGameover()) 
		{
			if (other.CompareTag("Object")) 
			{
				HandleOnObject(other);

				HandleOnWin();
			}

			if (other.CompareTag("Obstacle")) 
			{
				HandleOnObstacle(other);	
			}
		}
	}	

	private void HandleOnWin()
    {
		if (Level.Instance.GetCurrentCount() <= 0)
		{
			OnLevelCompleted?.Invoke();
		}
	}

	private void HandleOnObject(Collider other)
    {
		Level.Instance.SetCurrentCount(1);

		OnObjectConusmed?.Invoke(other);
		Destroy(other.gameObject);
	}

	private void HandleOnObstacle(Collider other)
    {
		GameManager.Instance.SetGameState(true);
		Camera.main.transform.DOShakePosition(1f, .2f, 20, 90f).OnComplete(() => { OnLevelFailed?.Invoke(); });
		Destroy(other.gameObject);
	}
}
