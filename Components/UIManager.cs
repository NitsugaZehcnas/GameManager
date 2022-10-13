using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manage all the UI in the game
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Variables

    //Dictionary of menus
    public Dictionary<string, BaseMenuManager> MenuDictionary { get; private set; }

    public BaseMenuManager CurrentMenu { get; private set; }
    public BasePanelManager CurrentPanel { get; private set; }
    public Button TargetButton { get; private set; }
    public Button DeselectButton { get; private set; }

    //Temporal values
    public int LateralButtonDir { get; private set; }
    public int SliderValue { get; private set; }
    
    #endregion

    #region Logic Methods    

    /// <summary>
    /// Init all the UIManager
    /// </summary>
    /// <param name="_gameManager">Reference of GameManeger</param>
    public void Initialize(GameManager _gameManager)
    {   
        //Init deselect button
        DeselectButton = GetComponent<Button>();

        //Create dictionary
        MenuDictionary = new Dictionary<string, BaseMenuManager>();

        for (int i = 0; i < transform.childCount; i++) 
        {
            BaseMenuManager _menu = transform.GetChild(i).GetComponent<BaseMenuManager>();

            //Save entro to dictionary
            MenuDictionary.Add(_menu.gameObject.name, _menu);

            //Initialize menus
            _menu.Initialize(_gameManager);
        }
    }

    #endregion

    #region UIManager Methods

    /// <summary>
    /// Open a menu
    /// </summary>
    /// <param name="_menuName">Name of menu to open</param>
    public void OpenMenu(string _menuName)
    {
        CurrentMenu = MenuDictionary[_menuName];
        MenuDictionary[_menuName].OpenMenu();
    }

    /// <summary>
    /// Close a menu
    /// </summary>
    /// <param name="_menuName">Name of menu to close</param>
    public void CloseMenu(string _menuName, bool _force = false) 
    {
        MenuDictionary[_menuName].CloseMenu(_force);
    }

    /// <summary>
    /// Change panel of the current menu
    /// </summary>
    /// <param name="_newPanel">Name of panel to change</param>
    /// <param name="_button">Button to select when change the panel</param>
    //public void ChangePanel(string _newPanel, int _button)
    //{
    //    CurrentMenu.ChangePanel(_newPanel, _button);
    //}

    #endregion

    #region Set Methods
    public void SetCurrentPanel(BasePanelManager _panel) => CurrentPanel = _panel;
    public void SetTargetButton(Button _button) => TargetButton = _button;
    public void SetLateralButtonDir(int _dir) => LateralButtonDir = _dir;
    public void SetSliderValue(int _value) => SliderValue = _value;
    #endregion
}
