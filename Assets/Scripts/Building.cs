using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Creates an instance of the building prefab using the BuildingData class()
*/
public class Building
{

    private BuildingData _data;
    private Transform _transform;
    private int _currentHealth;

    //Constructs an instance of a building with name Code by pulling public getter data from BuildingData "abstract" class and initializing the building with those values.
    public Building(BuildingData data)
    {
        _data = data;
        _currentHealth = data.HP;

        GameObject g = GameObject.Instantiate(
            Resources.Load($"Prefabs/Buildings/{_data.Code}")
        ) as GameObject;
        _transform = g.transform;
    }

    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }

    public string Code { get => _data.Code; }
    public Transform Transform { get => _transform; }
    //Public HP allowed to set in case we want to update the value from outside the class
    public int HP { get => _currentHealth; set => _currentHealth = value; }
    public int MaxHP { get => _data.HP; }
    //DataIndex is a "computed" property taht gives us the index of teh building type data instance.
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