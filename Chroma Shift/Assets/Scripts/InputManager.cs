using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {


	private static InputManager instance;
	public static InputManager Instance
	{
		get
		{
			if (!instance) 
				instance = GameObject.FindObjectOfType (typeof(InputManager)) as InputManager;

			return instance;
		}

	}

	//delegate for keyDown event
	public delegate void KeyDownEvent();
	//delegate for KeyUp event
	public delegate void KeyUpEvent();
	//delegate for Mouse event
	public delegate void MouseEvent(float x, float y);
	//delegate for Axis event
	public delegate void AxisEvent(float axisValue);

	//Input events
	public event KeyDownEvent Jump;
	public event KeyDownEvent DoubleJump;
	public event KeyDownEvent Hover;
	public event KeyDownEvent Attack;
	public event KeyDownEvent Block;
	public event KeyDownEvent SwitchColour;
	public event KeyDownEvent SwitchShade;
	public event KeyDownEvent Pause;
	public event KeyUpEvent UnBlock;
	public event KeyUpEvent UnAttack;
	public event AxisEvent Run;
	public event MouseEvent TrackMouseEvent;

	// Use this for initialization
	void Start () 
	{
	}

	void Update()
	{
		//if the Jump button is pushed and there are any subscribers
		if (Input.GetButtonDown("Jump"))
		{
			//subscribe all different jump event;
			if (Jump != null)
				Jump();

			if (DoubleJump != null)
				DoubleJump();

			if (Hover != null)
				Hover();
		}
		//if the Attack Button is pushed and there are subscribers
		if (Input.GetButtonDown("Attack") && Attack != null)
		{
			//Call the Attack event
			Attack();
		}
		//if the attack button is pushed and there are subscribers
		if (Input.GetButtonUp("Attack") && UnAttack != null)
		{
			//Call the UnAttack event
			UnAttack();
		}
		//if there are any Run Subscribers
		if (Run != null)
		{
			//Call the Run Event and link it to the horiztonal axis
			Run(Input.GetAxis("Horizontal"));
		}
		//if the Block Button is pushed and there are subscribers
		if (Input.GetButtonDown("Block") && Block != null)
		{
			//Call the block event
			Block();
		}
		//if the Block Button is released and there are subscribers
		if (Input.GetButtonUp("Block") && UnBlock != null)
		{
			//call the UnBlock event
			UnBlock();
		}
		//if the Pause Button is pushed and there are subscriberss
		if (Input.GetButtonDown("Pause") && Pause != null)
		{
			//Call the pause event
			Pause();
		}
		//if the SwitchColour button is pushed and there are subscribers
		if (Input.GetButtonDown("SwitchColour") && SwitchColour != null)
		{
			//call the SwitchColour event
			SwitchColour();
		}
		//if the SwitchShade button is pushed and there are subscribers
		if (Input.GetButtonDown("SwitchShade") && SwitchShade != null)
		{
			//call the SwitchShade event
			SwitchShade();
		}
		//if there are subscribers
		if (TrackMouseEvent != null)
		{
			//track the cursor of the mouse
			TrackMouseEvent(Input.mousePosition.x, Input.mousePosition.y);
		}
	}
}
