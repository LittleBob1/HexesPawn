using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IDamagable, IWhosePlayer, IParametresOfPawns
{
    private int indexRow;
    private int indexCell;
    private int health;
    private int armor;
    private Player pawnThisPlayer;
    private StartGameTwoPlayer playerLogic;
    private CanvasesController myCanvas;

    private GameObject[,] map;
    private List<GameObject> portalHex = new List<GameObject>();

    public void Initialization(GameObject[,] map)
    {
        this.map = map;
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        myCanvas = transform.GetChild(0).GetComponent<CanvasesController>();
    }

    public void SetIndexesPortal(int indexRow, int indexCell, GameObject[,] map)
    {
        this.indexRow = indexRow;
        this.indexCell = indexCell;

        CellPortals(map);
    }
    public void SetHealth(int health)
    {
        this.health = health;
    }
    public void SetArmor(int armor)
    {
        this.armor = armor;
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
            pawnThisPlayer.removePortal(gameObject.GetComponent<Portal>());
            pawnThisPlayer.removeObject(gameObject);
            map[indexRow, indexCell].GetComponent<HexLogic>().isEmpty = true;
            map[indexRow, indexCell].GetComponent<HexLogic>().SetObjOnHex(null);
            Destroy(gameObject);
            playerLogic.currentPlayer.setGold(playerLogic.currentPlayer.getGold() + 15);
        }
        myCanvas.RecalculationParameters();
    }
    public void setPlayer(Player pawnThisPlayer)
    {
        this.pawnThisPlayer = pawnThisPlayer;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetArmor()
    {
        return armor;
    }
    public int GetRow()
    {
        return indexRow;
    }

    public int GetCell()
    {
        return indexCell;
    }

    public List<GameObject> GetPortalHexes()
    {
        return portalHex;
    }

    private void CellPortals(GameObject[,] map)
    {
        if (indexRow == map.GetLength(0) / 2)
        {
            IllumMiddlePortal(map);
        }
        else if (indexRow >= map.GetLength(0) / 2 + 1)
        {
            IllumUpPortal(map);
        }
        else if (indexRow <= map.GetLength(0) / 2 - 1)
        {
            IllumDownPortal(map);
        }
    }

    private void IllumMiddlePortal(GameObject[,] map)
    {
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell - 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell - 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell + 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell - 1]);
        }
        catch { }


    }

    private void IllumUpPortal(GameObject[,] map)
    {
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell + 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell - 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell + 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell - 1]);
        }
        catch { }
    }

    private void IllumDownPortal(GameObject[,] map)
    {
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell - 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow - 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow + 1, indexCell + 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell + 1]);
        }
        catch { }
        try
        {
            portalHex.Add(map[indexRow, indexCell - 1]);
        }
        catch { }
    }
}
