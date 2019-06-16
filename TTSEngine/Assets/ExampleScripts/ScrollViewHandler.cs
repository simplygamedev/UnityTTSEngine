using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ButtonMethod(string _param);

public class ScrollViewHandler {

    static SpawnerScript buttonpool;

    GameObject _contentobject;

    List<GameObject> buttonsused;

    ButtonMethod _methodadd_tobuttons;

    public ScrollViewHandler(GameObject button, ref GameObject _content,  ButtonMethod methodadd_tobuttons) {

        if (buttonpool == null) {
            buttonpool = new SpawnerScript(button, 5);
        }

        _contentobject = _content;
        buttonsused = new List<GameObject>();
        _methodadd_tobuttons = methodadd_tobuttons;
    }

    public bool IsPopulated() {
        return (buttonsused.Count > 0) ? true : false;
    }

    public void PopulateScrollView(string[] contentstoadd) { 

        for (var i =0;i<contentstoadd.Length;i++) {

            var button = buttonpool.spawnPrefabIntoWorld();
            button.transform.SetParent(_contentobject.transform);
            button.transform.GetChild(0).GetComponent<Text>().text = contentstoadd[i];
            int k = i;

            button.GetComponent<Button>().onClick.AddListener(()=> { _methodadd_tobuttons(contentstoadd[k]); });
            buttonsused.Add(button);
        }
    }

    public void DepopulateScrollView() {

        for (var i=0;i<buttonsused.Count;i++) {
            buttonsused[i].transform.GetChild(0).GetComponent<Text>().text = "";
            buttonsused[i].transform.SetParent(null);
            buttonsused[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }

        buttonsused.Clear();
    }
}
