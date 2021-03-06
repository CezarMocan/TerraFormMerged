﻿using UnityEngine;
using System.Collections;
using Skeleton;

public class TestSmoothFilter : MonoBehaviour {

	private Mesh sourceMesh;
	private Mesh workingMesh;

	void Start () 
	{
		MeshFilter meshfilter = gameObject.GetComponentInChildren<MeshFilter>();

		// Clone the cloth mesh to work on
		sourceMesh = new Mesh();
		// Get the sourceMesh from the originalSkinnedMesh
		sourceMesh = meshfilter.mesh;
		// Clone the sourceMesh 
		workingMesh = CloneMesh(sourceMesh);
		// Reference workingMesh to see deformations
		meshfilter.mesh = workingMesh;


		// Apply Laplacian Smoothing Filter to Mesh
		int iterations = 1;
		for(int i=0; i<iterations; i++)
			//workingMesh.vertices = SmoothFilter.laplacianFilter(workingMesh.vertices, workingMesh.triangles);
			workingMesh.vertices = SmoothFilter.hcFilter(sourceMesh.vertices, workingMesh.vertices, workingMesh.triangles, 0.0f, 0.5f);
	}

	void Update() {

	}

	// Clone a mesh
	private static Mesh CloneMesh(Mesh mesh)
	{
		Mesh clone = new Mesh();
		clone.vertices = mesh.vertices;
		clone.normals = mesh.normals;
		clone.tangents = mesh.tangents;
		clone.triangles = mesh.triangles;
		clone.uv = mesh.uv;
		clone.uv2 = mesh.uv2;
		clone.uv2 = mesh.uv2;
		clone.bindposes = mesh.bindposes;
		clone.boneWeights = mesh.boneWeights;
		clone.bounds = mesh.bounds;
		clone.colors = mesh.colors;
		clone.name = mesh.name;
		//TODO : Are we missing anything?
		return clone;
	}
}