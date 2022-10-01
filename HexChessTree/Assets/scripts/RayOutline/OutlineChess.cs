using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OutlineChess : MonoBehaviour
{
    private List<Transform> chess;
    public Transform currentObj;
    private Transform currentOpponentObj;
    private Transform currentHex;

    private ButtonLogicBuy butLog;
    private StartGameTwoPlayer playerLogic;
    private MaterialsContainer resourses;

    private GameObject[,] map;

    public bool inTree = false;

    private void Start()
    {
        butLog = GameObject.Find("ButtonsController").GetComponent<ButtonLogicBuy>();
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        resourses = GameObject.Find("ResoursesContainer").GetComponent<MaterialsContainer>();
        chess = new List<Transform>();
        SimulationMap.GetMap += getMap;
    }
    

    void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform obj = hit.transform;

                    if (obj.gameObject.tag == "pawn" && inTree == false)
                    {
                        bool currentPlayerPawn = false;
                        if(obj.gameObject.GetComponent<IWhosePlayer>().GetPlayersTown() == playerLogic.currentPlayer)
                        {
                            currentPlayerPawn = true;
                        }

                        if (currentPlayerPawn && obj.gameObject.GetComponent<Pawns>().GetBoolMoved() == false)
                        {
                            obj.gameObject.GetComponent<Outline>().enabled = true;
                            IlluminationCancellation();
                            IllumOpponentPawnDisable();
                            obj.gameObject.GetComponent<Pawns>().CellIllumination();
                            chess.Add(obj);
                            currentObj = obj;
                            currentOpponentObj = null;
                        }
                        if (currentObj != null && currentPlayerPawn == false)
                        {
                            currentOpponentObj = obj;
                        }
                    }
                    else if (obj.gameObject.tag == "hex")
                    {
                        currentHex = obj;

                        if (currentObj != null)
                        {
                            for (int i = 0; i < butLog.pawns.Count; i++)
                            {
                                OutlineDisable(i);
                            }
                        }
                    }
                    else
                    {
                        bool currentPlayerPawn = false;

                        if (obj.gameObject.GetComponent<IWhosePlayer>().GetPlayersTown() == playerLogic.currentPlayer)
                        {
                            currentPlayerPawn = true;
                        }

                        if (currentObj != null && currentPlayerPawn == false)
                        {
                            currentOpponentObj = obj;
                        }
                    }
                }
            }
            else
            {
                if (currentObj != null)
                {
                    for (int i = 0; i < butLog.pawns.Count; i++)
                    {
                        OutlineDisable(i);
                    }
                }
            }
            
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (currentHex != null && currentObj != null)
                {
                    currentObj.GetComponent<Pawns>().Move(currentHex.gameObject);
                    currentHex = null;
                }
                else if(currentObj != null && currentOpponentObj != null)
                {
                    currentObj.GetComponent<Pawns>().Attack(currentOpponentObj.gameObject);
                    currentOpponentObj = null;
                }
            }
        }

        currentOpponentObj = null;
        currentHex = null;
    }

    private void OutlineDisable(int i)
    {
        if (butLog.pawns[i] != currentObj.gameObject)
        {
            butLog.pawns[i].gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    private void IllumOpponentPawnDisable()
    {
        List<GameObject> pawnsOne = new List<GameObject>();
        List<GameObject> pawnsTwo = new List<GameObject>();

        pawnsOne = playerLogic.GetPlayerOne().getAllObjects();

        pawnsTwo = playerLogic.GetPlayerTwo().getAllObjects();



        for (int i = 0; i < pawnsOne.Count; i++)
        {
            pawnsOne[i].GetComponent<MeshRenderer>().material = resourses.DefaultPawnFirstPlayer;
        }
        for (int i = 0; i < pawnsTwo.Count; i++)
        {
            pawnsTwo[i].GetComponent<MeshRenderer>().material = resourses.DefaultPawnSecondPlayer;
        }
    }

    private void IlluminationCancellation()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                if (map[x, y] != null)
                {
                    map[x, y].GetComponent<MeshRenderer>().material = resourses.HexDefault;
                }
            }
        }
    }

    private void getMap(GameObject[,] m)
    {
        map = m;
    }
}
