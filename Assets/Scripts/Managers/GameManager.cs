using UnityEngine;

public class GameManager: MonoBehaviour
{
	[SerializeField] private bool gameover = false;
	[SerializeField] private bool moving = false;

	#region Getters
	public bool IsGameover() => gameover;
	public bool IsMoving() => moving;

	public void SetGameState(bool state) => gameover = state;
	public void SetMovingState(bool state) => moving = state;
	#endregion

	public static GameManager Instance;

	#region Unity Methods
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	#endregion
}
