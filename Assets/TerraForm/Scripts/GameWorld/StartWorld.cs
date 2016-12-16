﻿using UnityEngine;
using System.Collections;

public class StartWorld : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NoiseGenerator.Initialize();
		AnimalGenerator.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        CellManager.GetCellManager().Tick();
	}
}
