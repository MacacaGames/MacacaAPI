#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class iCloudKeyValue  {
	[DllImport ("__Internal")]
	public static extern void iCloudKV_Synchronize();
	
	[DllImport ("__Internal")]
	public static extern void iCloudKV_SetString(string key, string value);
	
	[DllImport ("__Internal")]
	public static extern void iCloudKV_SetInt(string key, int value);
	
	[DllImport ("__Internal")]
	public static extern void iCloudKV_SetFloat(string key, float value);

	[DllImport ("__Internal")]
	public static extern string iCloudKV_GetString(string key);
	
	[DllImport ("__Internal")]
	public static extern int iCloudKV_GetInt(string key);
	
	[DllImport ("__Internal")]
	public static extern float iCloudKV_GetFloat(string key);
	
	[DllImport ("__Internal")]
	public static extern void iCloudKV_Reset();

	[DllImport ("__Internal")]
	public static extern bool iCloud_Enable();
}
#endif