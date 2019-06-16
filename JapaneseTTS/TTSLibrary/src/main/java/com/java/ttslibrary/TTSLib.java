package com.java.ttslibrary;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.app.Activity;

import java.util.HashMap;
import java.util.Locale;
import java.util.Set;

import android.speech.tts.TextToSpeech;
import android.util.Log;
import android.media.AudioManager;
import java.util.Locale;

public class TTSLib {
    static TextToSpeech tts;
    static Locale _locale = Locale.JAPANESE;
    static String packagetouse = "com.google.android.tts";

    static Bundle bundle = null;
    static HashMap<String, String> param;
    static Locale.Builder builder = new Locale.Builder();

    public static boolean IsRunning(){
        return (tts!=null);
    }

    public static void SetPitch(float pitch){
        if(tts!=null){
            tts.setPitch(pitch);
        }
    }

    public static void SetSpeedRate(float speechrate){
        if(tts!=null){
            tts.setSpeechRate(speechrate);
        }
    }

    public static boolean SetLocale(String locale, String script, String region){
        Locale locale_ = builder.setLanguage(locale).setScript(script).setRegion(region).build();
        if(tts==null){
            return false;
        }
        int result = tts.setLanguage(locale_);
        if (result == TextToSpeech.LANG_MISSING_DATA|| result == TextToSpeech.LANG_NOT_SUPPORTED) {
            return false;
        }
        _locale = locale_;
        return true;
    }

    public static String GetLocale(){
        return (tts!=null) ? tts.getLanguage().toString() : "";
    }

    public static String[] GetAvailableLocales(){

        /*if(instance.tts!=null){
            Set<Locale> locales = instance.tts.getAvailableLanguages();
            String[] temp = new String[locales.size()];
            Locale[] _locales = locales.toArray(new Locale[0]);
            for(int i=0;i<locales.size();i++){
                temp[i]=_locales[i].toString();
            }

            return temp;
        }

        return new String[]{};*/
        Locale[] locales = Locale.getAvailableLocales();
        String[] temp = new String[locales.length];

        for(int i=0;i<locales.length;i++){
            temp[i] = locales[i].toString();
        }

        return temp;
    }

    public static boolean setEngineByPackageName(String packagename){

        if(tts!=null && tts.setEngineByPackageName(packagename)==TextToSpeech.SUCCESS){
            return true;
        }
        return false;
    }


    public static void StartTTS(final Activity activity)
    {
        activity.setVolumeControlStream(AudioManager.STREAM_MUSIC);

        tts = new TextToSpeech(activity.getApplicationContext(), new TextToSpeech.OnInitListener() {
            @Override
            public void onInit(int status) {
                if (status == TextToSpeech.SUCCESS) {

                    int result = tts.setLanguage(Locale.JAPANESE);
                    if (result == TextToSpeech.LANG_MISSING_DATA
                            || result == TextToSpeech.LANG_NOT_SUPPORTED) {
                        Log.e("error", "Language Not supported");
                        DownloadTTSData(activity);
                    }
                    else{
                        Log.v("TTS","onInit succeeded");
                    }
                } else {
                    Log.e("error","Initialization Failed");
                }

            }
        },packagetouse);
    }

    public static void DownloadTTSData(Activity activity) {
        Intent installTTSIntent = new Intent();
        installTTSIntent.setAction(TextToSpeech.Engine.ACTION_INSTALL_TTS_DATA);
        activity.startActivity(installTTSIntent);
    }

    public static boolean IsUttering() {
        if(tts!=null && tts.isSpeaking()){
            return true;
        }
        return false;
    }

    public static void StopUtterance(){
        if(tts!=null){
            tts.stop();
        }
    }

    public static boolean GetLanguageAvailability(Activity activity, String locale) {
        int result = tts.isLanguageAvailable(builder.setLanguage(locale).build());

        if(!(result == TextToSpeech.LANG_MISSING_DATA || result == TextToSpeech.LANG_NOT_SUPPORTED)) {
            return true;
        }
        else {
            //DownloadTTSData(activity);
            return false;
        }
    }

    public static void Speak(String s){

        //Log.e("error","SpeakJapanese Is Running normally: " + tts.toString());
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            //Log.v("Change to new API", "Speak new API");
            if(bundle==null){
                Bundle bundle = new Bundle();
                bundle.putInt(TextToSpeech.Engine.KEY_PARAM_STREAM, AudioManager.STREAM_MUSIC);
            }
            tts.speak(s, TextToSpeech.QUEUE_FLUSH, bundle, null);
        } else {
            //Log.v(s, "Speak old API");
            if(param==null){
                param = new HashMap<>();
                param.put(TextToSpeech.Engine.KEY_PARAM_STREAM, String.valueOf(AudioManager.STREAM_MUSIC));
            }

            tts.speak(s, TextToSpeech.QUEUE_FLUSH, param);
        }
    }

    static protected void onDestroy() {
        // Don't forget to shutdown tts!
        if (tts != null) {
            Log.v("onDestroy()","onDestroy: shutdown TTS");
            tts.stop();
            tts.shutdown();
        }
    }

    public static void StopTTS() {
        onDestroy();
    }
}
