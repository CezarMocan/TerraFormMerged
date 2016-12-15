using UnityEngine;
using System.Collections;

public class MoveOnZ : MonoBehaviour {
	private bool isMoving;
	// Use this for initialization
	void Start () {
		isMoving = true;
		//gameObject.transform.localEulerAngles = new Vector3 (0, 0, Mathf.Sin (1f) * 45);
	}

	// Update is called once per frame
	void Update () {
		if (isMoving) {
			float deltaZ = Mathf.Sin ((Time.time + Mathf.PI + Mathf.PI) * 3) * .7f;
			float deltaZ2 = Mathf.Sin ((Time.time + Mathf.PI) * 3) * .7f;
			gameObject.transform.localPosition += new Vector3 (0, 0, Mathf.Min(deltaZ, deltaZ2));
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			isMoving = !isMoving;
		}
	}
}
