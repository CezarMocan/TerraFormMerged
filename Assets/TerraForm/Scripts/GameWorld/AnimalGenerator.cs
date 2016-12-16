using UnityEngine;
using System.Collections;
using Skeleton;

public static class AnimalGenerator {
	public static int noAnimals = 300;
	public static string[] animals = { "492108", "2592010", "14259702", "5542044" };
	public static string[] materials = { "Zebra", "Leopard", "HumanSkin", "Hippo", "Lizard", "BlueFeather" };

	public static float minX = -2000f;
	public static float maxX = 2000f;

	public static float minZ = -2000f;
	public static float maxZ = 2000f;

	public static void Initialize() {
		for (int i = 0; i < noAnimals; i++) {
			GameObject animalGO = new GameObject ("Animal_" + i.ToString());
			animalGO.transform.position = new Vector3 (Random.Range (minX, maxX), 800f, Random.Range (minX, maxX));
			float scale = Random.Range (0.4f, 3f);
			animalGO.transform.localScale = new Vector3 (scale, scale, scale);

			Skeleton.AnimateMesh animationScript = animalGO.AddComponent<Skeleton.AnimateMesh> ();
			animationScript.animatedAssetId = animals[Mathf.FloorToInt (Random.Range (0f, animals.Length))];

			string materialName = materials[Mathf.FloorToInt (Random.Range (0f, materials.Length))];
			Material m = Resources.Load ("Materials/" + materialName, typeof(Material)) as Material;
			animationScript.mat = m;

			animationScript.moveDirection = Random.Range (-1.5f, 0.7f);

		}
	}

}
