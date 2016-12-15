using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skeleton {
	public class AttachMesh : MonoBehaviour {
		Dictionary<int, MeshedSkeleton> meshIdToObject;
		// Use this for initialization
		void Start () {
			Debug.Log("start");
			meshIdToObject = new Dictionary<int, MeshedSkeleton> ();
			MeshedSkeleton ms = new MeshedSkeleton(this.gameObject.transform.parent.gameObject, this.gameObject.transform.localPosition, this.gameObject, meshIdToObject, false, false);
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}