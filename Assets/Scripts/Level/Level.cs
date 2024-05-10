using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	[Header("Effect")]
	[SerializeField] private ParticleSystem levelCompletedEffect;

	[Header("Transform")]
	[SerializeField] private Transform objectsParent;

	[Header("Counts")]
	[SerializeField] private int currentCount;
	[SerializeField] private int totalCount;

    public static Level Instance;

	#region Getters
	public int GetCurrentCount() => currentCount;
	public int GetTotalCount() => totalCount;
	public void SetCurrentCount(int count) => currentCount -=count;
	#endregion

	#region Unity Methods
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}


    private void Start ()
	{
		CountObjects();
		Collisions.OnLevelCompleted += HandleOnWin;
		Collisions.OnLevelFailed += HandleOnLose;
	}

    private void OnDestroy()
    {
		Collisions.OnLevelCompleted -= HandleOnWin;
		Collisions.OnLevelFailed -= HandleOnLose;

	}
    #endregion

	private void CountObjects()
	{
		totalCount = objectsParent.childCount;
		currentCount = totalCount;
	}

	private void HandleOnWin()
	{
		levelCompletedEffect.Play();
	}
	private void HandleOnLose()
    {
		RestartLevel();
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
