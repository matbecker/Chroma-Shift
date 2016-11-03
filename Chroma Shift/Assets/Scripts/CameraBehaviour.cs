using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	[SerializeField] Vector2 velocity;
	[SerializeField] float smoothTime;
	[SerializeField] GameObject hero;
	[SerializeField] Vector2 minPos;
	[SerializeField] Vector2 maxPos;
	// Use this for initialization
	void Start () 
	{
		hero = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 pos = new Vector3(Mathf.SmoothDamp(transform.position.x, hero.transform.position.x, ref velocity.x, smoothTime),
			Mathf.SmoothDamp(transform.position.y, hero.transform.position.y, ref velocity.y, smoothTime));

		transform.position = new Vector3(pos.x, pos.y, transform.position.z);

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(transform.position.y, minPos.y, maxPos.y), transform.position.z);

	}
}
