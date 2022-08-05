using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] Health health;
    [SerializeField] private GameObject unitPrefab;
   // [SerializeField] private GameObject redUnitPrefab;
    //[SerializeField] private GameObject blueUnitPrefab;
    [SerializeField] private Transform spanwnerPosition;

    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }

    [Server]
    void ServerHandleDie()
    {
        //NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void CmdSpawnUnit()
    {
        
        GameObject unitInstance = Instantiate(unitPrefab, spanwnerPosition.position, spanwnerPosition.rotation);
    
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    /*GameObject Color()
    {     
        if(netIdentity.netId == 2) { return blueUnitPrefab; }

        return redUnitPrefab;
    } */
   
    #endregion

    #region Client

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left) { return; }

        if (!hasAuthority) { return; }

        CmdSpawnUnit();
    }

    #endregion

}
