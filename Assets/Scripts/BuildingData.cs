using System.Collections.Generic;
/*
Used for storing generic building data including the code (ID) that identifies the specific building as well as its HP
*/

//Defines what a BuildingData object is and what it can hold
public class BuildingData
{
    private string _code;
    private int _healthpoints;
    private Dictionary<string, int> _cost;

    //Constructor code, initializes the code and HP of a newly constructed building.
    public BuildingData(string code, int healthpoints, Dictionary<string, int> cost)
    {
        _code = code;
        _healthpoints = healthpoints;
        _cost = cost;
    }

    public bool CanBuy()
    {
        foreach (KeyValuePair<string, int> pair in _cost)
        {
            if (Globals.GAME_RESOURCES[pair.Key].Amount < pair.Value)
            {
                return false;
            }
        }
        return true;
    }

    public string Code { get => _code; }
    public int HP { get => _healthpoints; }
    public Dictionary<string, int> Cost { get => _cost; }
}