using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private enum State {
        PlayButton = 0,
        OptionsButton = 1,
        CreditsButton = 2,
        QuitButton = 3,
    }

    public Image playerCursor;

    public Image[] buttonImage;

    float currentTime = 0;
    float normalizedValue;

    [Scene] public string sceneGameplay;

    public AudioSource audioUIValidate;

    private State state = State.PlayButton;
    private Vector2 pos;
    private Vector2 posPlayer;
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
        if (canCursorMove)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / 2;
            playerCursor.rectTransform.anchoredPosition = Vector2.Lerp(posPlayer, pos, normalizedValue);
            if (playerCursor.rectTransform.anchoredPosition.x == pos.x)
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
    }
}
