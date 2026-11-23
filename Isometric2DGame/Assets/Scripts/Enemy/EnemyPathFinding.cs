
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyPathfinding : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap[] blockedTilemaps;

    [Header("Pathfinding")]
    [SerializeField] private float repathInterval = 0.25f;
    [SerializeField] private float waypointReachDistance = 0.12f;
    [SerializeField] private int snapRadius = 3;

    private EnemyMovement movement;

    private readonly List<Vector2> path = new List<Vector2>();

    private int pathIndex;

    private Vector2 destination;
    private float movementSpeed;
    private float repathTimer;

    private bool following;
    private bool needsRepath;
    private bool hasDestinationCell;

    private Vector3Int lastDestinationCell;

    private static readonly Vector3Int[] Neighbours =
    {
        new Vector3Int( 1,  0, 0),
        new Vector3Int(-1,  0, 0),
        new Vector3Int( 0,  1, 0),
        new Vector3Int( 0, -1, 0),

        new Vector3Int( 1,  1, 0),
        new Vector3Int( 1, -1, 0),
        new Vector3Int(-1,  1, 0),
        new Vector3Int(-1, -1, 0)
    };

    private class Node
    {
        public Vector3Int position;
        public Node parent;

        public int gCost = int.MaxValue;
        public int hCost;

        public int FCost => gCost + hCost;

        public Node(Vector3Int position)
        {
            this.position = position;
        }
    }


    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();

        // Find the Walkable Tilemap
        GameObject walkableObject = GameObject.FindGameObjectWithTag("Walkable");

        if (walkableObject != null)
        {
            walkableTilemap = walkableObject.GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("No GameObject with the 'Walkable' tag found!");
        }

        // Find all Obstacles Tilemaps
        GameObject[] obstacleObjects = GameObject.FindGameObjectsWithTag("Obstacles");

        blockedTilemaps = new Tilemap[obstacleObjects.Length];

        for (int i = 0; i < obstacleObjects.Length; i++)
        {
            blockedTilemaps[i] = obstacleObjects[i].GetComponent<Tilemap>();
        }
    }


    private void Update()
    {
        if (!following)
            return;

        // Recalculate when necessary
        repathTimer -= Time.deltaTime;

        if (needsRepath || repathTimer <= 0f)
        {
            RecalculatePath();

            needsRepath = false;
            repathTimer = repathInterval;
        }

        FollowPath();
    }

    public void SetDestination(Vector2 target, float speed)
    {
        destination = target;
        movementSpeed = speed;

        following = true;

        Vector3Int targetCell =
            walkableTilemap.WorldToCell(target);

        // Only request a new path when the target enters
        // a different Tilemap cell.
        if (!hasDestinationCell || targetCell != lastDestinationCell)
        {
            lastDestinationCell = targetCell;
            hasDestinationCell = true;
            needsRepath = true;
        }
    }

    public void StopFollowing()
    {
        following = false;
        needsRepath = false;

        path.Clear();
        pathIndex = 0;

        movement.Stop();
    }

    private void RecalculatePath()
    {
        path.Clear();
        pathIndex = 0;

        Vector3Int startCell =
            walkableTilemap.WorldToCell(transform.position);

        Vector3Int targetCell =
            walkableTilemap.WorldToCell(destination);

        if (!TryFindNearestWalkable(startCell, out startCell) ||
            !TryFindNearestWalkable(targetCell, out targetCell))
        {
            movement.Stop();
            return;
        }

        // Already in the destination cell
        if (startCell == targetCell)
        {
            path.Add(destination);
            return;
        }

        FindPath(startCell, targetCell);
    }

    private bool FindPath(Vector3Int start, Vector3Int target)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();

        Dictionary<Vector3Int, Node> nodes =
            new Dictionary<Vector3Int, Node>();

        Node startNode = new Node(start);

        startNode.gCost = 0;
        startNode.hCost = GetDistance(start, target);

        openList.Add(startNode);
        nodes.Add(start, startNode);

        while (openList.Count > 0)
        {
            Node current = GetLowestCostNode(openList);

            openList.Remove(current);
            closedSet.Add(current.position);

            if (current.position == target)
            {
                BuildPath(current);
                return true;
            }

            foreach (Vector3Int offset in Neighbours)
            {
                Vector3Int neighbourPosition =
                    current.position + offset;

                if (!IsWalkable(neighbourPosition) ||
                    closedSet.Contains(neighbourPosition))
                    continue;

                // Prevent cutting diagonally through corners.
                if (offset.x != 0 && offset.y != 0)
                {
                    Vector3Int horizontal =
                        current.position + new Vector3Int(offset.x, 0, 0);

                    Vector3Int vertical =
                        current.position + new Vector3Int(0, offset.y, 0);

                    if (!IsWalkable(horizontal) ||
                        !IsWalkable(vertical))
                        continue;
                }

                int moveCost = offset.x != 0 && offset.y != 0
                    ? 14
                    : 10;

                int newCost = current.gCost + moveCost;

                if (!nodes.TryGetValue(neighbourPosition, out Node neighbour))
                {
                    neighbour = new Node(neighbourPosition);
                    nodes.Add(neighbourPosition, neighbour);
                }

                if (newCost >= neighbour.gCost)
                    continue;

                neighbour.gCost = newCost;
                neighbour.hCost = GetDistance(neighbourPosition, target);
                neighbour.parent = current;

                if (!openList.Contains(neighbour))
                    openList.Add(neighbour);
            }
        }

        // No possible route
        return false;
    }

    private void BuildPath(Node targetNode)
    {
        List<Vector2> reversedPath = new List<Vector2>();

        Node current = targetNode;

        // Exclude the starting cell
        while (current.parent != null)
        {
            Vector3 worldPosition =
                walkableTilemap.GetCellCenterWorld(current.position);

            reversedPath.Add(worldPosition);

            current = current.parent;
        }

        reversedPath.Reverse();

        path.AddRange(reversedPath);

        // Use the exact destination for the final waypoint.
        if (path.Count > 0)
            path[path.Count - 1] = destination;
    }

    private void FollowPath()
    {
        if (pathIndex >= path.Count)
        {
            movement.Stop();
            return;
        }

        // Update the final waypoint as the target moves.
        Vector2 waypoint = pathIndex == path.Count - 1
            ? destination
            : path[pathIndex];

        float distance = Vector2.Distance(
            transform.position,
            waypoint
        );

        if (distance <= waypointReachDistance)
        {
            pathIndex++;

            if (pathIndex >= path.Count)
            {
                movement.Stop();
                return;
            }

            waypoint = pathIndex == path.Count - 1
                ? destination
                : path[pathIndex];
        }

        movement.MoveTowards(waypoint, movementSpeed);
    }

    private bool IsWalkable(Vector3Int cell)
    {
        if (!walkableTilemap.cellBounds.Contains(cell))
            return false;

        // A floor tile must exist.
        if (!walkableTilemap.HasTile(cell))
            return false;

        // Blocked Tilemaps override the floor.
        if (blockedTilemaps != null)
        {
            foreach (Tilemap blocked in blockedTilemaps)
            {
                if (blocked != null && blocked.HasTile(cell))
                    return false;
            }
        }

        return true;
    }

    private bool TryFindNearestWalkable(
        Vector3Int origin,
        out Vector3Int result)
    {
        if (IsWalkable(origin))
        {
            result = origin;
            return true;
        }

        for (int radius = 1; radius <= snapRadius; radius++)
        {
            float bestDistance = float.MaxValue;
            bool found = false;
            result = origin;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) != radius &&
                        Mathf.Abs(y) != radius)
                        continue;

                    Vector3Int candidate =
                        origin + new Vector3Int(x, y, 0);

                    if (!IsWalkable(candidate))
                        continue;

                    float distance = x * x + y * y;

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        result = candidate;
                        found = true;
                    }
                }
            }

            if (found)
                return true;
        }

        result = origin;
        return false;
    }

    private Node GetLowestCostNode(List<Node> nodes)
    {
        Node lowest = nodes[0];

        foreach (Node node in nodes)
        {
            if (node.FCost < lowest.FCost ||
                (node.FCost == lowest.FCost &&
                 node.hCost < lowest.hCost))
            {
                lowest = node;
            }
        }

        return lowest;
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        int x = Mathf.Abs(a.x - b.x);
        int y = Mathf.Abs(a.y - b.y);

        int remaining = Mathf.Abs(x - y);

        return 14 * Mathf.Min(x, y) + 10 * remaining;
    }

    public bool TryGetRandomWalkablePoint(
        Vector2 origin,
        float minDistance,
        float maxDistance,
        out Vector2 result)
    {
        result = origin;

        if (walkableTilemap == null)
            return false;

        BoundsInt bounds = walkableTilemap.cellBounds;

        // Try up to 100 random cells
        for (int i = 0; i < 100; i++)
        {
            Vector3Int cell = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                0
            );

            if (!IsWalkable(cell))
                continue;

            Vector3 worldPosition =
                walkableTilemap.GetCellCenterWorld(cell);

            float distance = Vector2.Distance(
                origin,
                worldPosition
            );

            if (distance < minDistance || distance > maxDistance)
                continue;

            result = worldPosition;
            return true;
        }

        return false;
    }

}
