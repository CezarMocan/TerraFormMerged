using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton {
	public class GenerateMeshFrames : MonoBehaviour {
		public static int FRAMES_PER_FULL_MOVEMENT = 15;

		Dictionary<int, MeshedSkeleton> meshIdToObject;
		List<GameObject> meshes;
		int tick;

		public float minRotation = -44f;
		public float maxRotation = 10f;

		// Use this for initialization
		void Start () {
			GameObject container = new GameObject ("MeshContainer");

			Debug.Log ("starting");
			Debug.Log (this.gameObject.transform.position);
			container.transform.localPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
			this.gameObject.transform.SetParent (container.transform);
			this.gameObject.transform.localPosition = new Vector3 (0, 0, 0);

			meshIdToObject = new Dictionary<int, MeshedSkeleton> ();
			GameObject leftLeg = this.findChildWithName(this.gameObject, "LEFT_LEG");
			GameObject rightLeg = this.findChildWithName(this.gameObject, "RIGHT_LEG");
			GameObject leftArm = this.findChildWithName(this.gameObject, "LEFT_ARM");
			GameObject rightArm = this.findChildWithName(this.gameObject, "RIGHT_ARM");

			float currRotation = minRotation;
			float rotationDelta = 2f * (maxRotation - minRotation) / (1f * FRAMES_PER_FULL_MOVEMENT);

			float deltaSign = 1f;

			this.meshes = new List<GameObject> ();
			string guid = AssetDatabase.CreateFolder ("Assets/Creatures/Resources/AnimatedMeshes", Mathf.Abs(container.GetInstanceID ()).ToString ());
			AssetDatabase.SaveAssets ();
			string folderPath = AssetDatabase.GUIDToAssetPath( guid );

			for (int i = 0; i < FRAMES_PER_FULL_MOVEMENT; i++) {
				if (currRotation + rotationDelta > maxRotation)
					deltaSign = -1f;

				currRotation += deltaSign * rotationDelta;
				//float currOffsetRotation = 
				if (leftLeg != null) 
					leftLeg.transform.localEulerAngles = new Vector3(currRotation, leftLeg.transform.localEulerAngles.y, leftLeg.transform.localEulerAngles.z);
				if (rightLeg != null) 
					rightLeg.transform.localEulerAngles = new Vector3(currRotation, rightLeg.transform.localEulerAngles.y, rightLeg.transform.localEulerAngles.z);
				if (leftArm != null) 
					leftArm.transform.localEulerAngles = new Vector3(currRotation, leftArm.transform.localEulerAngles.y, leftArm.transform.localEulerAngles.z);
				if (rightArm != null)
					rightArm.transform.localEulerAngles = new Vector3(currRotation, rightArm.transform.localEulerAngles.y, rightArm.transform.localEulerAngles.z);

				MeshedSkeleton ms = new MeshedSkeleton(container, this.gameObject.transform.localPosition, this.gameObject, meshIdToObject, false, false);
				GameObject mesh = ms.getMeshGameObject ();
				mesh.transform.SetParent (container.transform);
				mesh.name = "MESH_" + i.ToString();
				if (i > 0)
					mesh.SetActive (false);
				this.meshes.Add (mesh);

				AssetDatabase.CreateAsset (mesh.GetComponent<MeshFilter>().mesh, folderPath + "/RUN_" + i.ToString() + ".asset");
			}

			AssetDatabase.SaveAssets ();

			container.AddComponent<Rigidbody> ();
			Rigidbody rb = container.GetComponent<Rigidbody> ();
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}
		
		// Update is called once per frame
		void Update () {
		}

		GameObject findChildWithName(GameObject go, string name) {
			if (go.name.Equals (name))
				return go;

			foreach (Transform child in go.transform) {
				GameObject childResult = this.findChildWithName (child.gameObject, name);
				if (childResult != null)
					return childResult;
			}

			return null;
		}
	}
}