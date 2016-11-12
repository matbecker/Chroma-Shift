using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	[SerializeField] CameraBehaviour cam;
	[SerializeField] BoxCollider2D aoeTrigger;
	private float shakeTimer;

	void Start()
	{
		aoeTrigger.enabled = false;
	}
	// Update is called once per frame
	void Update () 
	{
		if (shakeTimer > 0.0f)
		{
			Vector2 shakeAmount = Random.insideUnitCircle * 0.2f;
			//shake the cameras position randomly by a small value 
			cam.GetComponent<Transform>().position = new Vector3(cam.GetComponent<Transform>().position.x + shakeAmount.x, cam.GetComponent<Transform>().position.y + shakeAmount.y, cam.GetComponent<Transform>().position.z);

			shakeTimer -= Time.deltaTime;
		}
		else
			shakeTimer = 0.0f;
	}
	public void StartAreaOfEffect()
	{
		//start the shaking effect
		shakeTimer = 0.2f;
		aoeTrigger.enabled = true;
	}
	public void StopAreaOfEffect()
	{
		aoeTrigger.enabled = false;
	}
}
