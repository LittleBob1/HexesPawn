using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexLogic : MonoBehaviour
{
    public int indexRow;
    public int indexCell;
    public bool isEmpty = true;
    private GameObject objOnHex;

    public void SetObjOnHex(GameObject obj)
    {
        objOnHex = obj;
    }

    public GameObject GetObjOnHex()
    {
        return objOnHex;
    }
}
