using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class CanvasController : MonoBehaviour {

    public static CanvasController instance;

    static StringBuilder sb = new StringBuilder();

    static readonly string[,] languagelocaledict = new string[4,2] { { "japanese", "ja" },
                                                     {"english", "en" },
                                                     {"thai","th" },
                                                     {"german","de" }
    };

    public async void ChangeTTSLanguage(string language) {

        if (!TTSManager.IsBootedUp()) {
            TTSManager.BootUpTTS();
        }
        else if (TTSManager.IsUttering())
        {
            TTSManager.StopSpeaking();
        }

        string locale = null;

        for (var i=0;i<languagelocaledict.Length;i++) {
            if (languagelocaledict[i,0] == language.ToLower()) {
                locale = languagelocaledict[i,1];
                break;
            }
        }

        if (locale==null) {
            return;
        }

        SetLocaleOnView(locale);
        SetLocalAvailabilityOnView(locale);
        BoundaryClass.PopulateScrollView(locale);
        TTSManager.SetLocale(locale);     
    }

    public void Speak() {

        if (TTSManager.IsUttering())
        {
            TTSManager.StopSpeaking();
        }

        SpeakText();
    }

    async Task SpeakText() {

        if (!TTSManager.IsBootedUp())
        {
            TTSManager.BootUpTTS();
            return;
        }

        TTSManager.Speak(BoundaryClass.ReturnInputFieldComponent().text);
    }

    public void ChangePitch() {

        if (!TTSManager.IsBootedUp())
        {
            TTSManager.BootUpTTS();
            return;
        }

        TTSManager.SetPitch(BoundaryClass.ReturnBoundarySlider("pitch").value);
    }

    public void ChangeSpeed() {

        if (!TTSManager.IsBootedUp())
        {
            TTSManager.BootUpTTS();
        }

        TTSManager.SetSpeechRate(BoundaryClass.ReturnBoundarySlider("speed").value);
    }

    static void SetLocaleOnView(string Locale) {

        sb.Clear();
        BoundaryClass.SetTextComponent("displaylocale", sb.AppendFormat("{0}: {1}", "Current Locale", Locale.ToUpper()).ToString());
    }

    static void SetLocalAvailabilityOnView(string Locale) {

        sb.Clear();
        BoundaryClass.SetTextComponent("displaylocaleavailability", sb.AppendFormat("{0}:\n{1}", "Language Available", (TTSManager.GetLanguageAvailability(Locale)) ? "Available" : "Not Available").ToString());
    }

    public void DownloadTTSData() {
        if (!TTSManager.IsBootedUp())
        {
            TTSManager.BootUpTTS();
        }

        TTSManager.DownloadTTSData();
    }
}
