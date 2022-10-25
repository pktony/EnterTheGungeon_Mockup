using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    enum optionMode : byte
    {
        Main = 0,
        Ammonicon,
        Option,

        Gameplay,
        Controls,
        Display,
        Audio
    }

    CanvasGroup group;
    CanvasGroup optionGroup;
    CanvasGroup audioGroup;
    TextMeshProUGUI[] texts;

    optionMode mode = optionMode.Main;
    int selectedOption;

    PlayerInput input;

    public Texture2D cursorTexture_Default;
    public Texture2D cursorTexture_Heart;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        optionGroup = transform.GetChild(2).GetComponent<CanvasGroup>();
        audioGroup = transform.GetChild(3).GetComponent<CanvasGroup>();

        input = new();
    }

    private void OnEnable()
    {
        input.UI.Enable();
        input.UI.PauseMenu.performed += OnPauseMenu;
        input.UI.Updown.performed += OnUpDown;
        input.UI.Enter.performed += OnSelection;
        input.UI.Right_Left.performed += OnValueChange;
    }

    private void OnDisable()
    {
        input.UI.PauseMenu.performed -= OnPauseMenu;
        input.UI.Updown.performed -= OnUpDown;
        input.UI.Enter.performed -= OnSelection;
        input.UI.Right_Left.performed += OnValueChange;
        input.UI.Disable();
    }

    private void Start()
    {
        //TurnOffMenu();
        mode = optionMode.Main;
        RefreshTextComponents();
    }

    private void OnSelection(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if (mode == optionMode.Main)
        {
            switch (selectedOption)
            {
                case 0:// Resume
                    TurnOffMenu();
                    break;
                case 1:// Ammonicon

                    break;
                case 2:// Options
                    ChangeMenu(optionGroup, optionMode.Option);
                    RefreshTextComponents();
                    break;
                case 3:// Quick Restart
                    SceneManager.LoadScene(1);
                    break;
                case 4:// Quit to Desktop
                    Application.Quit();
                    break;
            }
        }
        else if(mode == optionMode.Option)
        {
            switch(selectedOption)
            {
                case 0:// Gameplay
                    mode = optionMode.Gameplay;
                    break;
                case 1:// Controls
                    mode = optionMode.Controls;
                    break;
                case 2:// Display
                    mode = optionMode.Display;
                    break;
                case 3:// Audio
                    ChangeMenu(optionGroup, optionMode.Option, false);
                    ChangeMenu(audioGroup, optionMode.Audio);
                    RefreshTextComponents();
                    break;
            }
        }

        SoundManager.Inst.PlaySound_UI(Clips_UI.Menu_Confirm);
    }

    private void ChangeMenu(CanvasGroup group, optionMode _mode, bool isShow = true)
    {
        mode = _mode;
        group.alpha = isShow ? 1.0f : 0.0f ;
        group.interactable = isShow;
        group.interactable = isShow;
        selectedOption = 0;
    }

    private void RefreshTextComponents()
    {
        texts = null;
        if(mode == optionMode.Main)
        {
            texts = transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();
        }
        else if(mode == optionMode.Option)
        {
            texts = transform.GetChild(2).GetComponentsInChildren<TextMeshProUGUI>();
        }
        else if(mode == optionMode.Audio)
        {
            texts = transform.GetChild(3).GetComponentsInChildren<TextMeshProUGUI>();
        }
    }

    private void OnUpDown(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if(input < 0f)
        {// up
            selectedOption = ++selectedOption % texts.Length;
        }
        else
        {// down
            selectedOption = (selectedOption + texts.Length - 1) % texts.Length;
        }

        foreach(var text in texts)
        {
            text.color = Color.gray;
        }
        texts[selectedOption].color = Color.white;

        SoundManager.Inst.PlaySound_UI(Clips_UI.Menu_Select);
    }

    private void OnValueChange(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        if (input > 0)
        {

        }
    }

    private void OnPauseMenu(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        if(group.alpha < 0.1f)
        {// Menu Closed
            TurnOnMenu();
        }
        else
        {// Menu Opened
            TurnOffMenu();
        }
    }

    private void TurnOnMenu()
    {
        Time.timeScale = 0f;
        group.alpha = 1.0f;
        group.interactable = true;
        group.blocksRaycasts = true;
        selectedOption = 0;
        input.UI.Enable();
        GameManager.Inst.Player.gameObject.SetActive(false);
        mode = optionMode.Main;
        Cursor.SetCursor(cursorTexture_Heart, Vector2.zero, CursorMode.Auto);

        SoundManager.Inst.PlaySound_UI(Clips_UI.Menu_Pause);
    }
    private void TurnOffMenu()
    {
        Time.timeScale = 1.0f;
        group.alpha = 0.0f;
        group.interactable = false ;
        group.blocksRaycasts = false;
        input.UI.Disable();
        input.UI.PauseMenu.Enable();
        GameManager.Inst.Player.gameObject.SetActive(true);
        Cursor.SetCursor(cursorTexture_Default, Vector2.zero, CursorMode.Auto);

        SoundManager.Inst.PlaySound_UI(Clips_UI.Menu_Cancel);
    }
}
