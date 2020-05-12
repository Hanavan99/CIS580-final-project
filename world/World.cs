using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS580_final_project.world
{
    public class World
    {

        public static readonly int TileSize = 64;

        private WorldTile[,] tileMap;
        private Rail[,] railMap;
        private List<Vehicle> vehicles = new List<Vehicle>();

        public World(int width, int height)
        {
            tileMap = new WorldTile[width, height];
            railMap = new Rail[width, height];
            StartingRails = new Dictionary<Rail, Payload>();
        }

        public int Width { get => tileMap.GetLength(0); }

        public int Height { get => tileMap.GetLength(1); }

        public Dictionary<Rail, Payload> StartingRails { get; }

        public WorldTile GetTile(int x, int y)
        {
            return tileMap[x, y];
        }

        public void SetTile(int x, int y, WorldTile tile)
        {
            tileMap[x, y] = tile;
        }

        public Rail GetRail(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return railMap[x, y];
            }
            return null;
        }

        public void SetRail(int x, int y, Rail rail)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                railMap[x, y] = rail;
            }
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
        }

        public void Update(GameState gameState)
        {
            // update vehicles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Rail r = GetRail(x, y);
                    if (r != null)
                    {
                        foreach (Vehicle v in r.Vehicles.ToArray())
                        {
                            r.UpdateVehicle(gameState, v);

                            if (v.Position > r.GetRailLength())
                            {
                                
                                Direction newDir = r.GetEndDirection(v.EnterDirection);

                                Rail newr = null;
                                switch (newDir)
                                {
                                    case Direction.North:
                                        newr = GetRail(x, y - 1);
                                        break;
                                    case Direction.East:
                                        newr = GetRail(x + 1, y);
                                        break;
                                    case Direction.South:
                                        newr = GetRail(x, y + 1);
                                        break;
                                    case Direction.West:
                                        newr = GetRail(x - 1, y);
                                        break;
                                }

                                if (newr != null && newr.CanEnterFrom(newDir))
                                {
                                    v.Position -= r.GetRailLength();
                                    r.Vehicles.Remove(v);
                                    v.EnterDirection = newDir;
                                    newr.Vehicles.Add(v);
                                } else
                                {
                                    //v.EnterDirection = newDir.Reverse();
                                    //v.Position = 0;
                                    v.Velocity = 0;
                                    v.Position = r.GetRailLength();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Draw(GameState gameState, bool isDeleting, Vector2 mouseTilePos)
        {
            // draw tiles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (tileMap[x, y] != null)
                    {
                        tileMap[x, y].Draw(gameState, x * TileSize, y * TileSize);
                    }
                }
            }

            // draw rails
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (railMap[x, y] != null)
                    {
                        railMap[x, y].Draw(gameState, x * TileSize, y * TileSize, isDeleting && mouseTilePos.X == x && mouseTilePos.Y == y ? Color.Red : Color.White);
                    }
                }
            }
        }

        public bool HasWon()
        {
            int bufferRailCount = 0;
            int correctVehicleCount = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Rail r = GetRail(x, y);
                    if (r != null && r.GetType() == typeof(BufferRail))
                    {
                        BufferRail br = (BufferRail) r;
                        if (br.TargetPayload != null)
                            bufferRailCount++;
                        foreach (Vehicle v in br.Vehicles)
                        {
                            if (v.GetType() == typeof(Wagon) && ((Wagon) v).Payload == br.TargetPayload)
                            {
                                correctVehicleCount++;
                            }
                        }
                    }
                }
            }
            return bufferRailCount == correctVehicleCount;
        }
    }
}
