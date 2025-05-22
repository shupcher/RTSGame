/*
Global variable script that's functioning as a stand-in for single data file.
*/
public class Globals
{

    public static BuildingData[] BUILDING_DATA = new BuildingData[]
    {
        new BuildingData("Building", 100)
    };

//Since the layer needs to be represented in binary use bitmasking (a list of all the layers with either a 0 or 1 to represent if they're part of the mask)
    public static int TERRAIN_LAYER_MASK = 1 << 8;

}