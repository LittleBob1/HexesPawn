using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyTree : MonoBehaviour, IDamagable, IWhosePlayer, IParametresOfPawns
{
    private Outline line;
    private int health;
    private int armor;
    private int indexRow;
    private int indexCell;
    private Player pawnThisPlayer;
    private CanvasesController myCanvas;

    private void Start()
    {
        line = GetComponent<Outline>();
        myCanvas = transform.GetChild(0).GetComponent<CanvasesController>();
    }
    public void setPlayer(Player pawnThisPlayer)
    {
        this.pawnThisPlayer = pawnThisPlayer;
    }
    public int GetRow()
    {
        return indexRow;
    }
    public int GetCell()
    {
        return indexCell;
    }
    public void SetRow(int indexRow)
    {
        this.indexRow = indexRow;
    }
    public void SetCell(int indexCell)
    {
        this.indexCell = indexCell;
    }
    public void IllumOutline()
    {
        line.enabled = !line.enabled;
    }
    public void setHealth(int health)
    {
        this.health = health;
    }
    public void setArmor(int armor)
    {
        this.armor = armor;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetArmor()
    {
        return armor;
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
            SceneManager.LoadScene(0);
        }
        myCanvas.RecalculationParameters();
    }

    public Player GetPlayersTown()
    {
        return pawnThisPlayer;
    }
}
