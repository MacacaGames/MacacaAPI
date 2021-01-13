using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MacacaGames;

public class CloudMacacaAPITest : MonoBehaviour
{

    public void OnPermissionClick()
    {
        CM_APIController.RequestPermission(CM_APIController.AndroidPermission.WRITE_EXTERNAL_STORAGE);

    }
    public void OnPermissionCheckClick()
    {
        Debug.Log(CM_APIController.CheckPermission(CM_APIController.AndroidPermission.WRITE_EXTERNAL_STORAGE));
    }
    public RawImage icon;
    public RawImage bigImage;
    public void OnGetADClick()
    {
        //CM_AdSystem.GetOneAd(abc);
#if UNITY_2017
        // CM_AdSystem.GetOneAd(
        //     (iconTexture, bigImageTexutre) =>
        //     {
        //         icon.texture = iconTexture;
        //         bigImage.texture = bigImageTexutre;
        //     }
        // );
#endif
    }

    // (iconTexture,bigImageTexutre)=>{
    // 			icon.texture = iconTexture;
    // 			bigImage.texture = bigImageTexutre;
    // 		}
    void abc(Texture iconTexture, Texture bigImageTexutre)
    {
        icon.texture = iconTexture;
        bigImage.texture = bigImageTexutre;
    }
    //Test modify
    public void OnVibrationPopClick()
    {
        CM_APIController.VibrationAsPop();
    }
    public void OnVibrationPeekClick()
    {
        CM_APIController.VibrationAsPeek();
    }
    public void OnVibrationNopeClick()
    {
        CM_APIController.VibrationAsNope();
    }
    public void OnVibrationAndroidClick()
    {
        CM_APIController.Vibration(1000);
    }
    public void OnShowToastClick()
    {
        CM_APIController.ShowToastMessage("Hello World");
    }
    public void OnShowRateClick()
    {
        CM_APIController.ShowRateUsDialog("Hello World", "Cancel", "Rate", "Later", "1041808803");
    }
    public void OnShareClick()
    {
        ShareScreenshotWithText("test");
    }
    public void OnAlertDialogClick()
    {
        //CM_APIController.ShowAlertDialog("title", "msg", "ok");
    }

    public void OnGameServiceInitClick()
    {
        CM_APIController.GameServiceInit();
    }
    public void OnGameServiceLoginClick()
    {
        CM_APIController.GameServiceSignIn();
    }
    public void OnShowAchiClick()
    {
        CM_APIController.GameServiceShowAchievementView();
    }
    public void OnShowLeaderClick()
    {
        CM_APIController.GameServiceShowLeaderboardView();
    }

    public void OnUploadScore()
    {
        if (Application.platform == RuntimePlatform.Android)
            CM_APIController.GameServiceUploadScore("CgkIxc2Sz5QREAIQAw", 100);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            CM_APIController.GameServiceUploadScore("testLeaderboard", 100);

    }
    public void OnUnlock()
    {
        if (Application.platform == RuntimePlatform.Android)
            CM_APIController.GameServiceUnlockAchievement("CgkIxc2Sz5QREAIQBA");
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            CM_APIController.GameServiceUnlockAchievement("testAchievement");
    }
    public string ScreenshotName = "screenshot.png";

    public void ShareScreenshotWithText(string text)
    {
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        if (File.Exists(screenShotPath)) File.Delete(screenShotPath);

        ///        ScreenCapture.CaptureScreenshot(ScreenshotName);

        StartCoroutine(delayedShare(screenShotPath, text));
    }

    //CaptureScreenshot runs asynchronously, so you'll need to either capture the screenshot early and wait a fixed time
    //for it to save, or set a unique image name and check if the file has been created yet before sharing.
    IEnumerator delayedShare(string screenShotPath, string text)
    {
        while (!File.Exists(screenShotPath))
        {
            yield return new WaitForSeconds(.05f);
        }
        CM_APIController.Share(screenShotPath, text, text, CM_APIController.ShareType.Image);

        //CM_APIController.Share(text, screenShotPath, "", "", "image/png", true, "");
    }



    public void OneTimePush()
    {
        CM_LocalNotificationController.SendNotification(1, 5000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }

    public void OneTimeBigIconPush()
    {
        CM_LocalNotificationController.SendNotification(1, 5000, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
    }

    public void OneTimeWithActionsPush()
    {
        CM_LocalNotificationController.Action action1 = new CM_LocalNotificationController.Action("background", "In Background", this);
        action1.Foreground = false;
        CM_LocalNotificationController.Action action2 = new CM_LocalNotificationController.Action("foreground", "In Foreground", this);
        CM_LocalNotificationController.SendNotification(1, 5000, "Title", "Long message text with actions", new Color32(0xff, 0x44, 0x44, 255), true, true, true, null, "boing", "default", action1, action2);
    }

    public void RepeatingPush()
    {
        CM_LocalNotificationController.SendRepeatingNotification(1, 5000, 60000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }

    public void StopPush()
    {
        CM_LocalNotificationController.CancelNotification(1);
    }

    public void ShowAlertDialogWithCallback()
    {
        // CM_APIController.ShowAlertDialogWithCallback(
        //     "title",
        //     "msg",
        //     "Ok",
        //     "No",
        //     new CMDialogCallback(
        //         () =>
        //         {
        //             CM_APIController.ShowToastMessage("Positive");
        //         },
        //         () =>
        //         {
        //             CM_APIController.ShowToastMessage("Negative");
        //         }
        //     )
        // );
    }

    public void ShowDatePickerWithCallback()
    {
        CM_APIController.ShowDatePickerWithCallback(
            "Ok",
            "No",
            new CMDatePickerCallback(
                (date) =>
                {
                    CM_APIController.ShowToastMessage(date);
                }
            )
        );
    }

    //Uinty LifeCycle
    void Awake()
    {
        CM_LocalNotificationController.ClearNotifications();
    }
}
