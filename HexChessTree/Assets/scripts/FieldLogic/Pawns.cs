using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawns : MonoBehaviour
{
    public abstract void ListRecalculation();
    public abstract void Move(GameObject hex);
    public abstract void Attack(GameObject currentOpponentObj); 
    public abstract void CellIllumination();
    public abstract void SetIndexes(int indexRow, int indexCell);
    public abstract void Initialization(GameObject[,] map);
    public abstract void SetBoolIsNotMoved();
    public abstract bool GetBoolMoved();
    public abstract void SetLvl(int level, GameObject textUpg);
    public abstract int GetLvl();
    public abstract void setHealth(int health);
    public abstract int getHealth();
    public abstract void setArmor(int armor);
    public abstract int getArmor();
    public abstract void setDamage(int damage);
    public abstract int getDamage();
    public abstract void setPlayer(Player pawnThisPlayer);

}
