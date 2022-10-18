using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasesController : MonoBehaviour
{
    private TMP_Text tt;
    private void Start()
    {
        tt = transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        int hp = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetHealth();
        int armor = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetArmor();
        int damage = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetDamage();

        tt.text = "HP " + hp + " A " + armor + " D " + damage;
    }
    public void RecalculationParameters()
    {
        int hp = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetHealth();
        int armor = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetArmor();
        int damage = transform.parent.gameObject.GetComponent<IParametresOfPawns>().GetDamage();

        tt.text = "HP " + hp + " A " + armor + " D " + damage;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
