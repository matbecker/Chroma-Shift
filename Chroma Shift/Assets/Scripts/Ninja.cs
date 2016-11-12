using UnityEngine;
using System.Collections;

public class Ninja : Hero {

	[SerializeField] private bool canDoubleJump;
	[SerializeField] GameObject dagger;
	[SerializeField] SpriteRenderer headband;
	[SerializeField] float invisibleDuration;
	[SerializeField] float lerpDuration;
	private Color currentColor;
	private float startLerpTimer;
	private float endLerpTimer;
	private bool freezeBlock;

	// Use this for initialization
	protected override void Start ()
	{	
		base.Start ();
		InputManager.Instance.DoubleJump += DoubleJump;
		currentColor = colour.GetCurrentColor();
		shieldBar.enabled = false;
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		InputManager.Instance.DoubleJump -= DoubleJump;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		if (startShieldTimer)
		{
			//make the ninja sprite dissapear
			sprite.color = Color.Lerp(sprite.color, Color.clear, startLerpTimer);
			//make the headband sprite dissapear
			headband.color = Color.Lerp(headband.color, Color.clear, startLerpTimer);
			//make the dagger dissapear
			dagger.SetActive(false);

			startLerpTimer += Time.deltaTime / lerpDuration;
		}
		else
		{
			freezeBlock = false;
			//make the ninja sprite reappear
			sprite.color = Color.Lerp(Color.clear, currentColor, endLerpTimer);
			//make the headband sprite reappear
			headband.color = Color.Lerp(Color.clear, Color.black, endLerpTimer);
			//make the dagger reappear
			dagger.SetActive(true);

			endLerpTimer += Time.deltaTime / lerpDuration;
		}
	}
	protected override void Attack ()
	{
		base.Attack ();
	}
	protected override void Block ()
	{
		base.Block ();
		//ninja cannot see his shield timer
		currentColor = colour.GetCurrentColor();

		//dont allow the player to reset the LerpTimer if they are currently blocking
		if (!freezeBlock && canBlock)
		{
			startLerpTimer = 0.0f;
			endLerpTimer = 0.0f;
		}
		freezeBlock = true;
	}
	protected override void OnCollisionExit2D (Collision2D other)
	{
		base.OnCollisionExit2D (other);
		//if the player has left the ground layer they can double jump
		canDoubleJump = true;
	}
	protected override void OnCollisionStay2D(Collision2D other)
	{
		base.OnCollisionStay2D(other);
		//if the player is on the ground layer they cannot double jump
		canDoubleJump = false;
	}
	private void DoubleJump()
	{
		//if the player can double jump
		if (canDoubleJump)
		{
			//apply a force in the y direction once again
			rb.AddForce(stats.jumpForce);
			//set the canDoubleJump variable to false to ensure the player cannot jump a third time
			canDoubleJump = false;
		}
	}
}
