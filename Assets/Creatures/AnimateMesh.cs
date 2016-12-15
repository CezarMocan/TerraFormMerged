using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Skeleton {
	public class AnimateMesh : MonoBehaviour {

		List<GameObject> meshes;
		int tick;
		public Material mat;
		public bool showRays = false;
		public float moveDirection = -1f;
		Ray lastNormal;
		float acceleration = 0f;
		public string animatedAssetId = "14259702";

		private float forwardDirection = 0f;
		public float targetForwardDirection = 0f;
		// Use this for initialization
		void Start () {
			//this.rb = this.GetComponent<Rigidbody> ();
			//mat = Resources.Load ("Zebra", typeof(Material)) as Material;
			meshes = new List<GameObject> ();

			this.gameObject.AddComponent<Rigidbody> ();
			Rigidbody rb = this.gameObject.GetComponent<Rigidbody> ();
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			for (int i = 0; i < GenerateMeshFrames.FRAMES_PER_FULL_MOVEMENT; i++) {
				Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath ("Assets/Creatures/Resources/AnimatedMeshes/" + animatedAssetId + "/RUN_" + i.ToString() + ".asset", typeof(Mesh));
				GameObject meshGO = new GameObject ("MESH_" + i.ToString ());
				meshGO.transform.parent = this.transform;
				meshGO.transform.localPosition = new Vector3 (0, 0, 0);
				meshGO.transform.localEulerAngles = new Vector3 (0, 0, 0);
				meshGO.transform.localScale = new Vector3 (1, 1, 1);
				MeshFilter meshFilter = (MeshFilter) meshGO.AddComponent(typeof(MeshFilter));
				meshFilter.mesh = m;
				//meshFilter.mesh.RecalculateNormals ();
				//meshFilter.mesh.Optimize ();

				MeshRenderer renderer = meshGO.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
				renderer.material = mat;

				MeshCollider collider = meshGO.AddComponent (typeof(MeshCollider)) as MeshCollider;
				collider.convex = true;

				if (i > 0) {
					meshGO.SetActive (false);
				}

				meshes.Add (meshGO);
			}

			lastNormal.origin = this.gameObject.transform.localPosition;
			lastNormal.direction = Vector3.up;

			this.targetForwardDirection = Random.Range (-1f, 1f);
			this.moveDirection = -1 * Random.Range (0.5f, 2f);
		}

		private void animateMotion() {
			GameObject activeMesh = null;
			bool nextActive = false;
			foreach (GameObject mesh in this.meshes) {
				if (mesh.activeSelf) {
					mesh.SetActive (false);
					nextActive = true;
				} else {
					if (nextActive) {
						mesh.SetActive (true);
						activeMesh = mesh;
						nextActive = false;
					}
				}
			}
			if (nextActive) {
				this.meshes [0].SetActive (true);
				activeMesh = this.meshes [0];
			}
		}

		private void adjustForwardDirection() {
			if (this.forwardDirection == this.targetForwardDirection) {
				if (Random.Range (0f, 300f) < 1f) {
					this.targetForwardDirection = Random.Range (-1f, 1f);
				}
			} else {
				if (this.forwardDirection < this.targetForwardDirection) {
					this.forwardDirection += 0.01f;
					if (this.forwardDirection > this.targetForwardDirection) {
						this.forwardDirection = this.targetForwardDirection;
					}
				} else {
					this.forwardDirection -= 0.01f;
					if (this.forwardDirection < this.targetForwardDirection) {
						this.forwardDirection = this.targetForwardDirection;
					}
				}
			}
		}

		private void adjustRotation() {
			this.adjustForwardDirection ();

			Vector3 perpForward = Vector3.Cross (lastNormal.direction, Vector3.right);
			Vector3 perpSideways = Vector3.Cross (lastNormal.direction, Vector3.forward);

			if (this.showRays) {
				Debug.DrawRay (this.gameObject.transform.localPosition, perpForward * 10, Color.blue, .2f);
				Debug.DrawRay (this.gameObject.transform.localPosition, perpSideways * 10, Color.cyan, .2f);
				Debug.DrawRay (this.gameObject.transform.localPosition, (this.gameObject.transform.forward * 10), Color.green, .2f);
			}

			Vector3 initEuler = this.gameObject.transform.localEulerAngles;
			this.gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(this.forwardDirection, -perpForward.y, -perpForward.z), lastNormal.direction);

			if (Mathf.Abs (initEuler.z - this.gameObject.transform.localEulerAngles.z) > 45) {
				this.gameObject.transform.localEulerAngles = new Vector3 (this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, initEuler.z);
			}

			if (Mathf.Abs (this.gameObject.transform.localEulerAngles.z) > 45f) {
				this.gameObject.transform.localEulerAngles = new Vector3 (this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, 0f);
			}

			if (Mathf.Abs (this.gameObject.transform.localEulerAngles.z) > 30f) {
				this.gameObject.transform.localEulerAngles = new Vector3 (this.gameObject.transform.localEulerAngles.x, this.gameObject.transform.localEulerAngles.y, 30f);
			}
		}

		private void moveBodyInSpace() {
			this.transform.localPosition += this.moveDirection * (this.gameObject.transform.forward * (acceleration * 0.5f) * Mathf.Pow(this.gameObject.transform.localScale.x, 1f));
			this.transform.localPosition += (this.transform.up * (acceleration * 0.1f) * Mathf.Pow(this.gameObject.transform.localScale.x, 0f));
			if (acceleration > 0)
				acceleration = acceleration - 1;
		}

		// Update is called once per frame
		void Update () {
			this.tick++;
			//if (tick % 2 == 0)
			//	return;
					
			this.animateMotion ();
			this.adjustRotation ();
			this.moveBodyInSpace ();
		}

		void FixedUpdate() {
			//rb = this.gameObject.GetComponent<Rigidbody> ();
			//rb.MovePosition(this.transform.position + this.transform.forward * Time.deltaTime * Mathf.Pow(this.transform.localScale.x, 2.5f));
		}

		void OnCollisionEnter(Collision collision) {
			if (acceleration <= 2) {
				acceleration = acceleration + 1;
			}
			//Debug.LogWarning ("*****");
			//Debug.LogWarning (this.gameObject.transform.forward);
			//Debug.LogWarning (this.gameObject.transform.right);
			//Debug.LogWarning ("*****");
			foreach (ContactPoint contact in collision.contacts) {
				//print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
				if (this.showRays) {
					Debug.DrawRay (contact.point, contact.normal * 10, Color.white, .2f);
				}
				if (Vector3.Magnitude(contact.normal) < .000001f)
					continue;
				lastNormal = new Ray ();
				lastNormal.origin = contact.point;
				lastNormal.direction = contact.normal;
				//Debug.LogWarning (lastNormal);
				//Debug.LogWarning (Vector3.Magnitude (lastNormal.direction));
				break;
			}
		}
	}
}