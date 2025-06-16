using NUnit.Framework.Internal;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;


public class BuildingPlacer : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction _placeBuildingAction;
    private InputAction _cancelBuildingAction;
    private Building _placedBuilding = null;
    private Ray _ray;
    private RaycastHit _raycastHit;
    private Vector3 _lastPlacementPosition;
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager component not found on BuildingPlacer.");
        }
    }
    public void SelectPlacedBuilding(int buildingDataIndex)
    {
        _PreparePlacedBuilding(buildingDataIndex);
    }

    void Start()
    {
        _placeBuildingAction = actions.FindActionMap("UI").FindAction("Click"); 
       _cancelBuildingAction = actions.FindActionMap("UI").FindAction("Cancel");

        // Enable the actions
        _placeBuildingAction.Enable();
        _cancelBuildingAction.Enable();

        // Initialize the placed building to null
        _placedBuilding = null;
        _lastPlacementPosition = Vector3.zero;

        // Check if UIManager is set
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is not set in BuildingPlacer.");
        }
    }

    void Update()
    {
        if (_placedBuilding != null)
        {
            if (_cancelBuildingAction.WasPerformedThisFrame())
            {
                _CancelPlacedBuilding();
                return;
            }

            if (_placedBuilding.HasValidPlacement && _placeBuildingAction.WasReleasedThisFrame())
            {
                _PlaceBuilding();
            }

            _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            //Casts a ray from the camera to the point clicked and outputs a hit if there is one
            if (Physics.Raycast(_ray, out _raycastHit, 1000f, Globals.TERRAIN_LAYER_MASK) && _placedBuilding != null)
            {
                //Moves the phantom building to position of raycast hit
                _placedBuilding.SetPosition(_raycastHit.point);
                //If it's a new spot, check that the new spot is valid
                if (_lastPlacementPosition != _raycastHit.point)
                {
                    _placedBuilding.CheckValidPlacement();
                }
                _lastPlacementPosition = _raycastHit.point;
            }
        }
    }

    void _PreparePlacedBuilding(int buildingDataIndex)
    {
        // destroy the previous "phantom" if there is one
        if (_placedBuilding != null && !_placedBuilding.IsFixed)
        {
            Destroy(_placedBuilding.Transform.gameObject);
        }
        Building building = new Building(
            Globals.BUILDING_DATA[buildingDataIndex]
        );
        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;

        // link the data into the manager
        building.Transform.GetComponent<BuildingManager>().Initialize(building);
        _placedBuilding = building;
        _lastPlacementPosition = Vector3.zero;
    }

    void _CancelPlacedBuilding()
    {
        // destroy the "phantom" building
        Destroy(_placedBuilding.Transform.gameObject);
        _placedBuilding = null;
    }

    void _PlaceBuilding()
    {
        _placedBuilding.Place();
        // keep on building the same building type
        if (_placedBuilding.CanBuy())
            _PreparePlacedBuilding(_placedBuilding.DataIndex);
        else
            _placedBuilding = null;
        // update the UI to reflect the new resource amounts
        _uiManager.UpdateResourceTexts();
        //update the UI to reflect which buildings can be placed
        _uiManager.CheckBuildingButtons();
    }
}