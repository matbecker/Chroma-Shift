using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.DemiLib;
using DG.Tweening;
using UnityEngine.UI;

public class PauseOverlay : MonoBehaviour {

	private static PauseOverlay instance;
	public static PauseOverlay Instance
	{
		get
		{
			if (!instance)
				instance = GameObject.FindObjectOfType(typeof(PauseOverlay)) as PauseOverlay;

			return instance;
		}
	}
	private bool isPaused;
	public bool canPause;
	[SerializeField] Image sprite;
	[SerializeField] GameObject buttonPanel;
	[SerializeField] Animator anim;

	// Use this for initialization
	void Start () 
	{
		InputManager.Instance.Pause += TogglePause;
		sprite.color = Color.clear;
		isPaused = false;
		canPause = true;
	}
	void OnDestroy()
	{
		if (InputManager.Instance)
			InputManager.Instance.Pause -= TogglePause;
	}
	public void TogglePause()
	{
		if (canPause)
		{
			isPaused = !isPaused;

			if (isPaused)
			{
				
				buttonPanel.transform.DOScale(Vector3.one, 2.0f).SetDelay(3.0f).SetEase(Ease.OutBounce, 0.5f);
				sprite.DOColor(new Color(1,1,1,0.7f), 2.0f).OnComplete(() => {
					
				});
				CameraBehaviour.Instance.SetPauseScreen();
				PlayerUI.Instance.canvas.sortingOrder = 3;
				PlayerUI.Instance.TimerStop();
				LevelManager.Instance.startTimer = false;
			}
			else
			{
				buttonPanel.transform.DOScale(Vector3.zero, 1.0f);
				sprite.DOColor(Color.clear, 2.0f);
				CameraBehaviour.Instance.ExitPauseScreen();
				PlayerUI.Instance.canvas.sortingOrder = 10;
				PlayerUI.Instance.TimerStart();
				LevelManager.Instance.startTimer = true;
			}
			StarBehaviour.Instance.SetPaused(isPaused);
		}

 	}
	public void Restart()
	{
		LoadingScreen.Instance.DisplayLoadingScreen(false);
		TogglePause();
		anim.SetTrigger("Normal");
	}
	public void LoadScene(string sceneName)
	{
		LevelLoader.Instance.sceneLoaded = false;
		SceneManager.LoadScene(sceneName);
	}
	// Update is called once per frame
	void Update () 
	{

	}
}
