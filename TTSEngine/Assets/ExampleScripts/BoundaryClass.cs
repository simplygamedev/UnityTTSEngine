using UnityEngine;
using UnityEngine.UI;

public class BoundaryClass : MonoBehaviour {

    private static BoundaryClass instance;

    public GameObject scrollview_content; //This is where we attach the Scroll Views child content GameObject.

    public GameObject scrollview_buttons;

    public GameObject PlayTextButton; //Attach the Button that plays the TTSManager.Speak() Method
    public GameObject _InputField;

    private InputField _InputFieldComponent;
    private static Text _InputFieldText;

    public Slider PitchSlider;
    public Slider SpeedSlider;

    public Text DisplayLocale;
    public Text DisplayLocalAvailaibility;

    public GameObject[] Buttons; //JapaneseButton, GermanButton, EnglishButton, ThaiButton

    static ScrollViewHandler sv_handler;

    static readonly string[] languagebuttons = new string[] { "Canvas/Japanese",
                                                              "Canvas/German",
                                                              "Canvas/Thai",
                                                              "Canvas/English" }; 


	void Awake () {

        instance = this;

        if (Buttons==null || Buttons.Length < languagebuttons.Length) {
            Buttons = new GameObject[languagebuttons.Length];

            for (var i =0;i<languagebuttons.Length;i++) {
                Buttons[i] = GameObject.Find(languagebuttons[i]);
                instance.Buttons[i] = Buttons[i];
            }
        }

            instance.PlayTextButton = GameObject.Find("Canvas/PlayText");


        instance.scrollview_buttons = scrollview_buttons;

            instance.scrollview_content = GameObject.Find("Canvas/Scroll View/Viewport/Content");


            instance.PitchSlider = GameObject.Find("Canvas/Pitch").GetComponent<Slider>();


            instance.SpeedSlider = GameObject.Find("Canvas/SpeechSpeed").GetComponent<Slider>();
        


            instance.DisplayLocale = GameObject.Find("Canvas/DisplayLocale").GetComponent<Text>();


            instance.DisplayLocalAvailaibility = GameObject.Find("Canvas/DisplayLocaleAvailability").GetComponent<Text>();

        instance._InputField = GameObject.Find("Canvas/InputField");

        instance._InputFieldComponent = _InputField.GetComponent<InputField>();

        _InputFieldText = GameObject.Find("Canvas/InputField/Text").GetComponent<Text>();

        sv_handler = new ScrollViewHandler(scrollview_buttons,ref scrollview_content,new ButtonMethod(_ButtonMethod));
    }

    public static void _ButtonMethod(string _param) {
        instance._InputFieldComponent.text = _param;
    }
	
    public static void PopulateScrollView(string type) {

        if (sv_handler.IsPopulated()) {
            sv_handler.DepopulateScrollView();
        }

        switch (type.ToLower()) {
            case "ja":
                sv_handler.PopulateScrollView(ExampleText.Japanese);
                _InputFieldText.fontSize = 80;
                break;
            case "de":
                sv_handler.PopulateScrollView(ExampleText.German);
                _InputFieldText.fontSize = 40;
                break;
            case "th":
                sv_handler.PopulateScrollView(ExampleText.Thai);
                _InputFieldText.fontSize = 80;
                break;
            case "en":
            default:
                sv_handler.PopulateScrollView(ExampleText.English);
                _InputFieldText.fontSize = 40;
                break;
        }
    }

    public static InputField ReturnInputFieldComponent()
    {
        return instance._InputFieldComponent;
    }

    public static Slider ReturnBoundarySlider(string slidername) {

        var temp = slidername.ToLower();

        if (temp.Contains("pitch")) {

            return instance.PitchSlider;
        }
        else if(temp.Contains("speed")){

            return instance.SpeedSlider;
        }
        else {

            throw new System.NullReferenceException("The Referenced Slider Object does not exist!");
        }
    }

    public static void SetTextComponent(string TextObjectName, string text) {

        var temp = TextObjectName.ToLower();

        if (temp == "displaylocale") {
            instance.DisplayLocale.text = text;
        }
        else if (temp == "displaylocaleavailability") {
            instance.DisplayLocalAvailaibility.text = text;
        }
        else {

            throw new System.NullReferenceException("The Referenced Text Object does not exist!");
        }
    }
}
