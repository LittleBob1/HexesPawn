using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Attacker : Pawns, IDamagable, IWhosePlayer, IParametresOfPawns
{
    public int indexRow;
    public int indexCell;
    public bool moveIsMade = false;

    private int health;
    private int armor;
    private int damage;
    private int level;
    private Player pawnThisPlayer;

    private List<GameObject> listOfHex = new List<GameObject>();
    private List<GameObject> listOfHexPortal = new List<GameObject>();
    private GameObject[,] map;

    private ButtonLogicBuy logBut;
    private StartGameTwoPlayer playerLogic;
    private MaterialsContainer resourses;
    private CanvasesController myCanvas;
    private OutlineChess outChess;

    private Dictionary<int, string> levels;

    public override void Initialization(GameObject[,] map)
    {
        this.map = map;
        logBut = GameObject.Find("ButtonsController").GetComponent<ButtonLogicBuy>();
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        resourses = GameObject.Find("ResoursesContainer").GetComponent<MaterialsContainer>();
        outChess = GameObject.Find("SetController").GetComponent<OutlineChess>();
        myCanvas = transform.GetChild(0).GetComponent<CanvasesController>();

        InitializeLvl();
    }
    private void InitializeLvl()
    {
        levels = new Dictionary<int, string>
        {
            {0, "2 0 3"},
            {1, "3 1 4 50"},
            {2, "3 1 5 70"},
        };
    }
    public override int GetLvl()
    {
        return level;
    }
    public override void SetLvl(int level, GameObject textUpg)
    {
        if (level == 0)
        {
            string[] s = levels[level].Split(" ");
            this.level = level;
            health = int.Parse(s[0]);
            armor = int.Parse(s[1]);
            damage = int.Parse(s[2]);
        }
        else if (level == -1)
        {
            if (this.level + 1 <= 2)
            {
                string[] lv = levels[this.level + 1].Split(" ");
                textUpg.GetComponent<TMP_Text>().text = "Upgrade to lvl " + (this.level + 1).ToString() + "\n" + lv[3];
            }
            else
            {
                textUpg.GetComponent<TMP_Text>().text = "MAX LVL";
            }
        }
        else if (level > 0)
        {
            if (level <= 2)
            {
                string[] lv = levels[level].Split(" ");

                if (playerLogic.currentPlayer.getGold() >= int.Parse(lv[3]))
                {
                    this.level = level;
                    health = int.Parse(lv[0]);
                    armor = int.Parse(lv[1]);
                    damage = int.Parse(lv[2]);
                    myCanvas.RecalculationParameters();
                    playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() - int.Parse(lv[3]));
                    logBut.UpdateGoldOnText();
                    if (level + 1 <= 2)
                    {
                        textUpg.GetComponent<TMP_Text>().text = "Upgrade to lvl " + (this.level + 1).ToString() + "\n" + lv[3];
                    }
                    else
                    {
                        textUpg.GetComponent<TMP_Text>().text = "MAX LVL";
                    }
                }
            }
            else
            {
                textUpg.GetComponent<TMP_Text>().text = "MAX LVL";
            }
        }
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
    public int GetDamage()
    {
        return damage;
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
        if (armor > 0)
        {
            armor--;
        }
        else
        {
            health -= damage;
        }
        if (health <= 0)
        {
            logBut.pawns.Remove(gameObject);
            pawnThisPlayer.removeAttacker(gameObject.GetComponent<Attacker>());
            pawnThisPlayer.removeObject(gameObject);
            pawnThisPlayer.removePawn(gameObject.GetComponent<Pawns>());
            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = true;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(null);
            playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() + 35);
            logBut.UpdateGoldOnText();
            Destroy(gameObject);
        }
        myCanvas.RecalculationParameters();
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
            int row = currentOpponentObj.GetComponent<IParametresOfPawns>().GetRow();
            int cell = currentOpponentObj.GetComponent<IParametresOfPawns>().GetCell();

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
            if (currentOpponentObj.GetComponent<IParametresOfPawns>().GetHealth() <= 0)
            {
                MoveAfterKill(map[row, cell]);
            }
            ActionAfterMove();
        }
    }
    private void MoveAfterKill(GameObject hex)
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

        List<GameObject> pawns = new List<GameObject>();
        Material playerMaterial;

        if (playerLogic.currentPlayer.getName() == "playerOne")
        {
            pawns = playerLogic.GetPlayerTwo().getAllObjects();
            playerMaterial = resourses.DefaultPawnSecondPlayer;
        }
        else
        {
            pawns = playerLogic.GetPlayerOne().getAllObjects();
            playerMaterial = resourses.DefaultPawnFirstPlayer;
        }

        for (int i = 0; i < pawns.Count; i++)
        {
            pawns[i].GetComponent<MeshRenderer>().material = playerMaterial;
        }

        outChess.panelUpgrade.SetActive(false);
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

            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = false;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(this.gameObject);

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

    public int GetRow()
    {
        return indexRow;
    }

    public int GetCell()
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
