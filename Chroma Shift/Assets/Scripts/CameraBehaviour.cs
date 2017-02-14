using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CameraBehaviour : MonoBehaviour {

	[SerializeField] Vector2 velocity;
	[SerializeField] float smoothTime;
	[SerializeField] Hero hero;
	[SerializeField] Vector2 minPos;
	[SerializeField] Vector2 maxPos;
	[SerializeField] float verticalSpeed;
	[SerializeField] float horizontalSpeed;
	[SerializeField] float ZoomValue;
	public bool inGame;

	void Awake()
	{
		InputManager.Instance.HorizontalMovement += HorizontalMovement;
		InputManager.Instance.VerticalMovement += VerticalMovement;
		InputManager.Instance.CameraZoomIn += CameraZoomIn;
		InputManager.Instance.CameraZoomOut += CameraZoomOut;
		LevelManager.Instance.OnHeroSpawned += OnHeroSpawned;
	}

	void OnHeroSpawned(Hero hero) 
	{
		this.hero = hero;
	}

	// Update is called once per frame
	void Update () 
	{
		if(hero == null)
			return;
		
		if (inGame)
		{
			//if (hero.GetComponent<Hero>().isFollowTarget)
			//{
			Vector2 pos = new Vector2(Mathf.SmoothDamp(transform.position.x, hero.transform.position.x, ref velocity.x, smoothTime),
				Mathf.SmoothDamp(transform.position.y, hero.transform.position.y, ref velocity.y, smoothTime));

			transform.position = new Vector3(pos.x, pos.y, transform.position.z);

			transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(transform.position.y, minPos.y, maxPos.y), transform.position.z);
			//}	
		}
	}

	private void CameraZoomIn()
	{
		var cam = gameObject.GetComponent<Camera>();
		var zV = ZoomValue;

		if (cam.orthographicSize < 1.5f)
		{
			zV = 0.0f;
		}
		else
		{
			zV = ZoomValue;
		}
		cam.orthographicSize -= zV;
	}
	private void CameraZoomOut()
	{
		var cam = gameObject.GetComponent<Camera>();
		var zV = ZoomValue;

		if (cam.orthographicSize > 20.0f)
		{
			zV = 0.0f;
		}
		else
		{
			zV = ZoomValue;
		}
		cam.orthographicSize += zV;
	}
	//editor stuff
	private void HorizontalMovement(float horizontalAxis)
	{
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), 
										Mathf.Clamp(transform.position.y, minPos.y, maxPos.y), 
										transform.position.z);

		if (horizontalAxis > 0)
		{
			transform.position += Vector3.right * horizontalSpeed;
		}
		if (horizontalAxis < 0)
		{
			transform.position += Vector3.left * horizontalSpeed;
		}
	}
	private void VerticalMovement(float verticalAxis)
	{
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), 
			Mathf.Clamp(transform.position.y, minPos.y, maxPos.y), 
			transform.position.z);
		
		if (verticalAxis > 0)
		{
			transform.position += Vector3.up * verticalSpeed;
		}
		if (verticalAxis < 0)
		{
			transform.position += Vector3.down * verticalSpeed;
		}
	}
}
