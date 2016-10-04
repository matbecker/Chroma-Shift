using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class HelperFunctions {

	public static IEnumerator TransitionTransparency(Image img, float duration)
	{
		while (true)
		{
			img.CrossFadeAlpha(0.0f, duration, false);
			yield return new WaitForSeconds(duration);
			img.CrossFadeAlpha(1.0f, duration, false);
			yield return new WaitForSeconds(duration);
		}
	}
}
