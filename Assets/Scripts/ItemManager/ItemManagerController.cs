using UnityEngine;

public class ItemManagerController : MonoBehaviour
{

    protected enum EItem { Wind, Attck, Heart, ReloadSpeed, Healthmax, Resurrection }
    private static string baseItemLocation = "Raid/Item/";
    public string[] ItemLocation = new string[]
    {
            baseItemLocation+"WindItem",
            baseItemLocation+"AttackItem",
            baseItemLocation+"HeartItem",
            baseItemLocation+"HealthmaxItem",
            baseItemLocation+"ReloadSpeedItem",
            baseItemLocation+"ResurrectionItem",

    };
}