using Util.Division;

namespace Communication.JsonFormat
{
    public class MapInfo
    {
    private int[,,] labylinthArray;
    private int[,] patrolPoint;
    private string[,] trap;
    private Division2 clearItem;

    public int[,,] LabylinthArray{get=>labylinthArray;set=>labylinthArray=value;}
    public int[,] PatrolPoint{get=>patrolPoint;set=>patrolPoint=value;}
    public string[,] Trap{get=>trap;set=>trap=value;}
    public Division2 ClearItem{get=>clearItem;set=>clearItem=value;}
    }
}