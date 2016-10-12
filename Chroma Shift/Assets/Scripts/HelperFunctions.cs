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
	public static Vector3 ArcTowards(Transform start, Transform end, float angle)
	{
		var direction = end.position - start.position;
		var height = direction.y;
		direction.y = 0;
		var distance = direction.magnitude;
		var a = angle * Mathf.Deg2Rad;
		direction.y = distance * Mathf.Tan(a);
		distance += height / Mathf.Tan(a);
		var velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

		return velocity * direction.normalized;
	}
}
