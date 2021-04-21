using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BaseEnemy : MonoBehaviour
{
    public float Speed = 100f;
    public float Acceleration = 10f;
    public float ShootTimer = 1f;
    public float Spread = 0f;
    public int MaxHealth = 20;
    public int KillValue = 5;
    public GameObject Projectile;
    public LayerMask SightMask;
    public Tilemap FullWallTilemap;
    public Tilemap HalfWallTilemap;

    protected Rigidbody2D rigidbody;
    protected GameObject player;
    protected bool hasLineOfSight;
    protected float nextShootTime;
    protected bool canShoot;
    protected Vector2 movement;
    protected int health;
    protected Vector2 towardsPlayer;
    protected PathfindingNodeQueue openNodes;
    protected HashSet<Vector2Int> closedNodes;
    protected LinkedList<Vector2> path;
    protected float lastPathCheck;

    public float BeatTime { get; set; }

    protected static Vector2Int[] adjacents;
    protected static Vector2Int[] diagonals;


    static BaseEnemy()
    {
        adjacents = new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        diagonals = new Vector2Int[] {
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1)
        };
    }

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        movement = Vector2.zero;
        nextShootTime = BeatTime;
        lastPathCheck = 0f;
        health = MaxHealth;
        towardsPlayer = Vector2.zero;
        openNodes = new PathfindingNodeQueue();
        closedNodes = new HashSet<Vector2Int>();
        path = new LinkedList<Vector2>();
    }

    protected virtual void Update()
    {
        if(player != null)
        {
            towardsPlayer = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, towardsPlayer, 1000f, SightMask);
            hasLineOfSight = hit.collider.gameObject == player;
        }
        else
        {
            hasLineOfSight = false;
        }

        if(Time.time - lastPathCheck > 1f)
        {
            FindPath();
            lastPathCheck = Time.time;
        }

        if(path.Count > 0)
        {
            Vector2 towardsNextPos = path.First.Value - (Vector2) transform.position;
            if(towardsNextPos.magnitude < 0.71f)
            {
                path.RemoveFirst();
            }
            movement = towardsNextPos.normalized;
        }

        canShoot = false;
        if(Time.time > nextShootTime)
        {
            nextShootTime += ShootTimer;
            canShoot = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        rigidbody.velocity += (movement - rigidbody.velocity) * Acceleration * Time.fixedDeltaTime;
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, Speed);
    }

    protected void Shoot(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Instantiate(Projectile, transform.position + new Vector3(direction.x, direction.y, 0f) * 0.5f, Quaternion.Euler(0f, 0f, angle));
    }

    protected void FindPath()
    {
        path.Clear();
        openNodes.Clear();
        closedNodes.Clear();

        Vector2Int goal = (Vector2Int) FullWallTilemap.WorldToCell(player.transform.position);
        Vector2Int nodePos = (Vector2Int) FullWallTilemap.WorldToCell(transform.position);
        openNodes.Add(new PathFindingNode(nodePos, null, 0f, (goal - nodePos).magnitude));
        PathFindingNode? node = null;
        bool success = false;
        while(openNodes.Count > 0)
        {
            node = openNodes.PopBestNode();
            if(node.Value == goal)
            {
                success = true;
                break;
            }
            closedNodes.Add(node.Value);
            float goalDistance = (goal - node.Value).magnitude;
            foreach(Vector2Int direction in adjacents)
            {
                Vector2Int newNodePos = node.Value + direction;
                Vector3Int newNodePos3 = new Vector3Int(newNodePos.x, newNodePos.y, 0);
                PathFindingNode newNode = new PathFindingNode(newNodePos, node, node.Distance + 1, goalDistance);
                if(!closedNodes.Contains(newNodePos))
                {
                    if(FullWallTilemap.HasTile(newNodePos3) || HalfWallTilemap.HasTile(newNodePos3))
                    {
                        closedNodes.Add(newNodePos);
                    }
                    else
                    {
                        openNodes.Add(newNode);
                    }
                }
            }

            foreach(Vector2Int direction in diagonals)
            {
                Vector2Int newNodePos = node.Value + direction;
                Vector3Int newNodePos3 = new Vector3Int(newNodePos.x, newNodePos.y, 0);
                PathFindingNode newNode = new PathFindingNode(newNodePos, node, node.Distance + 1.41421356f, goalDistance);
                Vector3Int[] cornerAdjacents = new Vector3Int[] {
                    new Vector3Int(newNodePos.x, node.Value.y, 0),
                    new Vector3Int(node.Value.x, newNodePos.y, 0)
                };
                if(!closedNodes.Contains(newNodePos))
                {
                    if(FullWallTilemap.HasTile(newNodePos3) || HalfWallTilemap.HasTile(newNodePos3)
                    || FullWallTilemap.HasTile(cornerAdjacents[0]) || HalfWallTilemap.HasTile(cornerAdjacents[0])
                    || FullWallTilemap.HasTile(cornerAdjacents[1]) || HalfWallTilemap.HasTile(cornerAdjacents[1]))
                    {
                        closedNodes.Add(newNodePos);
                    }
                    else
                    {
                        openNodes.Add(newNode);
                    }
                }
            }
        }

        if(success)
        {
            while(node != null)
            {
                path.AddFirst((Vector2) FullWallTilemap.CellToWorld(new Vector3Int(node.Value.x, node.Value.y, 0)) + new Vector2(0.5f, 0.5f));
                node = node.Parent;
            }
        }
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health == 0)
        {
            if(player != null)
            {
                player.GetComponent<PlayerController>().Money += KillValue;
            }

            Destroy(this.gameObject);
        }
    }
}
