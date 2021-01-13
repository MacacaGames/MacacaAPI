using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MacacaGames
{
    public class CM_APIController : MonoBehaviour
    {
        //小寫的代表 plugin 內的方法
        //大寫代表 Unity 的實作

#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void doCMVibrationAsPop();

        [DllImport("__Internal")]
        private static extern void doCMVibrationAsPeek();

        [DllImport("__Internal")]
        private static extern void doCMVibrationAsNope();

        [DllImport("__Internal")]
        private static extern void doCMShowToastMessage(string msg);
        [DllImport("__Internal")]
        private static extern void doCMShowAlertDialog(string title, string msg, string okText);
        [DllImport("__Internal")]
        private static extern void doCMShowRateUsDialog(string msg, string canelTest, string rateText, string laterText, string AppleStoreId);

        [DllImport("__Internal")]
        private static extern void doCMShowAlertDialogWithCallback(string title, string msg, string positiveBtnText, string negativeBtnText);

        [DllImport("__Internal")]
        private static extern void doCMShowDatePickerWithCallback(string okText, string cancelText);

        [DllImport("__Internal")]
        private static extern void doSetAlertDialogCallback(AlertDialogCallback cb);
        [DllImport("__Internal")]
        private static extern void doSetDatePickerCallback(DatePickerDialogCallback cb);

        [DllImport("__Internal")]
        private static extern void doCMShowShare(string filePath, string subject, string msg, int typeId);    

        [DllImport("__Internal")]
        private static extern void doShowAchievement();

        [DllImport("__Internal")]
        private static extern void doShowLeaderboard();

        [DllImport("__Internal")]
        private static extern void doUnlockAchievent(string achievementId);

        [DllImport("__Internal")]
        private static extern void doUploadLeaderboard(string boardId, int score);

        [DllImport("__Internal")]
        private static extern void doGameCenterInit();

        [DllImport("__Internal")]
        private static extern void doRequestIDFA();
        [DllImport("__Internal")]
        private static extern void doSetRequestIDFACallback(RequestCallback cb);
#endif
#if UNITY_EDITOR
        private static AndroidJavaObject cmAPIControllerAndroid = null;
        private static AndroidJavaObject cmGameServiceControllerAndroid = null;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaObject cmAPIControllerAndroid = new AndroidJavaObject("com.CloudMacaca.AndroidNative.CMAndroidController");
		private static AndroidJavaObject cmGameServiceControllerAndroid = new AndroidJavaObject("com.CloudMacaca.AndroidNative.CMAndroidPlayServiceController");
#endif
        #region Permission
        public enum ATTrackingManagerAuthorizationStatus
        {
            ATTrackingManagerAuthorizationStatusNotDetermined = 0,
            ATTrackingManagerAuthorizationStatusRestricted,
            ATTrackingManagerAuthorizationStatusDenied,
            ATTrackingManagerAuthorizationStatusAuthorized
        }
        delegate void RequestCallback(int Status);
        static System.Action<ATTrackingManagerAuthorizationStatus> requestCallback;

        [AOT.MonoPInvokeCallback(typeof(RequestCallback))]
        static void CallbackFunction(int status)
        {
            if (requestCallback != null)
            {
                requestCallback((ATTrackingManagerAuthorizationStatus)status);
            }
        }
        public static void RequestIDFA(System.Action<ATTrackingManagerAuthorizationStatus> OnComplete)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_IOS
            requestCallback = OnComplete;
            doSetRequestIDFACallback(CallbackFunction);
            doRequestIDFA();
#endif
#if UNITY_ANDROID
           return;
#endif
        }

        public enum AndroidPermission
        {
            ACCESS_COARSE_LOCATION,
            ACCESS_FINE_LOCATION,
            ADD_VOICEMAIL,
            BODY_SENSORS,
            CALL_PHONE,
            CAMERA,
            GET_ACCOUNTS,
            PROCESS_OUTGOING_CALLS,
            READ_CALENDAR,
            READ_CALL_LOG,
            READ_CONTACTS,
            READ_EXTERNAL_STORAGE,
            READ_PHONE_STATE,
            READ_SMS,
            RECEIVE_MMS,
            RECEIVE_SMS,
            RECEIVE_WAP_PUSH,
            RECORD_AUDIO,
            SEND_SMS,
            USE_SIP,
            WRITE_CALENDAR,
            WRITE_CALL_LOG,
            WRITE_CONTACTS,
            WRITE_EXTERNAL_STORAGE
        }

        public enum AndroidPermissionState
        {
            PERMISSION_GRANTED = 0,
            PERMISSION_DENIED = 1,
            PERMISSION_DENIED_NEVER_ASK = 2
        }
        const string PP_PERMISSION_REQUEST_TIME = "PP_PERMISSION_REQUEST_TIME";
        static int PERMISSION_REQUEST_TIME
        {
            get
            {
                return PlayerPrefs.GetInt(PP_PERMISSION_REQUEST_TIME);
            }
            set
            {
                PlayerPrefs.SetInt(PP_PERMISSION_REQUEST_TIME, value);
            }
        }

        /// <summary> Request a Runtime Permission 
        /// <param name="permission">The permission wish to Request</param>
        /// </summary>
        public static void RequestPermission(AndroidPermission permission)
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_IOS
            return;
#endif
#if UNITY_ANDROID
            PERMISSION_REQUEST_TIME++;
            cmAPIControllerAndroid.CallStatic("CM_RequestPermission", "android.permission." + permission.ToString());
#endif
        }

        /// <summary> CheckPermissionState a Runtime Permission Status
        /// <param name="permission">The permission wish to check</param>
        /// </summary>
        /// <value><c>AndroidPermissionState.PERMISSION_GRANTED</c> the permission is Granted <c>AndroidPermissionState.PERMISSION_DENIED</c>the permission is denied <c>AndroidPermissionState.PERMISSION_DENIED_NEVER_ASK</c>the permission is denied due to never ask</value>
        public static AndroidPermissionState CheckPermissionState(AndroidPermission permission)
        {
#if UNITY_EDITOR
            return AndroidPermissionState.PERMISSION_GRANTED;
#endif
#if UNITY_IOS
            return AndroidPermissionState.PERMISSION_GRANTED;
#endif
#if UNITY_ANDROID
            if (PERMISSION_REQUEST_TIME <= 0)
                return AndroidPermissionState.PERMISSION_DENIED;
            else
                return (AndroidPermissionState)cmAPIControllerAndroid.CallStatic<int>("CM_PermissionState", "android.permission." + permission.ToString());
#endif
        }

        /// <summary> Check app have a Permission or not
        /// <param name="permission">The permission wish to check</param>
        /// </summary>
        /// <value><c>true</c> The app have the permission otherwise, <c>false</c>.</value>
        public static bool CheckPermission(AndroidPermission permission)
        {
#if UNITY_EDITOR
            return true;
#endif
#if UNITY_IOS
            return true;
#endif
#if UNITY_ANDROID
            return cmAPIControllerAndroid.CallStatic<bool>("CM_HasPermission", "android.permission." + permission.ToString());
#endif
        }
        /// <summary>
        ///  Check the use have Set "Don't ask again" option
        /// </summary>
        /// <param name="permission">The permission wish to check</param>
        /// <returns> false, if the user have check the "Don't ask again" otherwise, true</returns>
        public static bool CheckPermissionNeedShowDialog(AndroidPermission permission)
        {
#if UNITY_EDITOR
            return true;
#endif
#if UNITY_IOS
            return true;
#endif
#if UNITY_ANDROID
            return cmAPIControllerAndroid.CallStatic<bool>("CM_PermissionNeedShowDialog", "android.permission." + permission.ToString());
#endif
        }

        #endregion
        #region Vibration

        public static bool isVibrationEnable = true;

        /// <summary>
        /// Vibrate device, use iOS Peek preset(iOS 9 or above), While Android vibrate 20 mileseconds 
        /// </summary>
        public static void VibrationAsPeek()
        {
            if (isVibrationEnable == false)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            Vibration(20);
            return;
#endif
#if UNITY_IOS
            doCMVibrationAsPeek();
            return;
#endif
            throw new System.NotSupportedException("This method only support iOS");
        }
        /// <summary>
        /// Vibrate device, use iOS Pop preset(iOS 9 or above), While Android vibrate 40 mileseconds
        /// </summary>
        public static void VibrationAsPop()
        {
            if (isVibrationEnable == false)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            Vibration(40);
            return;
#endif
#if UNITY_IOS
            doCMVibrationAsPop();
            return;
#endif
            throw new System.NotSupportedException("This method only support iOS");
        }
        /// <summary>
        /// Vibrate device, use iOS Nope preset(iOS 9 or above), While Android vibrate 200 mileseconds
        /// </summary>
        public static void VibrationAsNope()
        {
            if (isVibrationEnable == false)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            Vibration(200);
            return;
#endif
#if UNITY_IOS
            doCMVibrationAsNope();
            return;
#endif
            throw new System.NotSupportedException("This method only support iOS");
        }
        /// <summary>
        /// Vibrate on Android device with given seconds. Do nothing on iOS.
        /// </summary>
        /// <param name="milleSecond">The time wish to vibrate</param>
        public static void Vibration(int milleSecond)
        {
            if (isVibrationEnable == false)
            {
                return;
            }
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_Vibration", milleSecond);
            return;
#endif
            throw new System.NotSupportedException("This method only support Android");
        }

        /// <summary>
        /// Can a device vibrate or not.
        /// </summary>
        /// <returns>True if the device support vibrate otherwise, false.</returns>
        public static bool HasVibrator()
        {
#if UNITY_EDITOR
            return false;
#endif

            if (Application.isMobilePlatform == true)
            {
#if UNITY_IOS
                return true;
#endif
#if UNITY_ANDROID
                return cmAPIControllerAndroid.CallStatic<bool>("CM_HasVibration");
#endif
            }
            return false;

        }
        #endregion
        #region Dialog
        /// <summary>
        /// Show the RateUs Dialog.
        /// </summary>
        /// <param name="msg">The message wish to show on the dialog.</param>
        /// <param name="canelTest">The text on the cancel button.</param>
        /// <param name="rateText">The text on the rate button.</param>
        /// <param name="laterText">The text on the later button.</param>
        /// <param name="AppleStoreId">The AppStore Id of iOS app, only use by the device which iOS version is lower than iOS 9</param>
        public static void ShowRateUsDialog(string msg, string canelTest, string rateText, string laterText, string AppleStoreId)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_IOS
            doCMShowRateUsDialog(msg, canelTest, rateText, laterText, AppleStoreId);
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_ShowRateUsDialog", msg, canelTest, rateText, laterText);
#endif
        }

        /// <summary>
        /// Show a Dialog
        /// </summary>
        /// <param name="title">The title of dialog</param>
        /// <param name="msg">The message of dialog</param>
        /// <param name="positiveBtnText">The text on the button</param>
        /// <param name="callback">Only positive Action work on this API</param>
        public static void ShowAlertDialog(string title, string msg, string positiveBtnText, CMDialogCallback callback)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorUtility.DisplayDialog(
                title,
                msg,
                positiveBtnText))
            {
                callback.onPositiveButtonClick();
                return;
            }
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_AlertDialog", title, msg, positiveBtnText, callback);
            return;
#endif
#if UNITY_IOS
            PositiveCallback = callback.onPositiveButtonClick;
            NegativeCallback = callback.onNegativeButtonClick;
            doSetAlertDialogCallback(AlertDialogCallbackFunction);
            doCMShowAlertDialog(title, msg, positiveBtnText);
            return;
#endif
            throw new System.NotSupportedException("This method only support iOS");
        }
        /// <summary>
        /// Show a Dialog with callback
        /// </summary>
        /// <param name="title">The title of dialog</param>
        /// <param name="msg">The message of dialog</param>
        /// <param name="positiveBtnText">The text on the positive button</param>
        /// <param name="negativeBtnText">The text on the negative button</param>
        /// <param name="callback">A callback will fire after the dialog is closed.</param>
        public static void ShowAlertDialog(string title, string msg, string positiveBtnText, string negativeBtnText, CMDialogCallback callback)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorUtility.DisplayDialog(
                title,
                msg,
                positiveBtnText,
                negativeBtnText))
            {
                callback.onPositiveButtonClick();
                return;
            }
            else
            {
                callback.onNegativeButtonClick();
                return;
            }
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_AlertDialogWithCallback", title, msg, positiveBtnText, negativeBtnText, callback);
            return;
#endif
#if UNITY_IOS

            PositiveCallback = callback.onPositiveButtonClick;
            NegativeCallback = callback.onNegativeButtonClick;
            doSetAlertDialogCallback(AlertDialogCallbackFunction);
            doCMShowAlertDialogWithCallback(title, msg, positiveBtnText, negativeBtnText);
            return;
#endif
        }

        /// <summary>
        /// Show a native date picker.
        /// </summary>
        /// <param name="okText">The text on ok button (Only work on iOS)</param>
        /// <param name="cancelText">The text on cancel button (Only work on iOS)</param>
        /// <param name="callback">The callback if user has set a date and click the ok button</param>
        public static void ShowDatePickerWithCallback(string okText, string cancelText, CMDatePickerCallback callback)
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_DatePickerWithCallback", callback);
            return;
#endif
#if UNITY_IOS

            DatePickerOnResultCallback = callback._OnResult;
            doSetDatePickerCallback(DatePickerDialogCallbackFunction);
            doCMShowDatePickerWithCallback(okText, cancelText);
            return;
#endif
        }


        delegate void DatePickerDialogCallback(string date);
        static System.Action<string> DatePickerOnResultCallback;

        [AOT.MonoPInvokeCallback(typeof(DatePickerDialogCallback))]
        static void DatePickerDialogCallbackFunction(string date)
        {
            if (DatePickerOnResultCallback != null)
            {
                DatePickerOnResultCallback(date);
            }
        }

        delegate void AlertDialogCallback(int clickType);
        static System.Action PositiveCallback;
        static System.Action NegativeCallback;

        [AOT.MonoPInvokeCallback(typeof(AlertDialogCallback))]
        static void AlertDialogCallbackFunction(int clickType)
        {
            if (clickType == 0)
            {
                if (PositiveCallback != null)
                {
                    PositiveCallback();
                }
            }
            else
            {
                if (NegativeCallback != null)
                {
                    NegativeCallback();
                }
            }
        }

        /// <summary>
        /// Show a toast message
        /// </summary>
        /// <param name="msg">the message wish to show</param>
        public static void ShowToastMessage(string msg)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_IOS
            doCMShowToastMessage(msg);
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_ShowToastMessage", msg);
#endif
        }

        /// <summary>
        /// Show a Android Native like SnackBar, note: this snackbar is made by UnityGUI
        /// </summary>
        /// <param name="msg">The message on snackbar</param>
        /// <param name="btnString">The text on snackbar's button</param>
        /// <param name="id">Give the snackbar with an id</param>
        public static void ShowSnackBar(string msg, string btnString, string id)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_IOS
            return;
#endif
#if UNITY_ANDROID
            cmAPIControllerAndroid.CallStatic("CM_ShowSnackBar", msg, btnString, id);
#endif
        }
        #endregion

        #region Share

        public enum ShareType
        {
            Image = 0, GIF = 1, Viedo = 2
        }

        /// <summary>
        /// Show native share dialog
        /// </summary>
        /// <param name="mediaPath">The path of a media, image, gif ,etc.</param>
        /// <param name="subject">The title in share , may not show if also share a media</param>
        /// <param name="message">The message in share , may not show if also share a media</param>
        /// <param name="shareType">Define the media type, image, gif of video</param>
        public static void Share(string mediaPath, string subject, string message, ShareType shareType)
        {
            string authority = Application.identifier;
            if (mediaPath == null)
                mediaPath = string.Empty;

            if (subject == null)
                subject = string.Empty;

            if (message == null)
                message = string.Empty;

            if (mediaPath.Length > 0 && !File.Exists(mediaPath))
                throw new FileNotFoundException("File not found at " + mediaPath);

#if UNITY_EDITOR
            byte[] fileData;
            Texture2D texture = null;
            if (File.Exists(mediaPath))
            {
                fileData = File.ReadAllBytes(mediaPath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            if (texture == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("Select Texture", "You must select a texture first!", "OK");
                return;
            }

            var path = UnityEditor.EditorUtility.SaveFilePanel(
              "Save texture as PNG",
              "",
              texture.name + ".png",
              "png");
            if (path.Length != 0)
            {
                var pngData = texture.EncodeToPNG();
                if (pngData != null)
                    File.WriteAllBytes(path, pngData);
            }

            return;
#endif
#if UNITY_ANDROID
            if (authority == null || authority.Length == 0)
                throw new System.ArgumentException("Parameter 'authority' is null or empty!");

            cmAPIControllerAndroid.CallStatic("CM_NativeShare", mediaPath, subject, message, authority, (int)shareType);
#endif
#if UNITY_IOS
            doCMShowShare(mediaPath, subject, message, (int)shareType);
#endif
        }

        public static void Share(string subject, string message)
        {
            string mediaPath = string.Empty;
            string authority = Application.identifier;
            if (subject == null)
                subject = string.Empty;

            if (message == null)
                message = string.Empty;
#if UNITY_ANDROID
            if (authority == null || authority.Length == 0)
                throw new System.ArgumentException("Parameter 'authority' is null or empty!");

            cmAPIControllerAndroid.CallStatic("CM_NativeShare", mediaPath, subject, message, authority, 0);
#endif
#if UNITY_IOS
            doCMShowShare("", subject, message, 0);
#endif
        }

        #endregion

        #region GameService
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]
        public static void GameServiceInit()
        {
#if UNITY_EDITOR

            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("Init");
#elif UNITY_IOS
            doGameCenterInit();
#else
            Debug.Log("No Support for this platform");
#endif
        }
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]
        public static void GameServiceSignIn()
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            //cmGameServiceControllerAndroid.CallStatic("StartSignInIntent");
            Debug.Log("Android Play service after 11.8 will auto signin while Init()");
#elif UNITY_IOS
            Debug.Log("iOS GameCenter will auto signin while Init()");
#else
            Debug.Log("No Support for this platform");
#endif
        }

        public static void GameServiceSignOut()
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("SignOut");
#elif UNITY_IOS
            Debug.Log("iOS GameCenter not support Signout()");
#else
            Debug.Log("No Support for this platform");
#endif
        }
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]

        public static void GameServiceShowAchievementView()
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("ShowAchievements");
#elif UNITY_IOS
            doShowAchievement();
#else
            Debug.Log("No Support for this platform");
#endif
        }
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]

        public static void GameServiceShowLeaderboardView()
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("ShowLeaderboard");
#elif UNITY_IOS
            doShowLeaderboard();
#else
            Debug.Log("No Support for this platform");
#endif
        }
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]

        public static void GameServiceUnlockAchievement(string achievementId)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("UnlockAchievement", achievementId);
#elif UNITY_IOS
            doUnlockAchievent(achievementId);
#else
            Debug.Log("No Support for this platform");
#endif
        }
        [Obsolete("GameService Support in CloudMacacaAPI will no longer update ever, please use Unity Social API (For Play Service you need import play-games-plugin-for-unity)")]

        public static void GameServiceUploadScore(string leaderboardId, int score)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID
            cmGameServiceControllerAndroid.CallStatic("UploadNewScore", leaderboardId, score);
#elif UNITY_IOS
            doUploadLeaderboard(leaderboardId, score);
#else
            Debug.Log("No Support for this platform");
#endif
        }
        #endregion

        #region Push(Local)
        //See CM_LocalNotificationController.cs 
        #endregion
    }



}
