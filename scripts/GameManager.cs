using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	
	public Timer globalTimer;

	public static List<BP_Sector> _allSectors = new List<BP_Sector>();
    public static List<BP_Station> _allBuildings = new List<BP_Station>();
	public static Dictionary<string, BP_Ship> _allShips = new Dictionary<string, BP_Ship>();
	public static Dictionary<string, Jumpgate> _allJumpgates = new Dictionary<string, Jumpgate>();

    private PackedScene currentScene;
	private Node currentSectorInstance;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadSectorScene("res://scenes/Sectors/Alpha_Sector.tscn");

		Instance = this;

		globalTimer = new Timer();
		//globalTimer.Autostart = true;
		globalTimer.WaitTime = 1.0f;
		globalTimer.OneShot = false;
		globalTimer.Timeout += OnGlobalTimerTick;
		AddChild(globalTimer);
		globalTimer.Start();
	}

	public void CreateSectors()
	{
		
	}

	public void OnGlobalTimerTick()
	{
		UpdateAllSectors();
		Debug.WriteLine("Tick!");
	}

    public void UpdateAllSectors()
    {

    }

	public void AddSector(BP_Sector sector)
	{
        _allSectors.Add(sector);
	}

    public void LoadSectorScene(string scenePath)
    {
        if (currentSectorInstance != null)
        {
            currentSectorInstance.QueueFree();  
        }

        PackedScene sectorScene = (PackedScene)ResourceLoader.Load(scenePath);

        if (sectorScene != null)
        {
            AddChild(currentSectorInstance);  
        }
    }
}
