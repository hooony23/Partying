using Util.Division;

namespace Communication.JsonFormat
{
    public class MapInfo
    {
    public int[,,] labylinthArray;
    public int[,] patrolPoint;
    public string[,] trap;
    public Division2 clearItem;

    public int[,,] LabylinthArray{get=>labylinthArray;set=>labylinthArray=value;}
    public int[,] PatrolPoint{get=>patrolPoint;set=>patrolPoint=value;}
    public string[,] Trap{get=>trap;set=>trap=value;}
    public Division2 ClearItem{get=>clearItem;set=>clearItem=value;}
    }
}