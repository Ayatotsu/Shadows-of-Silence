using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuTween : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup blackBackgroundPanel;
    public CanvasGroup gameBackgroundPanel;
    public CanvasGroup studioLogoPanel;
    public CanvasGroup gameTitle;
    public CanvasGroup mainMenuPanel;
    public CanvasGroup playPanel;
    public CanvasGroup optionsPanel;
    public CanvasGroup creditsPanel;
    public CanvasGroup quitPanel;

    [Header("Main Menu Panel Buttons")]
    public CanvasGroup[] buttons; //Array to hold all main menu buttons for fade-in effect

    [Header("Play Panel Buttons")]
    public Button newGameButton;
    public Button continueButton; //skip functionality for now
    public Button backButton;

    [Header("Options Panel Buttons/Sliders/Toggles")]
    public Button optionsBackButton;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider globalSlider;
    public Toggle muteBGM;
    public Toggle muteSFX;

    [Header("Credits Panel Buttons")]
    public Button creditsBackButton;

    [Header("Quit Panel Buttons")]
    public Button quitYesButton;
    public Button quitNoButton;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;
    public float gameDurationDelay = 3f; //Transition delay before transitioning to Game Scene
    public float backgroundFadeDuration = 1f;
    public float gameBackgroundDuration = 2f; //Time the game background takes to fade in
    [Header("Timing")]
    public float logoDisplayTime = 3f; //Time the studio logo is displayed before transitioning to main menu
    public float titleDelay = 0.5f; //Initial delay before starting logo fade-in
    public float logoFadeDuration = 1f; //Duration of the logo fade-in and fade-out
    public float buttonsDelay = 1.0f; //Delay before starting buttons fade-in

    void Start()
    {
        blackBackgroundPanel.alpha = 0f; //black background always visible
        studioLogoPanel.alpha = 0f; //logo starts invisible
        gameBackgroundPanel.alpha = 0f; //game background starts invisible
        gameTitle.alpha = 0f; //title starts invisible
        mainMenuPanel.alpha = 0f; //main menu starts invisible
        
        foreach (CanvasGroup btn in buttons)
        {
            btn.alpha = 0f; //buttons start invisible
        }

        PlayAnimationSequence();

        //Initialize panels
        //SetPanelActive(blackBackgroundPanel, false);
        //SetPanelActive(studioLogoPanel, false);
        //SetPanelActive(gameTitle, false);
        //SetPanelActive(mainMenuPanel, false);
        SetPanelActive(playPanel, false);
        SetPanelActive(optionsPanel, false);
        SetPanelActive(creditsPanel, false);
        SetPanelActive(quitPanel, false);

        //Assign button actions
        mainMenuPanel.interactable = true; //Make main menu panel interactable after fade-in
        SetButtons();



    }
    public void SetButtons() 
    {
        //Mian Menu Buttons
        buttons[0].gameObject.gameObject.GetComponent<Button>().onClick.AddListener(OnPlayButton);
        buttons[1].gameObject.gameObject.GetComponent<Button>().onClick.AddListener(OnOptionsButton);
        buttons[2].gameObject.gameObject.GetComponent<Button>().onClick.AddListener(OnCreditsButton);
        buttons[3].gameObject.gameObject.GetComponent<Button>().onClick.AddListener(OnQuitButton);

        //New game button
        newGameButton.onClick.AddListener(OnNewGame);

        //Back To Main Menu Buttons
        backButton.onClick.AddListener(PlayBackToMainMenu);
        optionsBackButton.onClick.AddListener(OptionToMainMenu);
        creditsBackButton.onClick.AddListener(CreditsToMainMenu);
        quitNoButton.onClick.AddListener(QuitToMainMenu);

        //Quit Game Button
        quitYesButton.onClick.AddListener(OnQuit);
    }
    void PlayAnimationSequence()
    {
        //Fade in logo
        LeanTween.alphaCanvas(studioLogoPanel, 1f, fadeDuration)
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                //Wait for logo display time, then fade out
                LeanTween.delayedCall(logoDisplayTime, () =>
                {
                    LeanTween.alphaCanvas(studioLogoPanel, 0f, fadeDuration)
                        .setEase(LeanTweenType.easeInOutQuad)
                        .setOnComplete(() =>
                        {
                            //Activate and fade in main menu panel
                            mainMenuPanel.alpha = 1f; //panel itself visible
                            gameBackgroundPanel.alpha = 1f; //background starts invisible

                            LeanTween.delayedCall(gameBackgroundDuration, () => { 
                                //Fade in Game Background
                                LeanTween.alphaCanvas(gameBackgroundPanel, 1f, backgroundFadeDuration)
                                         .setEase(LeanTweenType.easeInOutQuad);
                            
                            });
                            
                            //Fade in title
                            LeanTween.delayedCall(titleDelay, () =>
                            {
                                LeanTween.alphaCanvas(gameTitle, 1f, fadeDuration)
                                         .setEase(LeanTweenType.easeInOutQuad);
                            });

                            //Fade in buttons after delay
                            LeanTween.delayedCall(buttonsDelay, () =>
                            {
                                foreach (CanvasGroup btn in buttons)
                                {
                                    LeanTween.alphaCanvas(btn, 1f, fadeDuration)
                                             .setEase(LeanTweenType.easeInOutQuad);
                                }
                            });
                        });
                });
            });
    }

    //Enable panel and set Canvas group alpha and interactivity
    void SetPanelActive(CanvasGroup panel, bool active)
    {
        panel.gameObject.SetActive(active);
        panel.alpha = active ? 1f : 0f;
        panel.interactable = active;
        panel.blocksRaycasts = active;
    }

    //Fade out the current panel and fade in the next panel
    public void TransitionPanel(CanvasGroup from, CanvasGroup to)
    {

        LeanTween.alphaCanvas(from, 0f, fadeDuration).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                SetPanelActive(from, false);
                SetPanelActive(to, true);
                LeanTween.alphaCanvas(to, 1f, fadeDuration).setEase(LeanTweenType.easeInOutQuad);
            });
    }

    public void TransitionToMainMenu(CanvasGroup from)
    {
        LeanTween.alphaCanvas(from, 0f, fadeDuration).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                SetPanelActive(from, false);
                SetPanelActive(mainMenuPanel, true);
                //Fade in title
                LeanTween.delayedCall(titleDelay, () =>
                {
                    LeanTween.alphaCanvas(gameTitle, 1f, fadeDuration)
                             .setEase(LeanTweenType.easeInOutQuad);
                });

                //Fade in buttons after delay
                LeanTween.delayedCall(buttonsDelay, () =>
                {
                    foreach (CanvasGroup btn in buttons)
                    {
                        LeanTween.alphaCanvas(btn, 1f, fadeDuration)
                                 .setEase(LeanTweenType.easeInOutQuad);
                        AddHoverEffect(btn.gameObject);
                    }
                });
            });
    }

    private void AddHoverEffect(GameObject button)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null) trigger = button.AddComponent<EventTrigger>();

        // Hover enter
        var enter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enter.callback.AddListener((_) => LeanTween.scale(button, Vector3.one * 1.1f, 0.2f));
        trigger.triggers.Add(enter);

        // Hover exit
        var exit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exit.callback.AddListener((_) => LeanTween.scale(button, Vector3.one, 0.2f));
        trigger.triggers.Add(exit);
    }

    //New Game transition to Prologue Scene(fade out panel then load new scene)
    public void OnNewGame()
    {
        //Fade out all active panels
        CanvasGroup[] panels = {blackBackgroundPanel, gameBackgroundPanel, studioLogoPanel, mainMenuPanel, playPanel, optionsPanel, creditsPanel, quitPanel };

        foreach (CanvasGroup panel in panels)
        {
            if (panel.gameObject.activeSelf)
            {
                LeanTween.alphaCanvas(panel, 0f, fadeDuration).setEase(LeanTweenType.easeInOutQuad);
                LeanTween.alphaCanvas(gameBackgroundPanel, 0f, backgroundFadeDuration).setEase(LeanTweenType.easeInOutQuad);
                //panel.interactable = false; //Disable interactivity during transition
            }
        }

        //Load new scene after fade out
        LeanTween.delayedCall(gameDurationDelay, () =>
        {
            SceneManager.LoadScene("SampleScene");
            Debug.Log("New Game Started, Prologue Scene Loaded!");
        });
    }

    //Quit game
    public void OnQuit()
    {
        LeanTween.alphaCanvas(quitPanel, 0f, fadeDuration).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.alphaCanvas(gameBackgroundPanel, 0f, gameBackgroundDuration).setEase(LeanTweenType.easeInOutQuad);

                //Delay 2 seconds then quit
                LeanTween.delayedCall(3f, () =>
                {
                    Application.Quit();
                    Debug.Log("Quit Game Completed!");

                });
            });
    }

    //Call functions on MainMenu buttons
    public void OnPlayButton() 
    {
        TransitionPanel(mainMenuPanel, playPanel); //Transitions the current panel to the next panel
    }
    public void OnOptionsButton()
    {
        TransitionPanel(mainMenuPanel, optionsPanel);
    }
    public void OnCreditsButton()
    {
        TransitionPanel(mainMenuPanel, creditsPanel);
    }
    public void OnQuitButton()
    {
        TransitionPanel(mainMenuPanel, quitPanel);
    }

    public void OnContinueButton()
    {
        //Continue functionality to be implemented later
        Debug.Log("Continue Button Pressed - Functionality to be implemented later");
    }
    public void PlayBackToMainMenu()
    {
        TransitionToMainMenu(playPanel);
    }
    public void OptionToMainMenu()
    {
        TransitionToMainMenu(optionsPanel);
    }

    public void CreditsToMainMenu()
    {
        TransitionToMainMenu(creditsPanel);
    }

    public void QuitToMainMenu()
    {
        TransitionToMainMenu(quitPanel);
    }
}

