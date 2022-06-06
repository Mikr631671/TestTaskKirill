using UnityEngine;
using System.IO;
using TMPro;

public class LanguageController : MonoBehaviour
{
    public Item item;

    /*[ContextMenu("Load")]
    public void LoadField()
    {
        item = JsonUtility.FromJson<Item>(File.ReadAllText(Application.streamingAssetsPath + "/data.json"));
    }

    [ContextMenu("Save")]
    public void SaveFiled()
    {
        File.WriteAllText(Application.streamingAssetsPath + "/data.json", JsonUtility.ToJson(item));
    } */ // Functions for reading and writing to a file by keywords (easy to edit in the inspector)

    //In the best case, you can write a script that will accept only 1 word (key) by which it will look for a translation
    //depending on the current language, but for a quick solution, I completed the task like this

    public void LoadRuFiled()
    {
        item = JsonUtility.FromJson<Item>(File.ReadAllText(Application.streamingAssetsPath + "/dataRU.json"));
        gameObject.GetComponent<TextMeshProUGUI>().text = item.touchText;
    }
    
    public void LoadENGFiled()
    {
        item = JsonUtility.FromJson<Item>(File.ReadAllText(Application.streamingAssetsPath + "/dataENG.json"));
        gameObject.GetComponent<TextMeshProUGUI>().text = item.touchText;
    }
}

[System.Serializable]
public class Item
{
    public string touchText;
    public string settingsText;
}
