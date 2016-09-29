using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	[System.Serializable]
	public class Stats
	{
		public int health;
		public int attackPower;
		public int defence;
		public float attackRange;
		public float attackSpeed;
		public Vector2 jumpForce;
		public Vector2 movementForce;
	}

	[SerializeField] Stats stats;
	[SerializeField] Rigidbody2D rb;
	// Use this for initialization
	void Start () 
	{
		//register jump event
		InputManager.Instance.Jump += Jump;
		//register run event
		InputManager.Instance.Run += Run;
		rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Jump()
	{
		rb.AddForce(stats.jumpForce);
	}
	public void Run(float horizontalAxis)
	{
		if (horizontalAxis > 0)
		{
			rb.AddForce(stats.movementForce);
		}
		if (horizontalAxis < 0)
		{
			rb.AddForce(-stats.movementForce);
		}
	}
}
