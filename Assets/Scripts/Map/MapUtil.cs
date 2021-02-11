using UnityEngine;
using Partying.Assets.Scripts.Util;
public class MapUtil : MonoBehaviour
{
    protected MapInfo mapInfo;
    protected MapObjectInfo mapObjectInfo;
    protected CreateMap createMap;

    public MapUtil()
    {
        mapInfo = new MapInfo();
        mapObjectInfo = new MapObjectInfo();
        createMap = new CreateMap(mapInfo, mapObjectInfo, Config.ROW, Config.COL);
    }
}