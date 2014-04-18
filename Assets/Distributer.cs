using UnityEngine;
using System.Collections;

public class Distributer : MonoBehaviour {
	public GameObject furfab;
	public int nx;
	public int nz;

	// Use this for initialization
	void Start () {
		for (var z = 0; z < nz; z++) {
			for (var x = 0; x < nx; x++) {
				var fur = (GameObject)Instantiate(furfab);
				var tr = fur.transform;
				tr.parent = transform;
				tr.localPosition = new Vector3(10f * x, 0f, 10f * z);
			}
		}
	}
}
