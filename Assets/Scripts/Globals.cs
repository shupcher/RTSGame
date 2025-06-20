using System.Collections.Generic;

/*
Global variable script that's functioning as a stand-in for single data file.
*/
public class Globals
{

    public static BuildingData[] BUILDING_DATA = new BuildingData[]
    {
        new BuildingData("House", 100, new Dictionary<string, int>()
        {
            //Key, value = Resource, amount
            { "palladium", 125 }
        }),
        new BuildingData("Tower", 200, new Dictionary<string, int>()
        {
            { "palladium", 200 },
            { "helium", 50 }
        })
    };

    //Since the layer needs to be represented in binary use bitmasking (a list of all the layers with either a 0 or 1 to represent if they're part of the mask)
    public static int TERRAIN_LAYER_MASK = 1 << 8;

    //Dictionary structure to hold resource list.
    public static Dictionary<string, GameResource> GAME_RESOURCES =
        new Dictionary<string, GameResource>()
    {
        //Key, value = Game Resource, initial value
        { "palladium", new GameResource("Palladium", 501) },
        { "helium", new GameResource("Helium", 500) },
        { "alien", new GameResource("Alien", 0) }
    };

     public static List<UnitManager> SELECTED_UNITS = new List<UnitManager>();

}