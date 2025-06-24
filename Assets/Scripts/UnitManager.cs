using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitManager : MonoBehaviour
{
    //Since BuildingManager inherits from UnitManager, selectionCircle needs to be attached to the BuildingManager script in the inspector
    public GameObject selectionCircle;
    private DefaultControls _defaultControls;
    private Ray _ray;
    private RaycastHit _raycastHit;

    private void Awake()
    {
        _defaultControls = new DefaultControls();
    }

    private void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }

    private void OnEnable()
    {
        _defaultControls.UI.Click.canceled += OnClick;
        _defaultControls.UI.Click.Enable();
    }

    private void OnDisable()
    {
        _defaultControls.UI.Click.Disable();
        _defaultControls.UI.Click.canceled -= OnClick;
    }
    void OnClick(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(_ray, out _raycastHit, 1000f) &&
        _raycastHit.collider.gameObject == gameObject)
        {
            Debug.Log("Unit clicked: " + gameObject.name);
            if (IsActive())
                Select(true);
        }
    }

    protected virtual bool IsActive()
    {
        return true; // Default implementation, can be overridden
    }
    public void Select() { Select(false); }
    public void Select(bool clearSelection)
    {
        Debug.Log("Selecting unit: " + gameObject.name);
        if (Globals.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitManager> selectedUnits = new List<UnitManager>(Globals.SELECTED_UNITS);
            foreach (UnitManager um in selectedUnits)
                um.Deselect();
        }
        Globals.SELECTED_UNITS.Add(this);
        selectionCircle.SetActive(true);
    }

    public void Deselect()
    {
        if (!Globals.SELECTED_UNITS.Contains(this)) return;
        Globals.SELECTED_UNITS.Remove(this);
        selectionCircle.SetActive(false);
    }
}