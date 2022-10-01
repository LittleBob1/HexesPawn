using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ButtonLogicBuy : MonoBehaviour
{
    private GameObject[,] map;
    private SimulationMap sim;
    private StartGameTwoPlayer playerLogic;
    private OutlineChess outChess;
    private MaterialsContainer resourses;
    private int sideLength;
    private Dictionary<string, GameObject> pawnBuys;

    public string key;

    public List<GameObject> pawns = new List<GameObject>();
    //public List<GameObject> portals = new List<GameObject>();

    private List<GameObject> listOfHexForPortals = new List<GameObject>();
    private List<GameObject> listOfHexForTreePlayerOne = new List<GameObject>();
    private List<GameObject> listOfHexForTreePlayerTwo = new List<GameObject>();

    public GameObject panelBuy;

    public GameObject A;
    public GameObject D;
    public GameObject P;

    public GameObject buttonD;
    public GameObject buttonA;
    public GameObject buttonP;

    public Color colorNorm;
    public Color colorPress;

    public TMP_Text textValue; 
    //public int value = 100;

    private void Start()
    {
        SimulationMap.GetMap += GetMap;
        sim = GameObject.Find("simulation").GetComponent<SimulationMap>();
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        resourses = GameObject.Find("ResoursesContainer").GetComponent<MaterialsContainer>();
        outChess = GameObject.Find("SetController").GetComponent<OutlineChess>();
        sideLength = sim.sideLength;
        pawnBuys = new Dictionary<string, GameObject>
        {
            {"A", A},
            {"D", D},
            {"P", P},
        };

        textValue.text = "Gold: " + playerLogic.currentPlayer.getGold().ToString();
    }

    public void ButtonBuyDefender()
    {
        buttonD.GetComponent<Image>().color = colorPress;
        buttonA.GetComponent<Image>().color = colorNorm;
        buttonP.GetComponent<Image>().color = colorNorm;

        IlluminationCancellation();

        ChangeTreeIllumForPlayer();

        key = "D";
    }

    public void ButtonBuyAttacker()
    {
        buttonD.GetComponent<Image>().color = colorNorm;
        buttonA.GetComponent<Image>().color = colorPress;
        buttonP.GetComponent<Image>().color = colorNorm;

        IlluminationCancellation();
        ChangeTreeIllumForPlayer();
        key = "A";
    }

    public void ButtonBuyPortal()
    {
        buttonD.GetComponent<Image>().color = colorNorm;
        buttonA.GetComponent<Image>().color = colorNorm;
        buttonP.GetComponent<Image>().color = colorPress;

        IlluminationCancellation();
        IllumForPortals();
        key = "P";
    }

    private void ChangeTreeIllumForPlayer()
    {
        if (playerLogic.currentPlayer.getName() == "playerOne")
        {
            CellIllumForTreePlayerOne();
        }
        else
        {
            CellIllumForTreePlayerTwo();
        }
    }

    private void GetMap(GameObject[,] m)
    {
        map = m;
    }

    private void Update()
    {        
        if(map == null)
        {
            map = sim.map;
            IllumPlayerOne();
            IllumPlayerTwo();
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform obj = hit.transform;

                    if (obj.gameObject.tag == "hex" && key != null)
                    {
                        string[] s = obj.GetComponent<MeshRenderer>().material.name.Split(" ");

                        if (s[0] == resourses.HexIllum.name && obj.GetComponent<HexLogic>().isEmpty == true)
                        {
                            if (key == "A" && playerLogic.currentPlayer.getGold() >= 25)
                            {
                                GameObject g = InstantiatePawn(obj);
                                Pawns scriptPawn = g.GetComponent<Pawns>();
                                playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() - 25);
                                scriptPawn.SetIndexes(obj.GetComponent<HexLogic>().indexRow, obj.GetComponent<HexLogic>().indexCell);
                                scriptPawn.Initialization(map);
                                scriptPawn.ListRecalculation();
                                scriptPawn.setPlayer(playerLogic.currentPlayer);
                                scriptPawn.setHealth(2);
                                scriptPawn.setArmor(0);
                                scriptPawn.setDamage(3);
                                playerLogic.currentPlayer.addPawn(g.GetComponent<Pawns>());
                                playerLogic.currentPlayer.addAttacker(g.GetComponent<Attacker>());
                                playerLogic.currentPlayer.addObject(g);
                                pawns.Add(g);
                            }
                            else if (key == "D" && playerLogic.currentPlayer.getGold() >= 35)
                            {
                                GameObject g = InstantiatePawn(obj);
                                Pawns scriptPawn = g.GetComponent<Pawns>();
                                playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() - 35);
                                scriptPawn.SetIndexes(obj.GetComponent<HexLogic>().indexRow, obj.GetComponent<HexLogic>().indexCell);
                                scriptPawn.Initialization(map);
                                scriptPawn.ListRecalculation();
                                scriptPawn.setPlayer(playerLogic.currentPlayer);
                                scriptPawn.setHealth(2);
                                scriptPawn.setArmor(2);
                                scriptPawn.setDamage(1);
                                playerLogic.currentPlayer.addPawn(g.GetComponent<Pawns>());
                                playerLogic.currentPlayer.addDefender(g.GetComponent<Defender>());
                                playerLogic.currentPlayer.addObject(g);
                                pawns.Add(g);
                            }
                            else if(key == "P" && playerLogic.currentPlayer.getListOfPortals().Count <= 2 && playerLogic.currentPlayer.getGold() >= 25)
                            {
                                GameObject g = InstantiatePawn(obj);
                                Portal scriptPawn = g.GetComponent<Portal>();
                                playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() - 25);
                                scriptPawn.SetIndexesPortal(obj.GetComponent<HexLogic>().indexRow, obj.GetComponent<HexLogic>().indexCell, map);
                                scriptPawn.setPlayer(playerLogic.currentPlayer);
                                scriptPawn.SetHealth(2);
                                scriptPawn.SetArmor(0);
                                playerLogic.currentPlayer.addPortal(g.GetComponent<Portal>());
                                playerLogic.currentPlayer.addObject(g);

                                RecalculationPawns();
                            }
                            textValue.text = "Gold: " + playerLogic.currentPlayer.getGold().ToString();
                            key = null;
                        }
                    }
                }
            }
        }
    }

    public void RecalculationPawns()
    {
        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i].GetComponent<Pawns>().ListRecalculation();
        }
    }

    public void OpenBuyCard()
    {
        outChess.inTree = !outChess.inTree;
        outChess.currentObj = null;
        IlluminationCancellation();
        IllumOpponentPawnDisable();
        key = null;

        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i].gameObject.GetComponent<Outline>().enabled = false;
        }

        buttonD.GetComponent<Image>().color = colorNorm;
        buttonA.GetComponent<Image>().color = colorNorm;
        buttonP.GetComponent<Image>().color = colorNorm;

        panelBuy.SetActive(!panelBuy.activeSelf);
    }

    public void CloseAllForChangeSide()
    {
        outChess.inTree = false;
        outChess.currentObj = null;
        IlluminationCancellation();
        IllumOpponentPawnDisable();
        key = null;

        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i].gameObject.GetComponent<Outline>().enabled = false;
        }

        panelBuy.SetActive(false);
    }

    private GameObject InstantiatePawn(Transform obj)
    {
        buttonD.GetComponent<Image>().color = colorNorm;
        buttonA.GetComponent<Image>().color = colorNorm;
        buttonP.GetComponent<Image>().color = colorNorm;

        GameObject g = Instantiate(pawnBuys[key], obj.position, Quaternion.Euler(-90, 0, 0));

        if(playerLogic.currentPlayer.getName() == "playerOne")
        {
            g.GetComponent<MeshRenderer>().material = resourses.DefaultPawnFirstPlayer; //green
        }
        else
        {
            g.GetComponent<MeshRenderer>().material = resourses.DefaultPawnSecondPlayer; //blue
        }

        obj.GetComponent<HexLogic>().isEmpty = false;
        obj.GetComponent<HexLogic>().SetObjOnHex(g);
        obj.GetComponent<MeshRenderer>().material = resourses.HexDefault;
        return g;
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

    private void IllumForPortals()
    {
        listOfHexForPortals.Clear();

        List<Pawns> currentPlayerPawns = playerLogic.currentPlayer.getListOfPawns();

        for (int i = 0; i < currentPlayerPawns.Count; i++)
        {
            int row = currentPlayerPawns[i].GetComponent<Pawns>().GetRow();
            int cell = currentPlayerPawns[i].GetComponent<Pawns>().GetCell();

            if (row == map.GetLength(0) / 2)
            {
                IllumMiddle(row, cell);
            }
            else if (row >= map.GetLength(0) / 2 + 1)
            {
                IllumUp(row, cell);
            }
            else if (row <= map.GetLength(0) / 2 - 1)
            {
                IllumDown(row, cell);
            }
        }

        CellIllumPortal();

    }


    private void CellIllumPortal()
    {
        for (int i = 0; i < listOfHexForPortals.Count; i++)
        {
            if (listOfHexForPortals[i] != null)
            {
                if (listOfHexForPortals[i].GetComponent<HexLogic>().isEmpty == true)
                {
                    listOfHexForPortals[i].GetComponent<MeshRenderer>().material = resourses.HexIllum;
                }
            }
        }
    }

    private void IllumMiddle(int row, int cell)
    {
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell - 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell - 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell + 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell - 1]);
        }
        catch { }


    }

    private void IllumUp(int row, int cell)
    {
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell + 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell - 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell + 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell - 1]);
        }
        catch { }
    }

    private void IllumDown(int row, int cell)
    {
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell - 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row - 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row + 1, cell + 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell + 1]);
        }
        catch { }
        try
        {
            listOfHexForPortals.Add(map[row, cell - 1]);
        }
        catch { }
    }


    private void CellIllumForTreePlayerOne()
    {
        for (int i = 0; i < listOfHexForTreePlayerOne.Count; i++)
        {
            if (listOfHexForTreePlayerOne[i].GetComponent<HexLogic>().isEmpty == true)
            {
                listOfHexForTreePlayerOne[i].GetComponent<MeshRenderer>().material = resourses.HexIllum;
            }
        }
    }

    private void CellIllumForTreePlayerTwo()
    {
        for (int i = 0; i < listOfHexForTreePlayerTwo.Count; i++)
        {
            if (listOfHexForTreePlayerTwo[i].GetComponent<HexLogic>().isEmpty == true)
            {
                listOfHexForTreePlayerTwo[i].GetComponent<MeshRenderer>().material = resourses.HexIllum;
            }
        }
    }

    private void IllumPlayerOne()
    {
        listOfHexForTreePlayerOne.Add(map[0, sideLength / 2 + 1]);
        listOfHexForTreePlayerOne.Add(map[0, sideLength / 2 + 2]);
        listOfHexForTreePlayerOne.Add(map[0, sideLength / 2 - 1]);
        listOfHexForTreePlayerOne.Add(map[0, sideLength / 2 - 2]);

        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2 - 2]);
        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2 - 1]);
        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2]);
        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2 + 2]);
        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2 + 1]);
        listOfHexForTreePlayerOne.Add(map[1, sideLength / 2 + 3]);

        listOfHexForTreePlayerOne.Add(map[2, sideLength / 2 + 2]);
        listOfHexForTreePlayerOne.Add(map[2, sideLength / 2 + 1]);
        listOfHexForTreePlayerOne.Add(map[2, sideLength / 2 + 3]);
        listOfHexForTreePlayerOne.Add(map[2, sideLength / 2 - 1]);
        listOfHexForTreePlayerOne.Add(map[2, sideLength / 2]);

        listOfHexForTreePlayerOne.Add(map[3, sideLength / 2 + 1]);
        listOfHexForTreePlayerOne.Add(map[3, sideLength / 2 + 2]);
        listOfHexForTreePlayerOne.Add(map[3, sideLength / 2 + 3]);
        listOfHexForTreePlayerOne.Add(map[3, sideLength / 2]);
    }

    private void IllumPlayerTwo()
    {
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1, sideLength / 2 + 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1, sideLength / 2 + 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1, sideLength / 2 - 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1, sideLength / 2 - 2]);

        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2 - 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2 - 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2 + 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2 + 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 1, sideLength / 2 + 3]);

        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 2, sideLength / 2 + 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 2, sideLength / 2 + 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 2, sideLength / 2 + 3]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 2, sideLength / 2 - 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 2, sideLength / 2]);

        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 3, sideLength / 2 + 1]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 3, sideLength / 2 + 2]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 3, sideLength / 2 + 3]);
        listOfHexForTreePlayerTwo.Add(map[map.GetLength(0) - 1 - 3, sideLength / 2]);
    }
}
