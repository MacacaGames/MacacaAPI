package com.CloudMacaca.AndroidNative;

import android.content.Context;
import android.content.SharedPreferences;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Calendar;
import java.util.TimeZone;
import java.util.concurrent.TimeUnit;
/**
 * Created by miki on 2018/1/10.
 */

public class Utils {


    public static String sha256(String input) throws NoSuchAlgorithmException, UnsupportedEncodingException {
        MessageDigest digest = MessageDigest.getInstance("SHA-256");
        digest.reset();

        byte[] byteData = digest.digest(input.getBytes("UTF-8"));
        StringBuffer sb = new StringBuffer();

        for (int i = 0; i < byteData.length; i++) {
            sb.append(Integer.toString((byteData[i] & 0xff) + 0x100, 16).substring(1));
        }
        return sb.toString();
    }

    public static String MD5(String str) throws NoSuchAlgorithmException, UnsupportedEncodingException {
        MessageDigest md5 = MessageDigest.getInstance("MD5");

        char[] charArray = str.toCharArray();
        byte[] byteArray = new byte[charArray.length];

        for(int i = 0; i < charArray.length; i++) {
            byteArray[i] = (byte)charArray[i];
        }

        byte[] md5Bytes = md5.digest(byteArray);

        StringBuffer hexValue = new StringBuffer();

        for( int i = 0; i < md5Bytes.length; i++) {
            int val = ((int)md5Bytes[i])&0xff;
            if(val < 16) {
                hexValue.append("0");
            }
            hexValue.append(Integer.toHexString(val));
        }
        return hexValue.toString();
    }

    public static long GetTimeStamp()
    {
        TimeZone tz = TimeZone.getTimeZone("GMT+8");
        Calendar c = Calendar.getInstance(tz);
        long timeMillis = c.getTimeInMillis();
        //long timeMillis = System.currentTimeMillis();
        return TimeUnit.MILLISECONDS.toSeconds(timeMillis);
    }



    public static String getPString(Context mContext, String key){
        SharedPreferences settings = mContext.getSharedPreferences("CM_API_INFO", 0);

        if (settings == null) {
            return "";
        }

        return settings.getString(key, "");
    }

    public static void setPString(Context mContext,String key, String value){
        SharedPreferences settings = mContext.getSharedPreferences("CM_API_INFO", 0);
        settings.edit().putString(key, value).apply();
    }

    public static Long getPLong(Context mContext,String key){
        SharedPreferences settings = mContext.getSharedPreferences("CM_API_INFO", 0);

        if (settings == null) {
            return Long.valueOf(0);
        }

        return settings.getLong(key,0);
    }

    public static void setPLong(Context mContext,String key, Long value){
        SharedPreferences settings = mContext.getSharedPreferences("CM_API_INFO", 0);
        settings.edit().putLong(key, value).apply();
    }
}
