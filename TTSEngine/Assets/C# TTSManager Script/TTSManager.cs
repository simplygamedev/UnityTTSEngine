using UnityEngine;

public static class TTSManager {

    private static AndroidJavaClass _mainactivity = null;
    private static AndroidJavaClass _libraryclass = null;

    const string _MainActivity = "com.unity3d.player.UnityPlayer";
    const string _LibraryClass = "com.java.ttslibrary.TTSLib";
    const string _MethodName = "Speak"; 
    const string _CheckAvailability = "GetLanguageAvailability"; 
    const string _getlocale = "GetLocale";
    const string _getavailablelocales = "GetAvailableLocales";
    const string _setlocale = "SetLocale";
    const string _setEngineByPackageName = "setEngineByPackageName"; 
    const string _downloadttsdata = "DownloadTTSData"; 
    const string _shutdowntts = "StopTTS"; 
    const string _uttering = "IsUttering"; 
    const string _setpitch = "SetPitch"; 
    const string _setspeed = "SetSpeedRate"; 
    const string _isbootedup = "IsRunning"; 
    const string _stopspeaking = "StopUtterance";
    const string _starttts = "StartTTS";

    public static void BootUpTTS() {
        #if UNITY_ANDROID && !UNITY_EDITOR
            _mainactivity = new AndroidJavaClass(_MainActivity);
            _libraryclass = new AndroidJavaClass(_LibraryClass);
            _libraryclass.CallStatic(_starttts, _mainactivity.GetStatic<AndroidJavaObject>("currentActivity"));
            SetSpeechRate(0.8f);
        #endif
    }

    public static void SetSpeechRate(float value) { // Sets the rate at which the TTS Engine speaks the words
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_libraryclass!=null) {
            _libraryclass.CallStatic(_setspeed, value);
        }
        #endif
    }

    public static void SetPitch(float value) { // Sets the pitch of the TTS Engine
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_libraryclass!=null) {
            _libraryclass.CallStatic(_setpitch, value);
        }
        #endif
    }

    public static bool GetLanguageAvailability(string locale) { //Java method call that checks to see whether or not the users phone has a particular language's TTS Support

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            return false;
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
            return (_libraryclass!=null)?_libraryclass.CallStatic<bool>(_CheckAvailability,_mainactivity.GetStatic<AndroidJavaObject>("currentActivity"), locale):false;
        #else 
            return false;
        #endif
    }

    public static void Speak(string sentence) { // A Generic method for accessing TTS engine to play words

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            return;
        }

        #if UNITY_ANDROID && !UNITY_EDITOR
        if(_libraryclass==null){
            BootUpTTS();
        }
        if(_libraryclass!= null && !_libraryclass.CallStatic<bool>(_uttering)) {
            _libraryclass.CallStatic(_MethodName,sentence);
        }
        #endif
    }

    public static void DownloadTTSData() { // Creates an Intent which allows users to download TTS languages

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            return;
        }
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_libraryclass!=null) {
            _libraryclass.CallStatic(_downloadttsdata, _mainactivity.GetStatic<AndroidJavaObject>("currentActivity"));
        }
        #endif
    }

    public static void StopTTS() { //Stop Running the TTS and free up resources
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (_libraryclass!=null) {
            _libraryclass.CallStatic(_shutdowntts);
        }
        #endif
        _mainactivity = null;
        _libraryclass = null;
    }

    public static string[] GetAvailableLocales() { // Returns a list of all the available locales/languages as a string array
        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass!=null)?_libraryclass.CallStatic<string[]>(_getavailablelocales):null;
        #else
        return null;
        #endif
    }

    public static bool SetLocale(string locale, string script = "", string region = "") { //Refer to Documentation for more details on this method
        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass!=null) ? _libraryclass.CallStatic<bool>(_setlocale, locale,script,region) : false;
        #else
        return false;
        #endif
    }

    public static string GetLocale() {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass!=null) ? _libraryclass.CallStatic<string>(_getlocale) : "TTS Has not been initialized!";
        #else 
        return "";
        #endif
    }

    public static bool IsBootedUp() {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass!=null) ? _libraryclass.CallStatic<bool>(_isbootedup) : false;
        #else
        return false;
        #endif
    }

    public static bool IsUttering() {

        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass!=null)?_libraryclass.CallStatic<bool>(_uttering) : false;
        #else
        return false;
        #endif
    }

    public static bool SetEngineByPackageName(string packagename) { // The Default TTS Engine used is com.google.android.tts.
        #if UNITY_ANDROID && !UNITY_EDITOR
        return (_libraryclass != null) ? _libraryclass.CallStatic<bool>(_setEngineByPackageName, packagename) : false;
        #else
        return false;
        #endif
    }

    public static void StopSpeaking() { //Stops the current running utternance. Can be used to stop a sentence that has been spoken midway
        #if UNITY_ANDROID && !UNITY_EDITOR
        if(_libraryclass != null) {
            _libraryclass.CallStatic(_stopspeaking);
        }
        #endif
    }
}
