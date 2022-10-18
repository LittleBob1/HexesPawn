using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeOfSides : MonoBehaviour
{
    private StartGameTwoPlayer playerLogic;
    private ButtonLogicBuy butLog;
    private RotateAroundAndZoom rt;

    public TMP_Text currentPlayerText;

    private bool afterChange = false;

    private void Start()
    {
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        butLog = GameObject.Find("ButtonsController").GetComponent<ButtonLogicBuy>();
        rt = GameObject.Find("MainCamera").GetComponent<RotateAroundAndZoom>();
    }

    public void ChangeTurn()
    {
        SetNotMovedOnPawns();
        afterChange = false;
        if (playerLogic.currentPlayer.getName() == "playerOne")
        {
            playerLogic.currentPlayer = playerLogic.GetPlayerTwo();
            currentPlayerText.text = "Blue player's turn";
            rt.sideIsChange = true;
            rt.lerpX = 180;

        }
        else
        {
            playerLogic.currentPlayer = playerLogic.GetPlayerOne();
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

        butLog.RecalculationPawns();
        butLog.CloseAllForChangeSide();
        butLog.UpdateGoldOnText();
    }

    private void SetNotMovedOnPawns()
    {
        List<Pawns> listOfCurrentPlayerPawns = playerLogic.currentPlayer.getListOfPawns();

        for (int i = 0; i < listOfCurrentPlayerPawns.Count; i++)
        {
            listOfCurrentPlayerPawns[i].SetBoolIsNotMoved();
        }
    }

    private void Update()
    {
        if (Input.touchCount >= 1 && afterChange)
        {
            rt.sideIsChange = false;
        }
        afterChange = true;
    }
}
