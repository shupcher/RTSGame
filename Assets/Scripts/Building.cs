using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Creates an instance of the building prefab using the BuildingData class()
*/
public enum BuildingPlacement
{
    VALID,
    INVALID,
    FIXED
};

//Constructs an instance of a building with namecode by pulling public getter data from BuildingData "abstract" class and initializing the building with those values.
public class Building
{

    private BuildingData _data;
    private Transform _transform;
    private BuildingPlacement _placement;
    private BuildingManager _buildingManager;
    private List<Material> _materials;
    private int _currentHealth;

    public Building(BuildingData data)
    {
        _data = data;
        _currentHealth = data.HP;
        _materials = new List<Material>();

        GameObject g = GameObject.Instantiate(
            Resources.Load($"Prefabs/Buildings/{_data.Code}")

        ) as GameObject;
        _transform = g.transform;
        // set building mode as "valid" placement
        _buildingManager = g.GetComponent<BuildingManager>();
        _placement = BuildingPlacement.VALID;

        //Saves prefabs material in order to restore the building's original appearance when its BuildingPlacement state becomes FIXED
        foreach (Material material in _transform.Find("Mesh").GetComponentInChildren<MeshRenderer>().materials)
        {
            _materials.Add(new Material(material));
            var renderer = _transform.Find("Mesh").GetComponentInChildren<MeshRenderer>();
            Debug.Log("Initial material count: " + renderer.materials.Length);

        }
        SetMaterials();
    }

    public void SetMaterials() { SetMaterials(_placement); }
    public void SetMaterials(BuildingPlacement placement)
    {
        List<Material> materials;
        //New list of materials and populates it with this single "Valid" material,  replacing all original materials
        if (placement == BuildingPlacement.VALID)
        {
            Material refMaterial = Resources.Load("Materials/Valid") as Material;
            //Debug.Log("Loaded material: " + refMaterial);
            materials = new List<Material>();
            //Iterates over all materials to replace them all with the same one
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }

        else if (placement == BuildingPlacement.INVALID)
        {
            Material refMaterial = Resources.Load("Materials/Invalid") as Material;
            //Debug.Log("Loaded material: " + refMaterial);
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
            {
                materials.Add(refMaterial);
            }
        }
        //Once building is placed (fixed) replace its materials with the original prefab's materials
        else if (placement == BuildingPlacement.FIXED)
        {
            materials = _materials;
        }
        else
        {
            return;
        }
        _transform.Find("Mesh").GetComponentInChildren<MeshRenderer>().materials = materials.ToArray();
    }

    public void Place()
    {
        // set placement state
        _placement = BuildingPlacement.FIXED;

        // remove "is trigger" flag from box collider to allow
        // for collisions with units
        _transform.GetComponent<BoxCollider>().isTrigger = false;

        //Sets material based on placement state
        SetMaterials();

        foreach (KeyValuePair<string, int> pair in _data.Cost)
        {
            //Removes the cost of the building from the resource pool
            Globals.GAME_RESOURCES[pair.Key].AddAmount(-pair.Value);
        }
    }

    public bool CanBuy()
    {
        //Checks if the building can be bought by checking the cost of the building against the current resource pool
        return _data.CanBuy();
    }
    public void CheckValidPlacement()
    {
        if (_placement == BuildingPlacement.FIXED) return;
        _placement = _buildingManager.CheckPlacement()
        ? BuildingPlacement.VALID
        : BuildingPlacement.INVALID;
    }

    public bool IsFixed { get => _placement == BuildingPlacement.FIXED; }
    public bool HasValidPlacement { get => _placement == BuildingPlacement.VALID; }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public string Code { get => _data.Code; }
    public Transform Transform { get => _transform; }
    //Public HP allowed to set in case we want to update the value from outside the class
    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHP { get => _data.HP; }
    //DataIndex is a "computed" property taht gives us the index of the building type data instance.
    public int DataIndex
    {
        get
        {
            for (int i = 0; i < Globals.BUILDING_DATA.Length; i++)
            {
                if (Globals.BUILDING_DATA[i].Code == _data.Code)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}