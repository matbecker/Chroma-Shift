using UnityEngine;
using System.Collections;
using System.Linq;

public class GroundBlock : LevelObject {

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
	public override string GetSaveString ()
	{
		return string.Join(SPLIT_CHAR.ToString(), new []{objectID, transform.position.x, transform.position.y, transform.position.z}.Select(s => s.ToString()).ToArray());
	}
	public override void LoadSaveData (string input)
	{
		var data = input.Split(SPLIT_CHAR);

		transform.position = new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
	}

}
