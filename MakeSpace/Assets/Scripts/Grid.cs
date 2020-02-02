using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    const int BOARD_SIZE = 14;
    [HideInInspector]
    public Room[,] rooms = new Room[BOARD_SIZE,BOARD_SIZE];

    [HideInInspector]
    public float space_threshold = 0.1f, damaged_threshold = 0.4f,
        door_chance = 0.4f;

    [HideInInspector]
    public int num_oxygen_tanks = BOARD_SIZE/2, num_jetpack_fuel = BOARD_SIZE/2, num_inflatable_walls = BOARD_SIZE/4, num_eva_suit = BOARD_SIZE/BOARD_SIZE; //XDDDDDDDDD

    public GameObject RoomPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        InitializeFloors();
        InitializeWalls();
        InitializeItems();
    }

    void InitializeFloors()
    {
        int i = 0, size = rooms.GetLength(0);

        foreach (Room room in GetComponentsInChildren<Room>())
        {
            int x = i % size, y = (int)i / size;
            bool edge = x == 0 || x == (size - 1) || y == 0 || y == (size - 1);
            bool spawn = false;
            if (x == 1)
                if (y == 1 || y == (size - 2))
                    spawn = true;
            else if (x == (size - 2))
                if (y == 1 || y == (size - 2))
                    spawn = true;

            FloorType floorType = FloorType.Floor;
            if (edge)
            {
                floorType = FloorType.Space;
            }
            else if (spawn)
            {
                floorType = FloorType.Floor;
            }
            else
            {
                float seed = Random.Range(0, Mathf.Epsilon);
                float noise = Mathf.PerlinNoise(x / size - seed, y / size - seed);
                if (noise < space_threshold)
                {
                    floorType = FloorType.Space;
                }
                else if (noise < damaged_threshold)
                {
                    floorType = FloorType.Damaged;
                }
            }

            rooms[x, y] = room;
            room.Initialize(x, y, this, edge, spawn, floorType);
            i++;
        }
    }

    void InitializeWalls()
    {
        foreach (Room room in rooms)
        {
            room.InitializeWalls();
        }
    }

    void InitializeItems()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
