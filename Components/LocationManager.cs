using System.IO;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manage the localization 
/// </summary>
public class LocationManager : MonoBehaviour
{
    #region Variables

    GameManager gameManager;

    //Localization    
    private Dictionary<string, string> languageDictionary;
    private List<string> languages;
    private int index;

    public string CurrentLanguage { get => currentLanguage; }
    private string currentLanguage;

    #endregion

    #region Initialization

    /// <summary>
    /// Init all the LocationManager
    /// </summary>
    /// <param name="_gameManager">Reference of GameManeger</param>
    public void Initialize(GameManager _gameManager)
    {
        //Init references
        gameManager = _gameManager;

        //Check language select
        if (PlayerPrefs.GetString("language") != "")
        {
            currentLanguage = PlayerPrefs.GetString("language");
        }

        else 
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Spanish:
                    currentLanguage = "language_es";
                    break;
                case SystemLanguage.English:
                    currentLanguage = "language_en";
                    break;
                default:
                    currentLanguage = "language_en";
                    break;
            }
        }
        
        //Select language
        LoadLenaguage(currentLanguage);
        SaveCurrentLanguages();
    }
    
    #endregion

    /// <summary>
    /// Load the correct language dictionary
    /// </summary>
    public void LoadLenaguage(string _newLanguage)
    {   
        //Reset dictionary
        languageDictionary = new Dictionary<string, string>();

        //Save the path of the file
        string _filePath = Path.Combine(Application.streamingAssetsPath + "/Languages", _newLanguage + ".json");

        if (!File.Exists(_filePath)) return;

        //Get information from .JSON
        string _json = File.ReadAllText(_filePath);
        LocalizationData _data = JsonUtility.FromJson<LocalizationData>(_json);

        //Save data in the dictionary
        for (int i = 0; i < _data.items.Count; i++) languageDictionary.Add(_data.items[i].key, _data.items[i].value);
        
        //Save playerpref data
        PlayerPrefs.SetString("language", _newLanguage);

        //Send language event
        if (Language_Events.languageEvent != null) Language_Events.languageEvent();
    }

    /// <summary>
    /// Save all the languages available in a list
    /// </summary>
    /// <param name="_currentLanguage">Nombre del archivo .json cargado</param>
    void SaveCurrentLanguages()
    {
        //Init list
        languages = new List<string>();

        //Get files
        string _filePath = Application.streamingAssetsPath + "/Languages";
        DirectoryInfo _folder = new DirectoryInfo(_filePath);
        FileInfo[] _file = _folder.GetFiles();

        //Save list
        for (int i = 0; i < _file.Length; i++) 
        {
            string[] _final = _file[i].Name.Split('.');
            if (_final.Length < 3) languages.Add(_final[0]);

            //Init index
            if (_final[0] == currentLanguage) index = i / 2;
        }
    }

    /// <summary>
    /// Change the current language
    /// </summary>
    /// <param name="_dir">Direction of traversing the list</param>
    public void ChangeLanguage(int _dir)
    {
        //Update index
        index = index + _dir;

        //Clamp index
        if (index >= languages.Count) index = 0;
        else if (index < 0) index = languages.Count - 1;

        LoadLenaguage(languages[index]);        
    }

    /// <summary>
    /// Check into the dictionary if the key exists
    /// </summary>
    /// <param name="_key">Key of location</param>
    /// <returns>The value of the location text</returns>
    public string GetText(string _key)
    {
        //Init the result
        string _result = "ERROR";

        //Check the key
        if (languageDictionary.ContainsKey(_key)) { _result = languageDictionary[_key]; }

        //Return the value
        return _result;
    }
}

//---------------------------------------

[System.Serializable]
public class LocalizationData
{
    public List<LocalizationItem> items;
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    [UnityEngine.TextArea] public string value;
}
