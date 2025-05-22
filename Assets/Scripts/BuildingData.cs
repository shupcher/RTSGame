using System.Collections.Generic;
/*
Used for storing generic building data including the code (ID) that identifies the specific building as well as its HP
*/

//Defines what a BuildingData object is and what it can hold
public class BuildingData
{
    private string _code;
    private int _healthpoints;

    //Constructor code, initializes the code and HP of a newly constructed building.
    public BuildingData(string code, int healthpoints)
    {
        _code = code;
        _healthpoints = healthpoints;
    }

    public string Code { get => _code; }
    public int HP { get => _healthpoints; }
}