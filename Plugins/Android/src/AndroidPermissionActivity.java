package com.CloudMacaca.AndroidNative;

import android.annotation.TargetApi;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.util.Log;
import android.content.DialogInterface;
import java.lang.annotation.Target;
@TargetApi(23)

public class AndroidPermissionActivity extends Activity {

    Activity activity;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        Bundle b = getIntent().getExtras();
        final String per = b.getString("Per", "");
        Log.d("CM",per);
        activity = this;

        activity.requestPermissions(new String[]{per},0);

    }


    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {

        super.onRequestPermissionsResult(requestCode, permissions, grantResults);

        switch (requestCode) {
            case 0: {
                if(grantResults.length > 0){
                    String result = "";
                    if(permissions.length > 0){
                        result = permissions[0];
                    }

                    if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                        Log.d("CM","OnAllow");
                        CMAndroidController.SendMessageToUnity("OnAllow",result);
                    }
                    else {
                        if(this.shouldShowRequestPermissionRationale(permissions[0])){
                            Log.d("CM","OnDeny");
                            CMAndroidController.SendMessageToUnity("OnDeny",result);
                        } else {
                            Log.d("CM","OnDenyAndNeverAskAgain");

                            CMAndroidController.SendMessageToUnity("OnDenyAndNeverAskAgain",result);
                        }
                    }
                }

                finish();
                break;
            }
        }
    }
}
