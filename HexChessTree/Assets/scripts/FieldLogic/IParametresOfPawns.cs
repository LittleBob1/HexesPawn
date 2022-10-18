using UnityEngine;
interface IParametresOfPawns
{
    int GetHealth();
    int GetArmor();
    int GetDamage() => 0;
    int GetRow();
    int GetCell();
}
