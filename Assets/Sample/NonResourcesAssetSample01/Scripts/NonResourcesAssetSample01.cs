using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NonResourcesAssetSample01 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// prefab読み込み
		GameObject prefab = MyUtility.MyUtility.NonResourcesAsset.Load<GameObject>("Assets/Sample/NonResourcesAssetSample01/Data/Cube.prefab");
		Assert.IsNotNull(prefab);

		// 読み込んだprefabをGameObjectのインスタンス化
		GameObject.Instantiate(prefab);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
