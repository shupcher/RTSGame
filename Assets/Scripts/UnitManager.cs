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

    private UnitsSelection _unitsSelection;

    private bool multiSelect = false;

    private void Awake()
    {
        _defaultControls = new DefaultControls();
        _unitsSelection = GameObject.Find("GAME").GetComponent<UnitsSelection>();
    }

    private void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    }

    private void OnEnable()
    {
        _defaultControls.UI.Click.performed += OnClick;
        _defaultControls.UI.Click.Enable();
        _defaultControls.UI.MultiSelect.started += OnMultiSelect;
        _defaultControls.UI.MultiSelect.canceled += OnMultiSelectEnd;
        _defaultControls.UI.MultiSelect.Enable();
    }

    private void OnDisable()
    {
        _defaultControls.UI.Click.Disable();
        _defaultControls.UI.Click.canceled -= OnClick;
        _defaultControls.UI.MultiSelect.Disable();
    }
    void OnClick(InputAction.CallbackContext context)
    {
        if (UnitsSelection.DragJustReleased || BuildingPlacer.BuildingJustPlaced)
            return; // Ignore clicks if the mouse drag just ended
        if (Physics.Raycast(_ray, out _raycastHit, 1000f) &&
        _raycastHit.collider.gameObject == gameObject)
        {
            if (!multiSelect)
            {
                if (IsActive())
                    Select(true);
            }
            else
            {
                if (IsActive())
                    Select(false);
            }
        }

        else if (!(_raycastHit.collider.gameObject.tag == "Unit"))
        {
            Deselect();
        }
    }

    void OnMultiSelect(InputAction.CallbackContext context)
    {
        multiSelect = true;
        _unitsSelection.enabled = false;
    }

    void OnMultiSelectEnd(InputAction.CallbackContext context)
    {
        multiSelect = false;
        _unitsSelection.enabled = true;
    }

    protected virtual bool IsActive()
    {
        return true; // Default implementation, can be overridden
    }
    public void Select() { Select(false); }
    public void Select(bool clearSelection)
    {
        //if (Globals.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            Debug.Log("Selection cleared");
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