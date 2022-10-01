using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Attacker : Pawns, IDamagable, IWhosePlayer, IParametresOfPawns
{
    public int indexRow;
    public int indexCell;
    public bool moveIsMade = false;

    private int health;
    private int armor;
    private int damage;
    private Player pawnThisPlayer;

    private List<GameObject> listOfHex = new List<GameObject>();
    private List<GameObject> listOfHexPortal = new List<GameObject>();
    private GameObject[,] map;

    private ButtonLogicBuy logBut;
    private StartGameTwoPlayer playerLogic;
    private MaterialsContainer resourses;

    public override void Initialization(GameObject[,] map)
    {
        this.map = map;
        logBut = GameObject.Find("ButtonsController").GetComponent<ButtonLogicBuy>();
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        resourses = GameObject.Find("ResoursesContainer").GetComponent<MaterialsContainer>();
    }
    public override bool GetBoolMoved()
    {
        return moveIsMade;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetArmor()
    {
        return armor;
    }
    public override void setDamage(int damage)
    {
        this.damage = damage;
    }
    public override int getDamage()
    {
        return damage;
    }
    public Player GetPlayersTown()
    {
        return pawnThisPlayer;
    }
    public void IDamage(int damage)
    {
        health -= damage;
        Debug.Log(this.gameObject + "damage");
    }
    public override void setPlayer(Player pawnThisPlayer)
    {
        this.pawnThisPlayer = pawnThisPlayer;
    }
    public override void SetBoolIsNotMoved()
    {
        moveIsMade = false;
    }
    public override void setHealth(int health)
    {
        this.health = health;
    }

    public override int getHealth()
    {
        return health;
    }

    public override void setArmor(int armor)
    {
        this.armor = armor;
    }

    public override int getArmor()
    {
        return armor;
    }

    public override void Attack(GameObject currentOpponentObj)
    {
        string[] s = currentOpponentObj.GetComponent<MeshRenderer>().material.name.Split(" ");

        if (s[0] == resourses.OpponentPawn.name)
        {
            gameObject.GetComponent<Outline>().enabled = false;
            moveIsMade = true;
            currentOpponentObj.GetComponent<IDamagable>().IDamage(damage);

            if (playerLogic.currentPlayer.getName() == "playerOne")
            {
                currentOpponentObj.GetComponent<MeshRenderer>().material = resourses.DefaultPawnSecondPlayer;
            }
            else
            {
                currentOpponentObj.GetComponent<MeshRenderer>().material = resourses.DefaultPawnFirstPlayer;
            }

            ActionAfterMove();
        }
    }
    private void ActionAfterMove()
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

        List<Pawns> pawns = new List<Pawns>();
        Material playerMaterial;

        if (playerLogic.currentPlayer.getName() == "playerOne")
        {
            pawns = playerLogic.GetPlayerTwo().getListOfPawns();
            playerMaterial = resourses.DefaultPawnSecondPlayer;
        }
        else
        {
            pawns = playerLogic.GetPlayerOne().getListOfPawns();
            playerMaterial = resourses.DefaultPawnFirstPlayer;
        }

        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i].gameObject.GetComponent<MeshRenderer>().material = playerMaterial;
        }
    }
    public override void Move(GameObject hex)
    {
        string[] s = hex.GetComponent<MeshRenderer>().material.name.Split(" ");

        if (s[0] == resourses.HexIllum.name && hex.GetComponent<HexLogic>().isEmpty == true)
        {
            moveIsMade = true;

            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = true;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(null);

            indexRow = hex.GetComponent<HexLogic>().indexRow;
            indexCell = hex.GetComponent<HexLogic>().indexCell;

            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = false;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(this.gameObject);
            ListRecalculation();
            transform.position = hex.transform.position;
            gameObject.GetComponent<Outline>().enabled = false;
            hex.GetComponent<HexLogic>().isEmpty = false;

            ActionAfterMove();
        }
        else if (s[0] == resourses.HexPortal.name && hex.GetComponent<HexLogic>().isEmpty == true)
        {
            moveIsMade = true;
            playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() - 10);
            logBut.textValue.text = "Value: " + playerLogic.currentPlayer.getGold().ToString();

            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = true;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(null);

            indexRow = hex.GetComponent<HexLogic>().indexRow;
            indexCell = hex.GetComponent<HexLogic>().indexCell;
            ListRecalculation();
            transform.position = hex.transform.position;
            gameObject.GetComponent<Outline>().enabled = false;
            hex.GetComponent<HexLogic>().isEmpty = false;

            ActionAfterMove();
        }
    }

    public override void SetIndexes(int indexRow, int indexCell)
    {
        this.indexRow = indexRow;
        this.indexCell = indexCell;
    }

    public override void ListRecalculation()
    {
        listOfHex.Clear();
        listOfHexPortal.Clear();

        if (indexRow == map.GetLength(0) / 2)
        {
            IllumMiddle(map);
        }
        else if (indexRow >= map.GetLength(0) / 2 + 1)
        {
            IllumUp(map);
        }
        else if (indexRow <= map.GetLength(0) / 2 - 1)
        {
            IllumDown(map);
        }

        List<int> indexThisPortal = new List<int>();

        for (int i = 0; i < playerLogic.currentPlayer.getListOfPortals().Count; i++)
        {
            List<GameObject> list1 = playerLogic.currentPlayer.getListOfPortals()[i].GetComponent<Portal>().GetPortalHexes();


            for (int g = 0; g < playerLogic.currentPlayer.getListOfPortals()[i].GetComponent<Portal>().GetPortalHexes().Count; g++)
            {
                if (map[indexRow, indexCell] == list1[g])
                {
                    indexThisPortal.Add(i);
                    break;
                }
            }
        }

        for (int i = 0; i < playerLogic.currentPlayer.getListOfPortals().Count; i++)
        {
            if (indexThisPortal.Count == 1)
            {
                List<GameObject> list1 = playerLogic.currentPlayer.getListOfPortals()[i].GetComponent<Portal>().GetPortalHexes();
                if (i != indexThisPortal[0])
                {
                    for (int j = 0; j < list1.Count; j++)
                    {
                        listOfHexPortal.Add(list1[j]);
                    }
                }
            }
            else
            {
                List<GameObject> list1 = playerLogic.currentPlayer.getListOfPortals()[i].GetComponent<Portal>().GetPortalHexes();

                for (int j = 0; j < list1.Count; j++)
                {
                    listOfHexPortal.Add(list1[j]);
                }

            }
        }

        if (indexThisPortal.Count == 0)
        {
            listOfHexPortal.Clear();
        }
    }

    public override void CellIllumination()
    {
        for (int i = 0; i < listOfHex.Count; i++)
        {
            if (listOfHex[i] != null)
            {
                if (listOfHex[i].GetComponent<HexLogic>().isEmpty == true)
                {
                    listOfHex[i].GetComponent<MeshRenderer>().material = resourses.HexIllum;
                }
                else
                {
                    if (listOfHex[i].GetComponent<HexLogic>().GetObjOnHex().GetComponent<IWhosePlayer>().GetPlayersTown() != playerLogic.currentPlayer)
                    {
                        listOfHex[i].GetComponent<HexLogic>().GetObjOnHex().GetComponent<MeshRenderer>().material = resourses.OpponentPawn;
                    }
                }
            }
        }
        for (int i = 0; i < listOfHexPortal.Count; i++)
        {
            if (listOfHexPortal[i] != null)
            {
                if (listOfHexPortal[i].GetComponent<HexLogic>().isEmpty == true)
                {
                    listOfHexPortal[i].GetComponent<MeshRenderer>().material = resourses.HexPortal;
                }
            }
        }

        for (int i = 0; i < listOfHex.Count; i++)
        {
            for (int j = 0; j < listOfHexPortal.Count; j++)
            {
                if (listOfHexPortal[j] != null && listOfHex[i] != null && listOfHexPortal[j].GetComponent<HexLogic>().isEmpty == true && listOfHex[i].GetComponent<HexLogic>().isEmpty == true)
                {
                    if (listOfHexPortal[j] == listOfHex[i])
                    {
                        listOfHexPortal[j].GetComponent<MeshRenderer>().material = resourses.HexIllum;
                    }
                }
            }
        }
    }

    public override int GetRow()
    {
        return indexRow;
    }

    public override int GetCell()
    {
        return indexCell;
    }

    private void IllumMiddle(GameObject[,] map)
    {
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell - 1]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell - 1]);
        }
        catch { }
    }

    private void IllumUp(GameObject[,] map)
    {
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell + 1]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell - 1]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
    }

    private void IllumDown(GameObject[,] map)
    {
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell - 1]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
        try
        {
            listOfHex.Add(map[indexRow + 1, indexCell + 1]);
        }
        catch { }
    }
}
