using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_CallbackTest : MonoBehaviour {

	// Use this for initialization
	public void OnAllow(string permission){
		Debug.Log("OnAllow" + permission);
	}
	public void OnDeny(string permission){
		Debug.Log("OnDeny" + permission);

	}
	public void OnDenyAndNeverAskAgain(string permission){
		Debug.Log("OnDenyAndNeverAskAgain" + permission);
	}
}
