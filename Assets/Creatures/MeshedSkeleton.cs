﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Skeleton {
	public class MeshedSkeleton {
		private SphereCompleter sc;
		private GameObject container, gameObject, meshGameObject, originalSkeleton;
		private Dictionary<int, MeshedSkeleton> meshIdToObject;
		private bool isSelected;
		private bool isMeshMutated;
		private Vector3 positionOnScreen;

		public static Color SELECTED_COLOR = Color.red;
		public static Color UNSELECTED_COLOR = Color.white;

		public MeshedSkeleton(GameObject container, Vector3 positionOnScreen, GameObject gameObject, Dictionary<int, MeshedSkeleton> meshIdToObject, bool isSelected = false, bool isMeshMutated = false) {
			this.container = container;
			this.positionOnScreen = positionOnScreen;
			this.originalSkeleton = gameObject;
			this.originalSkeleton.SetActive (false);
			this.originalSkeleton.transform.parent = this.container.transform;
			this.originalSkeleton.name = "OriginalSkeleton" + this.container.GetInstanceID ().ToString ();

			this.meshIdToObject = meshIdToObject;
			this.isSelected = isSelected;
			this.isMeshMutated = isMeshMutated;

			this.gameObject = (GameObject) Object.Instantiate (this.originalSkeleton, new Vector3 (0, 0, 0), Quaternion.identity, this.container.transform);
			this.gameObject.SetActive (false);
			this.gameObject.name = "FilledOutSkeleton" + this.container.GetInstanceID ().ToString ();
			this.gameObject.transform.localPosition = new Vector3 (0, 0, 0);
			this.gameObject.transform.localEulerAngles = this.originalSkeleton.transform.localEulerAngles;

			this.container.transform.localPosition = positionOnScreen;
			this.sc = null;
			this.updateMesh ();
		}

		public MeshedSkeleton(GameObject container, Vector3 positionOnScreen, SkeletonMutation mutation, Dictionary<int, MeshedSkeleton> meshIdToObject, bool isSelected = false):
		this(container, positionOnScreen, mutation.getMutatedObject(), meshIdToObject, isSelected, true) {
		}

		public void toggleSpheresVisibility(bool spheresVisible) {
			List<Renderer> renderers = new List<Renderer> (this.gameObject.GetComponentsInChildren<Renderer> ());
			foreach (Renderer r in renderers) {
				r.enabled = spheresVisible;
			}
		}

		public SphereCompleter getSphereCompleter() {
			return this.sc;
		}

		private Color getMeshColor() {
			return this.isSelected ? MeshedSkeleton.SELECTED_COLOR : MeshedSkeleton.UNSELECTED_COLOR;
		}

		public void updateMeshNoMove() {
			if (this.isMeshMutated)
				this.meshMutation ();
		}

		public void updateMesh() {
			if (this.sc != null) {
				meshIdToObject.Remove (this.getMeshId ());
				this.sc.removeMeshes ();
			}

			Color color = this.getMeshColor ();
			this.sc = new SphereCompleter (this.container, this.gameObject, color);
			this.meshGameObject = sc.generatePathAndMesh ();
			if (this.isMeshMutated)
				this.meshMutation ();
			meshIdToObject.Add (this.getMeshId (), this);
		}

		public void updateMeshColor() {
			Color color = this.getMeshColor ();
			if (this.isSelected) {
				Material material = Resources.Load ("Zebra", typeof(Material)) as Material; //new Material (Shader.Find ("Zebra"));
				//material.color = MeshedSkeleton.SELECTED_COLOR;
				MeshRenderer renderer = this.meshGameObject.GetComponent<MeshRenderer> ();
				renderer.material = material;
			} else {
				Material material = new Material (Shader.Find ("Diffuse"));
				material.color = MeshedSkeleton.UNSELECTED_COLOR;
				MeshRenderer renderer = this.meshGameObject.GetComponent<MeshRenderer> ();
				renderer.material = material;
			}
		}
			
		private void meshMutation() {
			/*
			Debug.Log ("mesh mutation");

			Skeleton.Perlin noise = new Skeleton.Perlin ();
			Mesh mesh = ((MeshFilter) this.meshGameObject.GetComponent<MeshFilter> ()).mesh;

			Vector3[] baseVertices = mesh.vertices;
			Vector3[] vertices = new Vector3[baseVertices.Length];

			float speed = 1.0f;
			float scale = 1.0f;
			float timex = Time.time * speed + 0.1365143f;
			float timey = Time.time * speed + 1.21688f;
			float timez = Time.time * speed + 2.5564f;
			for (int i=0;i<vertices.Length;i++) {
				Vector3 vertex = baseVertices[i];
				if (Random.Range (0f, 1f) >= 0f) {
					vertex.x += noise.Noise(timex + vertex.x, timex + vertex.y, timex + vertex.z) * scale;
					vertex.y += noise.Noise(timey + vertex.x, timey + vertex.y, timey + vertex.z) * scale;
					vertex.z += noise.Noise(timez + vertex.x, timez + vertex.y, timez + vertex.z) * scale;
				}
				vertices[i] = vertex;
			}

			mesh.vertices = vertices;

			//if (recalculateNormals)	
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			*/
		}


		public void toggleSelected() {
			Debug.Log("toggle selected");
			this.isSelected = !this.isSelected;
			this.updateMeshColor ();
		}

		public bool isObjectSelected() {
			return this.isSelected;
		}
			
		public GameObject getMeshGameObject() {
			return this.meshGameObject;
		}

		public int getMeshId() {
			return this.meshGameObject.GetInstanceID ();
		}

		public GameObject getContainer() {
			return this.container;
		}

		public GameObject getGameObject() {
			return this.gameObject;
		}

		public GameObject getOriginalSkeleton() {
			return this.originalSkeleton;
		}

		public Vector3 getPositionOnScreen() {
			return this.positionOnScreen;
		}

	}
}