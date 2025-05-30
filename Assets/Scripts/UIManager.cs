using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private BuildingPlacer _buildingPlacer;

    public Transform buildingMenu;
    public GameObject buildingButtonPrefab;

    private void Awake()
    {
        _buildingPlacer = GetComponent<BuildingPlacer>();

        // create buttons for each building type
        for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
        {
            GameObject button = GameObject.Instantiate(
                buildingButtonPrefab,
                buildingMenu);
            string code = Globals.BUILDING_DATA[i].Code;
            button.name = code;
            button.GetComponentInChildren<Text>().text = code;
            Button b = button.GetComponent<Button>();
            _AddBuildingButtonListener(b, i);
        }
    }

    private void _AddBuildingButtonListener(Button b, int i)
    {
//Outside for loop above because as the for loop iterates, the value i would update. We need to call the method in the loop instead so we only get the value of i at the time it is called
        b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
    }
}