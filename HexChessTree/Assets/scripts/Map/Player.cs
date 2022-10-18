using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{

    private string name;
    private int gold;

    //трон
    private MyTree myTree;

    //пешки
    private List<Defender> listOfDefenders = new List<Defender>();
    private List<Attacker> listOfAttacker = new List<Attacker>();
    private List<Pawns> listOfPawns = new List<Pawns>();
    private List<Portal> listOfPortals = new List<Portal>();
    private List<GameObject> allObjects = new List<GameObject>();


    public Player(string name)
    {
        this.name = name;
    }

    public string getName()
    {
        return name;
    }

    public int getGold()
    {
        return gold;
    }

    public void setGold(int gold)
    {
        this.gold = gold;
    }

    public void setMyTree(MyTree myTree)
    {
        this.myTree = myTree;
    }
    public MyTree getMyTree()
    {
        return myTree;
    }

    public List<Portal> getListOfPortals()
    {
        return listOfPortals;
    }

    public List<Defender> getListOfDefender()
    {
        return listOfDefenders;
    }

    public List<Attacker> getListOfAttacker()
    {
        return listOfAttacker;
    }

    public List<Pawns> getListOfPawns()
    {
        return listOfPawns;
    }
    public List<GameObject> getAllObjects()
    {
        return allObjects;
    }

    public void addPawn(Pawns pawn)
    {
        listOfPawns.Add(pawn);
    }

    public void removePawn(Pawns pawn)
    {
        listOfPawns.Remove(pawn);
    }

    public void addDefender(Defender def)
    {
        listOfDefenders.Add(def);
    }

    public void removeDefender(Defender def)
    {
        listOfDefenders.Remove(def);
    }

    public void addAttacker(Attacker ata)
    {
        listOfAttacker.Add(ata);
    }

    public void removeAttacker(Attacker ata)
    {
        listOfAttacker.Remove(ata);
    }

    public void addPortal(Portal port)
    {
        listOfPortals.Add(port);
    }

    public void removePortal(Portal port)
    {
        listOfPortals.Remove(port);
    }

    public void addObject(GameObject obj)
    {
        allObjects.Add(obj);
    }
    public void removeObject(GameObject obj)
    {
        allObjects.Remove(obj);
    }
}
