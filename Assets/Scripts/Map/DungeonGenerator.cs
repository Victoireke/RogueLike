using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;
    private int currentFloor = 0;
    List<Room> rooms = new List<Room>();

    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void SetMaxItems(int max)
    {
        maxItems = max;
    }

    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }

    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            GameObject roomObject = new GameObject("Room");
            Room room = roomObject.AddComponent<Room>();
            room.X = roomX;
            room.Y = roomY;
            room.Width = roomWidth;
            room.Height = roomHeight;

            if (room.Overlaps(rooms))
            {
                Destroy(roomObject);
                continue;
            }

            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }
                }
            }

            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }

            PlaceEnemies(room, maxEnemies);
            PlaceItems(room, maxItems);

            rooms.Add(room);
        }

        GameObject playerObject;
        if (GameManager.Get.Player != null)
        {
            playerObject = GameManager.Get.Player.gameObject;
            playerObject.transform.position = new Vector3(rooms[0].Center().x, rooms[0].Center().y, 0);

        }
        else
        {
            playerObject = MapManager.Get.CreateActor("Player", rooms[0].Center());
        }

        if (currentFloor > 0)
        {
            GameObject ladderUpObject = MapManager.Get.CreateActor("LadderUp", rooms[rooms.Count - 1].Center());
            if (ladderUpObject != null)
            {
                Ladder ladderUp = ladderUpObject.GetComponent<Ladder>();
                if (ladderUp != null)
                {
                    ladderUp.Up = true;
                }
            }
        }

        if (currentFloor > 0)
        {
            GameObject ladderDownObject = MapManager.Get.CreateActor("LadderDown", rooms[0].Center());
            if (ladderDownObject != null)
            {
                Ladder ladderDown = ladderDownObject.GetComponent<Ladder>();
                if (ladderDown != null)
                {
                    ladderDown.Up = false;
                }
            }
        }

        GameManager.Get.Player = playerObject.GetComponent<Actor>();
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }
        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        // Get centers of old and new rooms
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();

        // Determine the position of the corner for the tunnel
        Vector2Int tunnelCorner;
        if (Random.value < 0.5f)
        {
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        // Create a list to store tunnel coordinates
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();

        // Compute the tunnel coordinates between old and new room centers
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // Loop through the tunnel coordinates
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            // Set floor tiles for each coordinate
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0));

            // Set wall tiles around the floor tiles to create the tunnel
            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }




    private void PlaceEnemies(Room room, int maxEnemies)
    {
        int num = Random.Range(0, maxEnemies + 1);

        for (int counter = 0; counter < num; counter++)
        {
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            if (Random.value < 0.5f)
            {
                GameManager.Get.CreateActor("Enemy", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateActor("Enemy1", new Vector2(x, y));
            }
        }
    }

    private void PlaceItems(Room room, int maxItems)
    {
        int numItems = Random.Range(0, maxItems + 1);

        for (int i = 0; i < numItems; i++)
        {
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            float randomValue = Random.value;
            if (randomValue < 0.33f)
            {
                GameManager.Get.CreateActor("HealthPotion", new Vector2(x, y));
            }
            else if (randomValue < 0.66f)
            {
                GameManager.Get.CreateActor("Fireball", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateActor("ScrollOfConfusion", new Vector2(x, y));
            }
        }
    }
}
