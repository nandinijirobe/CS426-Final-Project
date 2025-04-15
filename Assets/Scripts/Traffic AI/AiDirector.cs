using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using UnityEditor;

public class AiDirector : MonoBehaviour
{
    // need a graph of grid

    public GameObject gridParent;
    public GameObject carPrefab;

    public LayerMask carMask;

    public List<GameObject> vehicles;

    private Dictionary<Point, Transform> pointToTransformMap = new Dictionary<Point, Transform>();
    private Dictionary<Transform, Point> transformToPointMap = new Dictionary<Transform, Point>();
    private Grid grid;

    private int numCars = 0;

    AdjacencyGraph carGraph = new AdjacencyGraph();
    // List<Transform> carPath = new List<Transform>();
    List<Vector3> carPath = new List<Vector3>();


    void Start() {
        BuildGrid2();
        SpawnCars();
    }

    void Update() {
        // DrawGraph(carGraph);

        if (numCars < 100) {
            SpawnCars();
        }
    }

    void BuildGrid2() {
        if (gridParent == null)
        {
            Debug.LogError("Grid parent not assigned!");
            return;
        }

        int height = 29;
        int maxWidth = 29;

        grid = new Grid(maxWidth, height);

        int rowIndex = 0;
        foreach (Transform row in gridParent.transform)
        {
            int colIndex = 0;
            foreach (Transform Cube in row)
            {
                Point point = new Point(colIndex, rowIndex);

                if (Cube.transform.childCount == 0) { // not a road!
                    grid[point.X, point.Y] = CellType.Structure;
                    // Debug.Log("Setting object type to Structure...");
                    
                } else {
                    grid[point.X, point.Y] = CellType.Road;
                    // Transform road = Cube.transform.GetChild(0);
                    // pointToTransformMap[point] = road;
                }

                pointToTransformMap[point] = Cube;
                transformToPointMap[Cube] = point;

                colIndex++;
            }
            rowIndex++;
        }

        Debug.Log($"Grid initialized: {maxWidth} x {height} with {pointToTransformMap.Count} road points.");
    }
    
    public void SpawnCars()
    {
        if (grid == null || carPrefab == null) return;

        Point spawnPoint = grid.GetRandomRoadPoint();
        Point endPoint = grid.GetRandomRoadPoint();

        // make sure they're not the same
        while (endPoint == spawnPoint) {
            endPoint = grid.GetRandomRoadPoint();
        }

        if (pointToTransformMap.TryGetValue(spawnPoint, out Transform transform))
        {
            var path = GetPathBetween(spawnPoint, endPoint);
            path.Reverse();

            if (transform.childCount == 0) {
                Debug.Log("Something went wrong...");
            }

            var road = transform.GetChild(0).gameObject;
            var road_end = pointToTransformMap[endPoint].GetChild(0).gameObject;


            var startMarkerPosition = road.GetComponent<RoadHelper>().GetPositioForCarToSpawn(path[1].position);
            var endMarkerPosition = road_end.GetComponent<RoadHelper>().GetPositioForCarToEnd(path[path.Count-2].position);

            if (path == null) {
                // Debug.Log("Something wrong with path...");
                return;
            }
            if (startMarkerPosition == null) {
                // Debug.Log("Something wrong with start marker position...");
                return;
            }
            if (endMarkerPosition == null) {
                // Debug.Log("Something wrong with end marker position");
                return;
            }

            if (!CheckForSpawnCollisions(startMarkerPosition.Position)) {
                return;
            };

            // Debug.Log("Start position: " + startMarkerPosition.Position);
            // Debug.Log("End position: " + endMarkerPosition.Position);

            carPath = GetCarPath(path, startMarkerPosition.Position, endMarkerPosition.Position);
            // Debug.Log("Car path: " + carPath[0]);
           

            if (carPath.Count > 1) {
                System.Random rand = new System.Random();
                GameObject prefab = vehicles[rand.Next(0, vehicles.Count - 1)];


                // Debug.Log($"Spawned car at grid point {spawnPoint} -> world position {transform.position}");

                // var car = Instantiate(carPrefab, startMarkerPosition.Position, Quaternion.identity);
                var car = Instantiate(prefab, startMarkerPosition.Position, Quaternion.identity);
                car.GetComponent<CarAI>().SetPath(carPath);
                car.GetComponent<CarAI>().director = this;
                numCars++;
            }
        }
        else
        {
            Debug.LogWarning("Could not find Transform for selected road point.");
        }
    }


    private bool CheckForSpawnCollisions(Vector3 spawnPoint) {
        Vector3 overlapTestBox = new Vector3(10f, 10f, 10f);
        Collider[] collidersTouchingBox = new Collider[1];
        int numCollisions = Physics.OverlapBoxNonAlloc(spawnPoint, overlapTestBox, collidersTouchingBox, Quaternion.Euler(0,0,0), carMask);

        if (numCollisions == 0) {
            // Debug.Log("Found a collision at spawn point");   
            return true;
        } else {
            return false;
        }
    }
    internal List<Transform> GetPathBetween(Point startPosition, Point endPosition)
    {
        List<Point> resultPath = GridSearch.AStarSearch(grid, startPosition, endPosition);
        List<Transform> path = new List<Transform>();
        foreach (Point point in resultPath)
        {
            // path.Add(new Vector3Int(point.X, 0, point.Y));
            
            // if (pointToTransformMap.TryGetValue(point, out Transform t))
            // {
            //     path.Add(t.position);
            //     Debug.Log("Adding position: " + t.position + " to path.");
            // }
            // else
            // {
            //     Debug.LogWarning($"Transform not found for point {point}");
            // }

            path.Add(pointToTransformMap[point]);
        }
        return path;
    }


    private List<Vector3> GetCarPath(List<Transform> path, Vector3 startPosition, Vector3 endPosition)
    {
        carGraph.ClearGraph();
        CreatACarGraph(path);
        // Debug.Log(carGraph);
        return AdjacencyGraph.AStarSearch(carGraph, startPosition, endPosition);
    }

    private void CreatACarGraph(List<Transform> path)
    {
        Dictionary<Marker, Vector3> tempDictionary = new Dictionary<Marker, Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            var currentPosition = path[i];
            // var roadStructure = placementManager.GetStructureAt(currentPosition);
            var roadStructure = currentPosition.GetChild(0).gameObject;
            var markersList = roadStructure.GetComponent<RoadHelper>().GetAllCarMarkers();
            var limitDistance = markersList.Count > 3;
            tempDictionary.Clear();

            foreach (var marker in markersList)
            {
                carGraph.AddVertex(marker.Position);
                foreach (var markerNeighbour in marker.adjacentMarkers)
                {
                    carGraph.AddEdge(marker.Position, markerNeighbour.Position);
                }
                if(marker.OpenForconnections && i + 1 < path.Count)
                {
                    // var nextRoadPosition = placementManager.GetStructureAt(path[i + 1]);
                    var nextRoadPosition = path[i+1].GetChild(0).gameObject;
                    if (limitDistance)
                    {
                        tempDictionary.Add(marker, nextRoadPosition.GetComponent<RoadHelper>().GetClosestCarMarkerPosition(marker.Position));
                    }
                    else
                    {
                        carGraph.AddEdge(marker.Position, nextRoadPosition.GetComponent<RoadHelper>().GetClosestCarMarkerPosition(marker.Position));
                    }
                }
            }
            if (limitDistance && tempDictionary.Count > 2)
            {
                var distanceSortedMarkers = tempDictionary.OrderBy(x => Vector3.Distance(x.Key.Position, x.Value)).ToList();
                foreach (var item in distanceSortedMarkers)
                {
                    // Debug.Log(Vector3.Distance(item.Key.Position, item.Value));
                }
                for (int j = 0; j < 2; j++)
                {
                    carGraph.AddEdge(distanceSortedMarkers[j].Key.Position, distanceSortedMarkers[j].Value);
                }
            }
        }
    }


    private void DrawGraph(AdjacencyGraph graph)
    {
        foreach (var vertex in graph.GetVertices())
        {
            foreach (var vertexNeighbour in graph.GetConnectedVerticesTo(vertex))
            {
                Debug.DrawLine(vertex.Position + Vector3.up, vertexNeighbour.Position + Vector3.up, Color.red);
            }
        }
    }

    public void updateNumCars() {
        numCars--;
    }
}
