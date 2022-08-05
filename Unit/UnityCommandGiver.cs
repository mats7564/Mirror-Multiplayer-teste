using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler;
    [SerializeField] LayerMask layerMask;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        GameOver.ClientOnGameOver += ClientHandleGameOver;
    }
    private void OnDestroy()
    {
        GameOver.ClientOnGameOver -= ClientHandleGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        if(hit.collider.TryGetComponent<Targetable>(out Targetable target))
        {
            if (target.hasAuthority)
            {
                TryMove(hit.point);
                return;
            }

            TryTarget(target);
            return;
        }

        TryMove(hit.point);
    }

    void TryMove(Vector3 point)
    {
        foreach (Unit unit in unitSelectionHandler.SelectUnits)
        {
            unit.GetUnitMov().CmdMove(point);
        }
    }

    void TryTarget(Targetable target)
    {
        foreach (Unit unit in unitSelectionHandler.SelectUnits)
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }
    }
    void ClientHandleGameOver(string winnerName)
    {
        enabled = false;
    }
}
