using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTree : MonoBehaviour, IDamagable, IWhosePlayer, IParametresOfPawns
{
    private Outline line;
    private int health;
    private int armor;
    private Player pawnThisPlayer;

    private void Start()
    {
        line = GetComponent<Outline>();
    }
    public void setPlayer(Player pawnThisPlayer)
    {
        this.pawnThisPlayer = pawnThisPlayer;
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
        health -= damage;
    }

    public Player GetPlayersTown()
    {
        return pawnThisPlayer;
    }
}
