using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	[Header("Progress UI")]
	[SerializeField] private TextMeshProUGUI nextLevelText;
	[SerializeField] private TextMeshProUGUI currentLevelText;
	[SerializeField] private Image progressbarImage;

	[Header("Texts")]
	[SerializeField] private TMP_Text levelCompletedText;

	[Header("Panel")]
	[SerializeField] private Image fadePanel;

	public static UIManager Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}
	private void Start ()
	{
		Initialize();

		Collisions.OnObjectConusmed += HandleOnConsumed;
		Collisions.OnLevelCompleted += HandleOnCompleted;
	}
    private void OnDestroy()
    {
		Collisions.OnObjectConusmed -= HandleOnConsumed;
		Collisions.OnLevelCompleted -= HandleOnCompleted;
	}

    private void Initialize()
    {
		FadeIn();
		progressbarImage.fillAmount = 0f;
		UpdateLevelText();
	}

    private void HandleOnConsumed(Collider other)
    {
		UpdateLevelProgress();
	}
	private void HandleOnCompleted()
    {
		levelCompletedText.DOFade(1f, .6f).From(0f);
	}

	private void UpdateLevelText()
	{
		int level = SceneManager.GetActiveScene ().buildIndex;
		currentLevelText.SetText($"{level}");
		nextLevelText.SetText($"{level + 1}");
	}

	private void UpdateLevelProgress()
	{
		float val = 1f - ((float)Level.Instance.GetCurrentCount() / Level.Instance.GetTotalCount());
		progressbarImage.DOFillAmount(val, 0.35f);
	}

	private void FadeIn()
	{
		fadePanel.DOFade(0f, 1.3f).From(1f);
	}
}
