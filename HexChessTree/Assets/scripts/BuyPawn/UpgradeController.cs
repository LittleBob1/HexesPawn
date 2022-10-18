using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeController : MonoBehaviour
{
    private GameObject currentPawn;

    public GameObject textUpg;
    public void UpgradePawn(GameObject currentPawn)
    {
       this.currentPawn = currentPawn;
       currentPawn.GetComponent<Pawns>().SetLvl(-1, textUpg);
    }

    public void UpgradeButton()
    {
       currentPawn.GetComponent<Pawns>().SetLvl(currentPawn.GetComponent<Pawns>().GetLvl() + 1, textUpg);
    }
}
