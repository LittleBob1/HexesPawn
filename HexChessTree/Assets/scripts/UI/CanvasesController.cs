using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasesController : MonoBehaviour
{
    private void Start()
    {
        TMP_Text tt = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        int hp = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetHealth();
        int armor = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetArmor();

        tt.text = "HP " + hp + " A " + armor;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
