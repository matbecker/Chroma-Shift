using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarBehaviour : MonoBehaviour {

	[SerializeField] Animator animator;
	[SerializeField] string[] trigNames;
	[SerializeField] Color[] starColours;
	[SerializeField] Image image;
	private Coroutine delayCor;
	private float currentDelay;
	private float timer;
	// Use this for initialization
	void Start () 
	{
		delayCor = null;
		currentDelay = Random.Range(1.0f,3.0f);
		image.color = starColours[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;

		if (timer > currentDelay)
		{
			float rand = Random.Range(1.0f, 3.0f);
			delayCor = StartCoroutine(DelayAnimCor(rand,animator));
			timer = 0.0f;
		}

		
	}
	private IEnumerator DelayAnimCor(float duration, Animator anim)
	{
		int rand = Random.Range(0,trigNames.Length);
		anim.SetTrigger(trigNames[rand]);

		yield return new WaitForSeconds(duration);

		currentDelay = Random.Range(1.0f,3.0f);
		StopCoroutine(delayCor);
		delayCor = null;
	}
}
