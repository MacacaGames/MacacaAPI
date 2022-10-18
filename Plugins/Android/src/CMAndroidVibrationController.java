package com.CloudMacaca.AndroidNative;
import android.annotation.SuppressLint;
import android.content.Context;
import android.os.Build;
import android.os.Vibrator;
/**
 * Created by mikisahika on 2017/9/3.
 */

public class CMAndroidVibrationController {

    private Vibrator mVibrator = null;

    private static final CMAndroidVibrationController instance = new CMAndroidVibrationController();

    public static CMAndroidVibrationController getInstance() {
        return instance;
    }

    public CMAndroidVibrationController()
    {
        this.mVibrator = ((Vibrator) CMAndroidController.m_Activity.getSystemService(Context.VIBRATOR_SERVICE));
    }



    @SuppressLint({"NewApi"})
    public boolean hasVibrator()
    {
        boolean result = this.mVibrator != null;
        if ((result) && (Build.VERSION.SDK_INT >= 11)) {
            result = (result) && (this.mVibrator.hasVibrator());
        }
        return result;
    }

    public void vibrate(int milliseconds)
    {
        if (!hasVibrator()) {
            return;
        }
        this.mVibrator.vibrate(milliseconds);
    }

    public void vibrate(long[] pattern, boolean loop)
    {
        if (!hasVibrator()) {
            return;
        }
        int repeat = loop ? 0 : -1;
        vibrate(pattern, repeat);
    }

    public void vibrate(long[] pattern, int repeat)
    {
        if (!hasVibrator()) {
            return;
        }
        this.mVibrator.vibrate(pattern, repeat);
    }

    public void cancel()
    {
        if (!hasVibrator()) {
            return;
        }
        this.mVibrator.cancel();
    }
}
