using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Audio;

public class TitleMenuController : MonoBehaviour
{
	private Controls controls;

	private GameObject baseMenu;
	private GameObject currentOpenMenu;
	[NonSerialized] public bool gameIsPaused;

	[SerializeField] private GameObject titleMenu;
	[SerializeField] private GameObject levelSelectionMenu;
	[SerializeField] private GameObject settingsMenu;

	[SerializeField] private GameObject confirmController;
	[SerializeField] private Button confirmButton;
	[SerializeField] private TextMeshProUGUI confirmText;

	public AudioMixer audioMixer;

	private float normalFixedDeltaTime;

	private void Awake()
	{
		controls = new Controls();
		normalFixedDeltaTime = Time.fixedDeltaTime;
	}

	private void Start()
	{
		baseMenu = titleMenu;
		baseMenu.SetActive(true);
		levelSelectionMenu.SetActive(false);
		settingsMenu.SetActive(false);
		titleMenu.SetActive(true);
	}

	void Update()
	{
		if (controls.Menu.MenuESC.WasPerformedThisFrame())
		{
			HandleMenu();
		}
	}

	private void OnEnable()
	{
		controls.Enable();
	}

	private void OnDisable()
	{
		controls.Disable();
	}

	public void HandleMenu()
	{
		if (confirmController.activeSelf == true) confirmController.SetActive(false);
		else if (titleMenu.activeSelf == true) return;
		else CloseSelectedMenu();
	}

	public void OpenSelection(GameObject currentMenu)
	{
		{
			currentOpenMenu = currentMenu;
			currentMenu.SetActive(true);

			titleMenu.SetActive(false);

			//AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
		}
	}

	public void CloseSelectedMenu()
	{
		if (currentOpenMenu != null)
		{
			currentOpenMenu.SetActive(false);
			currentOpenMenu = null; // Clear previous menu after returning
			baseMenu.SetActive(true);
		}
		else
		{
			Debug.LogWarning("No previous menu to return to. Going back to inGameMenu.");
			baseMenu.SetActive(true);
		}
		//AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
	}

	public void EndGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	private void OpenConfirmController(UnityAction buttonEvent, string text)
	{

		confirmText.text = text;

		confirmButton.onClick.RemoveAllListeners();
		confirmButton.onClick.AddListener(() => buttonEvent());
		confirmController.SetActive(true);

		//AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
	}

	public void CloseConfirmSelection()
	{
		confirmController.SetActive(false);

		//AudioManager.Instance.PlayUtilityOneshot((int)AudioManager.UtilitySounds.MenuSelect);
	}

	public void TimeScaleToZero()
	{
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0;
	}

	public void ResetTimeScale()
	{
		Time.timeScale = 1;
		Time.fixedDeltaTime = normalFixedDeltaTime;
	}
}
