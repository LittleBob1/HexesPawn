using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimulationMap : MonoBehaviour
{
    public GameObject Hex;
    public GameObject Tree;
    public int sideLength = 9;

    private Vector3 bounds;
    private int hex = 1;
    private int minusRow = 0;
    public GameObject[,] map;
    private RotateAroundAndZoom rot;
    private StartGameTwoPlayer playerLogic;
    private MaterialsContainer resourses;
    private int centerRow;

    public static Action<GameObject[,]> GetMap;

    void Start()
    {
        playerLogic = GameObject.Find("playerLogic").GetComponent<StartGameTwoPlayer>();
        rot = GameObject.Find("MainCamera").GetComponent<RotateAroundAndZoom>();
        resourses = GameObject.Find("ResoursesContainer").GetComponent<MaterialsContainer>();

        map = new GameObject[sideLength * 2 - 1, sideLength * 2 - 1];
        minusRow = 0;
        bounds = Hex.GetComponent<Renderer>().bounds.extents;
        for(int z = 0; z < sideLength * 2 - 1; z++)
        {
            
            if(z - sideLength == -1)
            {  
                centerRow = z;        
            }
            if(z >= sideLength)
            {
                Hex.transform.position = new Vector3(Hex.transform.position.x + bounds.x, Hex.transform.position.y, Hex.transform.position.z);
            }
            else
            {
                Hex.transform.position = new Vector3(Hex.transform.position.x - bounds.x, Hex.transform.position.y, Hex.transform.position.z);
            }
            int a = RowsCount(z);
            for (int x = 0; x < a; x++)
            {
                if(x == 0)
                {
                    hex = 1;
                }
                else
                {
                    hex++;
                }
                GameObject hexGame = Instantiate(Hex, new Vector3(XPosition(), Hex.transform.position.y, ZPosition(z)), Quaternion.Euler(-90, 0, 0));
                map[z, x] = hexGame;
                HexLogic hexlogic = hexGame.GetComponent<HexLogic>();
                hexlogic.indexRow = z;
                hexlogic.indexCell = x;
                hexlogic.isEmpty = true;
            }
        }

        string s = "";
        for(int i = 0; i < sideLength * 2 - 1; i++)
        {
            s += "\n";
            for (int j = 0; j < sideLength * 2 - 1; j++)
            {
                if(map[i,j] != null)
                {
                    s += i.ToString() + j.ToString() + " ";
                }      
            }
        }
        //Debug.Log(s);

        rot.target = map[centerRow, (sideLength * 2 - 1) / 2].transform;

        CreatePlayersTrees();

        Destroy(Hex);
        GetMap?.Invoke(map);
    }

    private void CreatePlayersTrees()
    {
        GameObject treeOne = Instantiate(Tree, map[0, sideLength / 2].transform.position, Quaternion.Euler(-90, 0, 90));
        treeOne.GetComponent<MyTree>().setPlayer(playerLogic.GetPlayerOne());
        treeOne.GetComponent<MyTree>().setArmor(1);
        treeOne.GetComponent<MyTree>().setHealth(6);
        treeOne.GetComponent<MyTree>().SetRow(0);
        treeOne.GetComponent<MyTree>().SetCell(sideLength / 2);
        treeOne.GetComponent<MeshRenderer>().material = resourses.DefaultPawnFirstPlayer;
        map[0, sideLength / 2].GetComponent<HexLogic>().isEmpty = false;
        map[0, sideLength / 2].GetComponent<HexLogic>().SetObjOnHex(treeOne);

        GameObject treeTwo = Instantiate(Tree, map[map.GetLength(0) - 1, sideLength / 2].transform.position, Quaternion.Euler(-90, 0, 90));
        treeTwo.GetComponent<MyTree>().setPlayer(playerLogic.GetPlayerTwo());
        treeTwo.GetComponent<MyTree>().setArmor(1);
        treeTwo.GetComponent<MyTree>().setHealth(6);
        treeTwo.GetComponent<MyTree>().setHealth(6);
        treeTwo.GetComponent<MyTree>().SetRow(map.GetLength(0) - 1);
        treeTwo.GetComponent<MyTree>().SetCell(sideLength / 2);
        treeTwo.GetComponent<MeshRenderer>().material = resourses.DefaultPawnSecondPlayer;
        map[map.GetLength(0) - 1, sideLength / 2].GetComponent<HexLogic>().isEmpty = false;
        map[map.GetLength(0) - 1, sideLength / 2].GetComponent<HexLogic>().SetObjOnHex(treeTwo);

        playerLogic.SetTrees(treeOne.GetComponent<MyTree>(), treeTwo.GetComponent<MyTree>());
    }

    private int RowsCount(int rows)
    {
        if (rows >= sideLength)
        {
            minusRow+=2;
            rows -= minusRow;
        }
        return sideLength + rows;
    }

    private float XPosition()
    {
        float xPos = Hex.transform.position.x + (bounds.x * 2 * hex);
        return xPos;
    }

    private float ZPosition(int row)
    {
        float zPos = Hex.transform.position.z + (bounds.z * row + bounds.x * 2 / Mathf.Sqrt(3) / 2 * row);
        return zPos;
    }
}
