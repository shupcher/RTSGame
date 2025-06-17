using UnityEngine;
using UnityEngine.InputSystem;

public class UnitsSelection : MonoBehaviour
{
    private DefaultControls _defaultControls;
    private InputAction _selectUnitsAction;
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;

    private void Awake()
    {
        _defaultControls = new DefaultControls();
    }

    private void OnEnable()
    {
        _defaultControls.UI.Click.performed += OnPress;
        _defaultControls.UI.Click.canceled += OnRelease;
        _defaultControls.UI.Click.Enable();
    }

    private void OnDisable()
    {
        _defaultControls.UI.Click.Disable();
        _defaultControls.UI.Cancel.Disable();
    }

    void OnPress(InputAction.CallbackContext context)
    {
        _isDraggingMouseBox = true;
        _dragStartPosition = Mouse.current.position.ReadValue();
    }

    void OnRelease(InputAction.CallbackContext context)
    {
        _isDraggingMouseBox = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (_isDraggingMouseBox)
        {
            // Update the dragging box selection
            _SelectUnitsInDraggingBox();
        }

    }

    // Selects units in a rectangle defined by the start position and the current mouse position
    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = Utils.GetViewportBounds(
            Camera.main,
            _dragStartPosition,
            Mouse.current.position.ReadValue()
        );
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(
                Camera.main.WorldToViewportPoint(unit.transform.position)
            );
            if (inBounds)
            {
                unit.GetComponent<UnitManager>().Select();
            }
            else
                unit.GetComponent<UnitManager>().Deselect();
        }
    }

    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Mouse.current.position.ReadValue());
            Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
        }
    }

}