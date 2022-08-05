using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Building[] buildings;

    [SerializeField] private List<Unit> UnitsList = new List<Unit>();
    [SerializeField] private List<Building> BuildingsList = new List<Building>();

    public List<Unit> GetMyUnits()
    {
        return UnitsList;
    }
    public List<Building> GetMyBuildings()
    {
        return BuildingsList;
    }


    #region Server

    public override void OnStartServer()
    {
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned += ServerHandleUnitDespawned;

        Building.ServerOnBuildingDespawned += ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned += ServerHandleBuildingDespawned;
    }
    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDespawned -= ServerHandleUnitDespawned;

        Building.ServerOnBuildingDespawned -= ServerHandleBuildingSpawned;
        Building.ServerOnBuildingDespawned -= ServerHandleBuildingDespawned;
    }

    [Command]
    public void CmdTryPlaceBuilding(int buildingID, Vector3 point)
    {
        Building buildingToPlace = null;
       
        foreach (Building building in buildings)
        {
            if(building.GetID()== buildingID)
            {
                Debug.Log("entrei");
                buildingToPlace = building;
                break;
            }
        }

        if(buildingToPlace == null) { return; }

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);

    }

    void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        UnitsList.Add(unit);
    }
    void ServerHandleUnitDespawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        UnitsList.Remove(unit);
    }

    void ServerHandleBuildingSpawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        BuildingsList.Add(building);
    }
    void ServerHandleBuildingDespawned(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        BuildingsList.Remove(building);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        if (NetworkServer.active) { return; }

        Unit.AuthorityOnUnitSpawned += AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned += AuthorityHandleUnitDespawned;

        Building.AuthorityOnBuildingSpawned += AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingDespawned += AuthorityHandleBuildingDespawned;
    }
    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority)   { return; }

        Unit.AuthorityOnUnitSpawned -= AuthorityHandleUnitSpawned;
        Unit.AuthorityOnUnitDespawned -= AuthorityHandleUnitDespawned;

        Building.AuthorityOnBuildingSpawned -= AuthorityHandleBuildingSpawned;
        Building.AuthorityOnBuildingDespawned -= AuthorityHandleBuildingDespawned;
    }

    void AuthorityHandleUnitSpawned(Unit unit)
    {
        UnitsList.Add(unit);
    }
    void AuthorityHandleUnitDespawned(Unit unit)
    {
        UnitsList.Remove(unit);
    }

    void AuthorityHandleBuildingSpawned(Building building)
    {
        BuildingsList.Add(building);
    }
    void AuthorityHandleBuildingDespawned(Building building)
    {
        BuildingsList.Remove(building);
    }

    #endregion
}
