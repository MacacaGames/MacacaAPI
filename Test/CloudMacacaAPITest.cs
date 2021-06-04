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
        CM_APIController.ShowRateUsDialog();
    }
    public void OnShareClick()
    {
        ShareScreenshotWithText("test");
    }
    public void OnAlertDialogClick()
    {
        //CM_APIController.ShowAlertDialog("title", "msg", "ok");
    }

    public string ScreenshotName = "screenshot.png";

    public void ShareScreenshotWithText(string text)
    {
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        if (File.Exists(screenShotPath)) File.Delete(screenShotPath);

        ScreenCapture.CaptureScreenshot(ScreenshotName);
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



    public void ShowAlertDialogWithCallback()
    {
        CM_APIController.ShowAlertDialog(
            "title",
            "msg",
            "Ok",
            "No",
            new CMDialogCallback(
                () =>
                {
                    CM_APIController.ShowToastMessage("Positive");
                },
                () =>
                {
                    CM_APIController.ShowToastMessage("Negative");
                }
            )
        );
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
    }
}
