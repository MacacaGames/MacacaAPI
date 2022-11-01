package com.CloudMacaca.AndroidNative;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.net.Uri;
import android.speech.tts.UtteranceProgressListener;
import android.util.Log;
import android.view.View;
import android.widget.DatePicker;
import android.widget.Toast;
import android.os.Build;
import android.os.Bundle;

//import com.google.android.play.core.review.ReviewInfo;
//import com.google.android.play.core.review.ReviewManager;
//import com.google.android.play.core.review.ReviewManagerFactory;
//import com.google.android.play.core.tasks.OnCompleteListener;
//import com.google.android.play.core.tasks.Task;
import com.unity3d.player.UnityPlayer;
import android.content.Context;


import java.io.File;
import java.util.ArrayList;
import java.util.Calendar;

//import static androidx.core.app.ActivityCompat.shouldShowRequestPermissionRationale;
//import static androidx.core.content.ContextCompat.checkSelfPermission;


/**
 * Created by mikisahika on 2017/9/3.
 */

public class CMAndroidController {
    public static final int GRANTED = 0;
    public static final int DENIED = 1;
    public static final int BLOCKED_OR_NEVER_ASKED = 2;

    // Needed to get the battery level.
    public static Activity m_Activity;

    public CMAndroidController(Activity activity){
        m_Activity = activity;
//        PrepareReview(m_Activity);
    }

    public CMAndroidController(){
        m_Activity = UnityPlayer.currentActivity;
//        PrepareReview(m_Activity);
    }


    public static void SendMessageToUnity(String MethodName,String Parameters){
        UnityPlayer.UnitySendMessage("CMAPI_Callback", MethodName, Parameters);
    }
    //Permission

    public static void CM_RequestPermission(String permissionStr) {
        Bundle bundle = new Bundle();
        Intent intent =  new Intent(CMAndroidController.m_Activity, AndroidPermissionActivity.class);
        bundle.putString("Per", permissionStr);
        intent.putExtras(bundle);
        m_Activity.startActivity(intent);
    }

    public static int CM_PermissionState(String permissionStr) {
        if(Build.VERSION.SDK_INT < 23) {
            return GRANTED;
        }
        if(m_Activity.checkSelfPermission( permissionStr) != PackageManager.PERMISSION_GRANTED) {
            if(!m_Activity.shouldShowRequestPermissionRationale( permissionStr)){
                return BLOCKED_OR_NEVER_ASKED;
            }
            return DENIED;
        }
        return GRANTED;
    }

    public static boolean CM_HasPermission(String permissionStr) {
        if(Build.VERSION.SDK_INT < 23) {
            return true;
        }
        Context context = m_Activity.getApplicationContext();
        return context.checkCallingOrSelfPermission(permissionStr) == PackageManager.PERMISSION_GRANTED;
    }

    public static boolean CM_PermissionNeedShowDialog(String permissionStr) {
        if(Build.VERSION.SDK_INT < 23) {
            return true;
        }
        return m_Activity.shouldShowRequestPermissionRationale(permissionStr);
    }

    //Permission



    public static void CM_ShowToastMessage(final String msg){
        m_Activity.runOnUiThread(new Runnable() {
            public void run() {
                Toast.makeText(m_Activity, msg , Toast.LENGTH_SHORT).show();
            }
        });
    }

    static String HasRateKey = "PP_Rate";
    static String LastShowRateDate = "PP_Rate_Date";
    public static void CM_ShowRateUsDialog(){

    }


    static void OldVersionRateUs(String mag,String CancelText,String RateText,String LaterText){
        final String hasRate = Utils.getPString(m_Activity,HasRateKey);

        if(hasRate == "Yes"){
            Log.d("Rate","Has Rate");

            return;
        }

        //Get App name First
        PackageManager packageManager = m_Activity.getPackageManager();
        ApplicationInfo applicationInfo = null;
        try {
            applicationInfo = packageManager.getApplicationInfo(m_Activity.getApplicationInfo().packageName, 0);
        } catch (final PackageManager.NameNotFoundException e) {
        }
        String appName = (String) (applicationInfo != null ? packageManager.getApplicationLabel(applicationInfo) : "Unknown");

        //Make a Dialog View
        new AlertDialog.Builder(m_Activity)
                .setTitle(appName)
                .setMessage(mag)
                .setNeutralButton(RateText, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        try{
                            Intent rateIntent =new Intent(Intent.ACTION_VIEW, Uri.parse("market://details?id=" + m_Activity.getPackageName()));
                            rateIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                            m_Activity.startActivity(rateIntent);
                        }
                        catch(Exception ex){
                            Intent rateIntent =new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=" + m_Activity.getPackageName()));
                            rateIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                            m_Activity.startActivity(rateIntent);
                        }
                        Utils.setPString(m_Activity,hasRate,"Yes");
                    }
                })
                .setNegativeButton(CancelText, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        //Toast.makeText(getApplicationContext(), R.string.i_am_hungry, Toast.LENGTH_SHORT).show();
                    }
                })
//                .setPositiveButton(LaterText, new DialogInterface.OnClickListener() {
//                    @Override
//                    public void onClick(DialogInterface dialog, int which) {
//                        //Toast.makeText(getApplicationContext(), R.string.diet, Toast.LENGTH_SHORT).show();
//                        Utils.setPLong(m_Activity,"lastRate", Utils.GetTimeStamp());
//                    }
//                })
                .show();
    }

    public static void CM_AlertDialog(String title,String msg,String OkBtnText, final CMDialogCallback callback){
        AlertDialog.Builder dialog =  new AlertDialog.Builder(m_Activity).setTitle(title).setMessage(msg).setCancelable(false);

        dialog.setPositiveButton(OkBtnText, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                callback.onPositiveButtonClick();
            }
        });
        dialog.show();
    }

    public static void CM_AlertDialogWithCallback(String title, String msg, String PositiveBtnText, String NegativeBtnText, final CMDialogCallback callback){
        AlertDialog.Builder dialog =  new AlertDialog.Builder(m_Activity).setTitle(title).setMessage(msg);
        Log.e("CM API","CM_AlertDialogWithCallback");
        dialog.setPositiveButton(PositiveBtnText, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                callback.onPositiveButtonClick();
            }
        }).setNegativeButton(NegativeBtnText, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                callback.onNegativeButtonClick();
            }
        });
        dialog.show();
    }

    public static void CM_DatePickerWithCallback(final CMDatePickerCallback callback){
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);
        new DatePickerDialog(m_Activity, new DatePickerDialog.OnDateSetListener() {
            @Override
            public void onDateSet(DatePicker view, int year, int month, int day) {
                String dateTime = String.valueOf(year)+"/"+String.valueOf(month+1)+"/"+String.valueOf(day);
                callback.onResultClick(dateTime);
            }
        }, year, month, day).show();
    }

    public static void CM_Vibration(int millisec){
        CMAndroidVibrationController.getInstance().vibrate(millisec);
    }

    public static void CM_CancelVibration(){

        CMAndroidVibrationController.getInstance().cancel();
    }

    public static boolean CM_HasVibration(){
        return CMAndroidVibrationController.getInstance().hasVibrator();
    }

    private static String authority = null;

    public static void CM_NativeShare(String mediaPath, String subject, String message, String authority, int typeId )
    {
        Intent intent = new Intent( Intent.ACTION_SEND );

        if( subject != null && subject.length() > 0 )
            intent.putExtra( Intent.EXTRA_SUBJECT, subject );

        if( message != null && message.length() > 0 )
            intent.putExtra( Intent.EXTRA_TEXT, message );

        String mimeType;
        if( mediaPath != null && mediaPath.length() > 0 )
        {
            Uri contentUri = UnityShareContentProvider.getUriForFile( m_Activity, authority, new File( mediaPath ) );
            intent.putExtra( Intent.EXTRA_STREAM, contentUri );
            //0 image
            //1 gif
            //2 video
            if( typeId == 0 || typeId == 1 )
                mimeType = "image/*";
            else
                mimeType = "video/mp4";
        }
        else
            mimeType = "text/plain";

        intent.setType( mimeType );
        intent.setFlags( Intent.FLAG_GRANT_READ_URI_PERMISSION );

        m_Activity.startActivity( Intent.createChooser( intent, "" ) );
    }

}
