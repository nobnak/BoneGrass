using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour {
	public const float TWO_PI = 2f * Mathf.PI;

	public float duration;
	public float phase;
	public float scale;

	void Update () {
		var time = Time.time;
		var counter = Mathf.FloorToInt(time / duration);
		var t = (time - counter * duration) / duration;

		var theta = TWO_PI * (t + phase);
		var pos = transform.localPosition;
		pos.x = scale * Mathf.Sin(theta);
		transform.localPosition = pos;
	}
}
