using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartGameTwoPlayer : MonoBehaviour
{
    public Player currentPlayer;
    private Player playerOne;
    private Player playerTwo;

    public TMP_Text currentPlayerText;

    private RotateAroundAndZoom rt;
    private ButtonLogicBuy logBut;


    private void Start()
    {
        playerOne = new Player("playerOne");
        playerTwo = new Player("playerTwo");

        rt = GameObject.Find("MainCamera").GetComponent<RotateAroundAndZoom>();
        logBut = GameObject.Find("ButtonsController").GetComponent<ButtonLogicBuy>();

        SideSelection();
    }

    private void SideSelection()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            currentPlayer = playerOne;
            currentPlayerText.text = "Green player's turn";

            rt.sideIsChange = true;
          
            if (rt.X >= 180)
            {
                rt.lerpX = 360;
            }
            else
            {
                rt.lerpX = 0;
            }
        }
        else
        {
            currentPlayer = playerTwo;
            currentPlayerText.text = "Blue player's turn";

            rt.sideIsChange = true;
            rt.lerpX = 180;
        }
    }

    public void SetTrees(MyTree plOne, MyTree plTwo)
    {
        playerOne.setMyTree(plOne);
        playerTwo.setMyTree(plTwo);
        playerOne.setGold(200);
        playerTwo.setGold(200);
    }

    public Player GetPlayerOne()
    {
        return playerOne;
    }

    public Player GetPlayerTwo()
    {
        return playerTwo;
    }
}
