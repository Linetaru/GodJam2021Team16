using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using TMPro;

public class GameOver : MonoBehaviour
{
    bool isDead;

    private int playerID = 0;
    [SerializeField] private Player player;

    public TextMeshProUGUI cycleText;
    public GameObject panelGameOver;


    public TextMeshProUGUI restartText;
    public TextMeshProUGUI menuText;

    private int buttonChoosen;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            if (player.GetAxis("VerticalMove") < 0)
            {
                if (buttonChoosen == 0)
                {
                    buttonChoosen = 1;
                    restartText.GetComponent<Animator>().SetBool("isPulsing", false);
                    menuText.GetComponent<Animator>().SetBool("isPulsing", true);
                }
                else if (buttonChoosen == 1)
                {
                    buttonChoosen = 0;

                    restartText.GetComponent<Animator>().SetBool("isPulsing", true);
                    menuText.GetComponent<Animator>().SetBool("isPulsing", false);
                }
            }
            else if (player.GetAxis("VerticalMove") > 0)
            {
                if (buttonChoosen == 0)
                {
                    buttonChoosen = 1;
                    restartText.GetComponent<Animator>().SetBool("isPulsing", false);
                    menuText.GetComponent<Animator>().SetBool("isPulsing", true);
                }
                else if (buttonChoosen == 1)
                {
                    buttonChoosen = 0;

                    restartText.GetComponent<Animator>().SetBool("isPulsing", true);
                    menuText.GetComponent<Animator>().SetBool("isPulsing", false);
                }
            }

            if(player.GetButtonDown("Validate"))
            {
                if (buttonChoosen == 0)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainSceneGameplay");
                else if (buttonChoosen == 1)
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }

    public void playerIsDead()
    {
        player = ReInput.players.GetPlayer(playerID);
        isDead = true;
        cycleText.text = DayNightCycleManager.current.getCurrentDay().ToString();
        panelGameOver.SetActive(true);
    }
}
