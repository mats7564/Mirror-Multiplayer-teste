using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] private Targetable target;

    public Targetable GetTarget()
    {
        return target;
    }

    public override void OnStartServer()
    {
        GameOver.ServerOnGameOver += ServerHandlerGameOver;
    }
    public override void OnStopServer()
    {
        GameOver.ServerOnGameOver -= ServerHandlerGameOver;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if(!targetGameObject.TryGetComponent<Targetable>(out Targetable newtarget)) { return; }

        target = newtarget;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }

    [Server]
    void ServerHandlerGameOver()
    {
        ClearTarget();
    }


}
