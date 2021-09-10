using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private enum State {
        PlayButton = 0,
        OptionsButton = 1,
        CreditsButton = 2,
        QuitButton = 3,
    }

    private enum StateMenu
    {
        MenuPrincipal = 0,
        MenuOptions = 1,
        MenuCredits = 2,
    }


    public Image[] buttonImage;

    float currentTime = 0;
    float normalizedValue;

    [Scene] public string sceneGameplay;

    public AudioSource audioUIValidate;

    [Header ("Panel Principal")]
    public GameObject PanelPrincipal;
    public Image playerCursor;
    public RectTransform goingOutPrincipal;

    [Header("Panel Options")]
    public GameObject PanelOptions;
    public Image playerCursorOptions;
    public RectTransform goingIn;
    public RectTransform goingOut;
    private bool isTransition;
    public Slider musicSlider;
    public Slider sfxSlider;
    private int sliderChoosen;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI musicSliderPourcentText;
    public TextMeshProUGUI sfxSliderText;
    public TextMeshProUGUI sfxSliderPourcentText;
    private float timerContinueSlider;

    [Header("Panel Credits")]
    public GameObject PanelCredits;

    private State state = State.PlayButton;
    private StateMenu stateMenu = StateMenu.MenuPrincipal;
    private Vector2 pos;
    private Vector2 posOption;
    private Vector2 posPlayer;
    private Vector2 posPlayerOption;
    private bool canCursorMove;
    private float timerForReInput;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        buttonImage[(int)state].gameObject.GetComponent<Animator>().SetBool("isPulsing", true);
    }

    private void MovingPlayerCursor(int id, bool flip)
    {

        timerForReInput = 0.3f;
        currentTime = 0;

        canCursorMove = true;

        pos = playerCursor.rectTransform.anchoredPosition;
        posPlayer = playerCursor.rectTransform.anchoredPosition;
        pos.x = buttonImage[id].rectTransform.anchoredPosition.x;


        buttonImage[(int)state].gameObject.GetComponent<Animator>().SetBool("isPulsing", false);
        state = (State)id;

        if (flip)
            playerCursor.rectTransform.rotation = Quaternion.Euler(0, 180, 0);
        else
            playerCursor.rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        playerCursor.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
    }

    private void Update()
    {
        switch (stateMenu)
        {
            case StateMenu.MenuPrincipal:
                if (canCursorMove)
                {
                    currentTime += Time.deltaTime;
                    normalizedValue = currentTime / 2;
                    playerCursor.rectTransform.anchoredPosition = Vector2.Lerp(posPlayer, pos, normalizedValue);
                    
                    if (playerCursor.rectTransform.anchoredPosition.x == goingOutPrincipal.anchoredPosition.x && isTransition)
                    {
                        playerCursor.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                        currentTime = 0;
                        PanelPrincipal.SetActive(false);
                        PanelOptions.SetActive(true);
                        playerCursorOptions.rectTransform.rotation = Quaternion.Euler(0, 180, 0);
                        playerCursorOptions.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
                        stateMenu = StateMenu.MenuOptions;
                        musicSliderText.GetComponent<Animator>().SetBool("isPulsing", true);
                    }
                    else if (playerCursor.rectTransform.anchoredPosition.x == pos.x && !isTransition)
                    {
                        playerCursor.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                        buttonImage[(int)state].gameObject.GetComponent<Animator>().SetBool("isPulsing", true);
                        canCursorMove = false;
                        currentTime = 0;
                    }
                }

                if (timerForReInput > 0)
                {
                    timerForReInput -= Time.deltaTime;
                }
                else
                {
                    switch (state)
                    {
                        case State.PlayButton:
                            if (player.GetButtonDown("Validate"))
                            {
                                audioUIValidate.Play();
                                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneGameplay);
                            }

                            if (player.GetAxis("HorizontalMove") > 0)
                            {
                                MovingPlayerCursor(3, false);
                            }
                            else if (player.GetAxis("HorizontalMove") < 0)
                            {
                                MovingPlayerCursor(1, false);
                            }
                            break;

                        case State.OptionsButton:

                            if (player.GetButtonDown("Validate"))
                            {
                                audioUIValidate.Play();

                                posOption = playerCursorOptions.rectTransform.anchoredPosition;
                                posPlayerOption = playerCursorOptions.rectTransform.anchoredPosition;
                                posOption.x = goingIn.anchoredPosition.x;

                                timerForReInput = 0.3f;
                                currentTime = 0;

                                canCursorMove = true;

                                pos = playerCursor.rectTransform.anchoredPosition;
                                posPlayer = playerCursor.rectTransform.anchoredPosition;
                                pos.x = goingOutPrincipal.anchoredPosition.x;

                                buttonImage[(int)state].gameObject.GetComponent<Animator>().SetBool("isPulsing", false);

                                playerCursor.rectTransform.rotation = Quaternion.Euler(0, 180, 0);

                                playerCursor.gameObject.GetComponent<Animator>().SetBool("isWalking", true);

                                isTransition = true;
                            }

                            if (player.GetAxis("HorizontalMove") > 0)
                            {
                                MovingPlayerCursor(0, true);
                            }
                            else if (player.GetAxis("HorizontalMove") < 0)
                            {
                                MovingPlayerCursor(2, false);
                            }
                            break;

                        case State.CreditsButton:
                            if (player.GetAxis("HorizontalMove") > 0)
                            {
                                MovingPlayerCursor(1, true);
                            }
                            else if (player.GetAxis("HorizontalMove") < 0)
                            {
                                MovingPlayerCursor(3, false);
                            }
                            break;

                        case State.QuitButton:

                            if (player.GetButtonDown("Validate"))
                            {
                                audioUIValidate.Play();
                                Application.Quit();
                            }

                            if (player.GetAxis("HorizontalMove") > 0)
                            {
                                MovingPlayerCursor(2, true);
                            }
                            else if (player.GetAxis("HorizontalMove") < 0)
                            {
                                MovingPlayerCursor(0, true);
                            }
                            break;
                    }
                }
                break;

            case StateMenu.MenuOptions:

                if (canCursorMove)
                {
                    currentTime += Time.deltaTime;
                    normalizedValue = currentTime / 2;
                    playerCursorOptions.rectTransform.anchoredPosition = Vector2.Lerp(posPlayerOption, posOption, normalizedValue);
                    if (playerCursorOptions.rectTransform.anchoredPosition.x == goingIn.anchoredPosition.x && isTransition)
                    {
                        playerCursorOptions.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                        currentTime = 0;
                        canCursorMove = false; 
                        isTransition = false;
                    }
                    else if (playerCursorOptions.rectTransform.anchoredPosition.x == goingOut.anchoredPosition.x && !isTransition)
                    {
                        playerCursorOptions.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
                        currentTime = 0;
                        PanelPrincipal.SetActive(true);
                        PanelOptions.SetActive(false);
                        stateMenu = StateMenu.MenuPrincipal;

                        pos = goingOutPrincipal.anchoredPosition;
                        posPlayer = goingOutPrincipal.anchoredPosition;
                        pos.x = buttonImage[1].rectTransform.anchoredPosition.x;

                        playerCursor.rectTransform.rotation = Quaternion.Euler(0, 0, 0);

                        playerCursor.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
                    }
                }

                if (player.GetButtonDown("Return") && !playerCursorOptions.gameObject.GetComponent<Animator>().GetBool("isWalking"))
                {
                    audioUIValidate.Play();
                    playerCursorOptions.gameObject.GetComponent<Animator>().SetBool("isWalking", true);

                    posOption = playerCursorOptions.rectTransform.anchoredPosition;
                    posPlayerOption = playerCursorOptions.rectTransform.anchoredPosition;
                    posOption.x = goingOut.anchoredPosition.x;
                    canCursorMove = true;
                    playerCursorOptions.rectTransform.rotation = Quaternion.Euler(0, 0, 0);

                    musicSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                    sfxSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                }

                if (timerContinueSlider <= 0 && !playerCursorOptions.gameObject.GetComponent<Animator>().GetBool("isWalking"))
                {

                    if (player.GetAxis("VerticalMove") > 0)
                    {
                        if (sliderChoosen == 1)
                        {
                            sliderChoosen = 0;
                            musicSliderText.GetComponent<Animator>().SetBool("isPulsing", true);
                            sfxSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                        }
                        else if (sliderChoosen == 0)
                        {
                            sliderChoosen = 1;
                            musicSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                            sfxSliderText.GetComponent<Animator>().SetBool("isPulsing", true);
                        }
                        timerContinueSlider = 0.4f;
                    }
                    else if (player.GetAxis("VerticalMove") < 0)
                    {
                        if (sliderChoosen == 1)
                        {
                            sliderChoosen = 0;
                            musicSliderText.GetComponent<Animator>().SetBool("isPulsing", true);
                            sfxSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                        }
                        else if (sliderChoosen == 0)
                        {
                            sliderChoosen = 1;
                            musicSliderText.GetComponent<Animator>().SetBool("isPulsing", false);
                            sfxSliderText.GetComponent<Animator>().SetBool("isPulsing", true);
                        }
                        timerContinueSlider = 0.4f;
                    }

                    if (player.GetAxis("HorizontalMove") < 0)
                    {
                        if (sliderChoosen == 1)
                        {
                            sfxSlider.value = Mathf.Clamp(sfxSlider.value + 1, 0.001f, 100);
                            sfxSliderPourcentText.text = ((int)sfxSlider.value).ToString();
                        }
                        else if (sliderChoosen == 0)
                        {
                            musicSlider.value = Mathf.Clamp(musicSlider.value + 1, 0.001f, 100);
                            musicSliderPourcentText.text = ((int)musicSlider.value).ToString();
                        }
                        timerContinueSlider = 0.4f;
                    }
                    else if (player.GetAxis("HorizontalMove") > 0)
                    {
                        if (sliderChoosen == 1)
                        {
                            sfxSlider.value = Mathf.Clamp(sfxSlider.value - 1, 0.001f, 100);
                            sfxSliderPourcentText.text = ((int)sfxSlider.value).ToString();
                        }
                        else if (sliderChoosen == 0)
                        {
                            musicSlider.value = Mathf.Clamp(musicSlider.value - 1, 0.001f, 100);
                            musicSliderPourcentText.text = ((int)musicSlider.value).ToString();
                        }
                        timerContinueSlider = 0.4f;
                    }
                }
                else
                {
                    timerContinueSlider -= Time.deltaTime;
                }

                break;

            case StateMenu.MenuCredits:
                break;
        }

        
    }

}
