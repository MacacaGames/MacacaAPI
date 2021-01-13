# 如何使用

以 SubModules 或 SubTree 的方式將本專案 Clone 至您的 Unity 專案中即可。


# 記得匯入套件！！

部分的功能需要手動將 CloudMacacaAPI/Package 資料夾中的 unitypackage 手動匯入至專案，匯入的套件請維持其目錄位置（匯入套件時 Unity 會自動安排目錄位置，請勿任意更動）。

# Push 的設定方式
在任何低方修改 CM_LocalNotificationControllerㄡsmallIcon 變數為值為通知圖示的檔案名稱（檔名部分即可，不需要副檔名），預設是使用 App 本身的 Icon，例如：

	void Awake(){
		CM_LocalNotificationController.smallIcon = "app_icon";
	}

在 /Assets/Plugins/Android/res/drawable/ 中將通知的圖示放入本資料夾

<font color="red">注意：沒有在本資料夾放入圖示圖片，在部分 Android 版本中彈出通知時可能會倒致 App 閃退！！
<font>


# Android 必須的額外設定

當首次匯入套件後 PlayServiceResolver 會自動把需要的第三方 Android Library 加入至專案中位於 Assets/Plugins/Android。

請手動於 Assets/Plugins/Android 加入 AndroidManifes.xml 檔案，檔案內容參考以下。

此外請進行以下操作：
- 在 {Google Play Service Id Here} 部分替換成對應的 Google Play Service Id
（請保留前面的 "\ " 字符，例如：\ 589650241221）
- 在 {Package Name Here} 部分替換成對應的 PackageName (遊戲包名)



    <?xml version="1.0" encoding="utf-8"?>
    <manifest
	    xmlns:android="http://schemas.android.com/apk/res/android"
	    package="com.unity3d.player"
	    xmlns:tools="http://schemas.android.com/tools"
	    android:installLocation="preferExternal"
	    android:versionCode="1"
	    android:versionName="1.0">
    <supports-screens
        android:smallScreens="true"
        android:normalScreens="true"
        android:largeScreens="true"
        android:xlargeScreens="true"
        android:anyDensity="true"/>

	    <application
	        android:theme="@style/UnityThemeSelector"
	        android:icon="@drawable/app_icon"
	        android:label="@string/app_name">
	        <activity android:name="com.unity3d.player.UnityPlayerActivity"
                  android:label="@string/app_name">
	            <intent-filter>
	                <action android:name="android.intent.action.MAIN" />
	                <category android:name="android.intent.category.LAUNCHER" />
	            </intent-filter>
	            <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
	        </activity>
	        <meta-data android:name="com.google.android.gms.games.APP_ID" android:value="\ {Google Play Service Id Here}" />
			<provider android:name="com.CloudMacaca.AndroidNative.UnityShareContentProvider"
				android:authorities="{Package Name Here}"
				android:exported="false"
				android:grantUriPermissions="true" />
	    </application>
    </manifest>



# iOS 的額外設定

理論上 CloudMacacaAPI 會自動修改相關需要的設定，無須做其他調整。


	   
