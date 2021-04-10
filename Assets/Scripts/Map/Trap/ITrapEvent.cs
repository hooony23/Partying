using UnityEngine;

public interface ITrapEvent
{
    void TrapEvent(Collider col,params object[] param);
}