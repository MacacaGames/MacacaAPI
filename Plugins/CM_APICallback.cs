using UnityEngine;
using System;

namespace MacacaGames
{
#if UNITY_EDITOR
    public class CMDialogCallback
#elif UNITY_ANDROID
    public class CMDialogCallback : AndroidJavaProxy
#elif UNITY_IOS
    public class CMDialogCallback
#else
    public class CMDialogCallback 
#endif
    {
        Action _OnPositive;
        Action _OnNegative;
#if UNITY_EDITOR
        public CMDialogCallback(Action OnPositive, Action OnNegative)
#elif UNITY_ANDROID
        public CMDialogCallback(Action OnPositive, Action OnNegative) : base("com.CloudMacaca.AndroidNative.CMDialogCallback")
#elif UNITY_IOS
        public CMDialogCallback(Action OnPositive, Action OnNegative) 
#else
        public CMDialogCallback(Action OnPositive, Action OnNegative)
#endif
        {
            _OnPositive = OnPositive;
            _OnNegative = OnNegative;
        }

        public void onPositiveButtonClick()
        {
            _OnPositive();
        }
        public void onNegativeButtonClick()
        {
            _OnNegative();
        }
    }
#if UNITY_ANDROID
    public class CMDatePickerCallback : AndroidJavaProxy
#elif UNITY_IOS
    public class CMDatePickerCallback
#else
    public class CMDatePickerCallback
#endif
    {
        public Action<string> _OnResult;
        public void onResultClick(string date){
            _OnResult(date);
        }
#if UNITY_ANDROID
        public CMDatePickerCallback(Action<string> OnResult) : base("com.CloudMacaca.AndroidNative.CMDatePickerCallback")
#elif UNITY_IOS
        public CMDatePickerCallback(Action<string> OnResult) 
#else
        public CMDatePickerCallback(Action<string> OnResult) 
#endif
        {
            _OnResult = OnResult;
        }
    }
}

