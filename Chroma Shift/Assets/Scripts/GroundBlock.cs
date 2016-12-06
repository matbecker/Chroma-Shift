using UnityEngine;
using System.Collections;

public class GroundBlock : MonoBehaviour {

	[SerializeField] float duration;
	[SerializeField] SpriteRenderer sprite;
	private float startLerpTimer;
	private float endLerpTimer;
	private bool hasCollided;
	// Use this for initialization
	void Start () 
	{
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (hasCollided)
		{
			endLerpTimer = 0.0f;

			sprite.color = Color.Lerp(sprite.color, Color.white, startLerpTimer);

			startLerpTimer += Time.deltaTime / duration;
		}
		else
		{
			startLerpTimer = 0.0f;

			sprite.color = Color.Lerp(sprite.color, Color.black, endLerpTimer);

			endLerpTimer += Time.deltaTime / duration;
		}
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		hasCollided = true;
	}
	void OnCollisionExit2D(Collision2D other)
	{
		hasCollided = false;
	}
}
