using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    const int BOARD_SIZE = 12;
    [HideInInspector]
    public Room[,] rooms = new Room[BOARD_SIZE,BOARD_SIZE];

    float space_threshold = 0.2f;
    float damaged_threshold = 0.4f;

    [HideInInspector]
    public float door_chance = 0.4f;


    int num_oxygen_tanks = BOARD_SIZE / 2;
    int num_jetpack_fuel = BOARD_SIZE / 2;
    int num_inflatable_walls = BOARD_SIZE / 4;
    int num_eva_suit = BOARD_SIZE/BOARD_SIZE; //XDDDDDDDDD

    public GameObject room_prefab, oxygen_tank, jetpack_fuel, inflatable_wall, eva_suit;

    [HideInInspector]
    public Transform wall_holder;
    [HideInInspector]
    public Transform room_holder;
   
    // Start is called before the first frame update
    void Awake()
    {
        wall_holder = transform.FindDeepChild("Walls");
        room_holder = transform.FindDeepChild("Rooms");
        Initialize();
    }

    public void Initialize()
    {
        SpawnRooms();
        InitializeFloors();
        InitializeWalls();
        //InitializeItems();
    }

    void SpawnRooms()
    {
        float room_pos = 0;
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                GameObject room = Instantiate(room_prefab, new Vector2(room_pos+(i*Room.ROOM_SIZE), room_pos+(j*Room.ROOM_SIZE)), Quaternion.identity);
                room.transform.parent = room_holder;
            }
        }
    }

    void InitializeFloors()
    {
        int i = 0, size = BOARD_SIZE;

        foreach (Room room in GetComponentsInChildren<Room>())
        {
            int x = (int)i / size, y = i % size;
            bool edge = x == 0 || x == (size - 1) || y == 0 || y == (size - 1);
            bool spawn = false;
            if (x == 1)
            {
                if (y == 1 || y == (size - 2))
                    spawn = true;
            }
            else if (x == (size - 2))
            {
                if (y == 1 || y == (size - 2))
                    spawn = true;
            }
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
                float seed = (Random.Range(0, 100) % 100) / 100.0f;
                float noise = Mathf.PerlinNoise((float)x / size - seed, (float)y / size - seed);
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
        List<Room> shuffled_rooms = new List<Room>();
        foreach (Room room in rooms) 
            shuffled_rooms.Add(room);
        shuffled_rooms.Shuffle();

        PopulateMap(eva_suit, num_eva_suit, shuffled_rooms);
        PopulateMap(oxygen_tank, num_oxygen_tanks, shuffled_rooms);
        PopulateMap(jetpack_fuel, num_jetpack_fuel, shuffled_rooms);
        PopulateMap(inflatable_wall, num_inflatable_walls, shuffled_rooms);
    }

    void PopulateMap(GameObject item, int count, List<Room> shuffled_rooms)
    {
        int placed = 0;
        for (int i = shuffled_rooms.Count-1; i >= 0; i--)
        {
            Room room = shuffled_rooms[i];
            if (room.floorType != FloorType.Space && !room.spawn)
            {
                room.PopulateItem(item);
            }
            shuffled_rooms.RemoveAt(i);
            if (placed >= count)
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
