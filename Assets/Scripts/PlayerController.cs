using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float Speed = 280f;
    public float SlowSpeed = 100f;
    public int MaxHealth = 3;
    public float PlacementDistance = 5f;
    public Tilemap FloorTilemap;
    public Tilemap FullBlockingTilemap;
    public Tilemap WalkBlockingTilemap;
    public Tilemap FunctionalTilemap;
    public Tilemap ShieldTilemap;
    public Tilemap GhostTilemap;
    public Buildable[] BuildableHotbar;

    private Rigidbody2D rigidbody;
    private int health;
    private int hotbarSelection;
    private bool canPlace;
    private float nextSecondTime;

    public bool LevelStarted { get; set; }
    public bool Alive { get; private set; }
    public int Money { get; set; }
    public float BeatTime { get; set; }

    void Start()
    {
        LevelStarted = false;
        rigidbody = GetComponent<Rigidbody2D>();
        health = MaxHealth;
        Alive = true;
        hotbarSelection = -1;
    }

    void Update()
    {
        if(LevelStarted)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int hoveredTilePos = FunctionalTilemap.WorldToCell(mousePos);
            Vector3 hoveredTileCenter = FunctionalTilemap.CellToWorld(hoveredTilePos) + new Vector3(0.5f, 0.5f, 0f);
            GhostTilemap.ClearAllTiles();
            canPlace = false;
            if(hotbarSelection >= 0)
            {
                Buildable selectedBuildable = BuildableHotbar[hotbarSelection];
                Color c;
                if(Money >= selectedBuildable.Price && (hoveredTileCenter - transform.position).magnitude < PlacementDistance && FloorTilemap.HasTile(hoveredTilePos) && !(FullBlockingTilemap.HasTile(hoveredTilePos) || WalkBlockingTilemap.HasTile(hoveredTilePos) || FunctionalTilemap.HasTile(hoveredTilePos)))
                {
                    canPlace = true;
                    c = Color.white;

                }
                else
                {
                    c = Color.red;
                    c.g = 0.5f;
                    c.b = 0.5f;
                }

                c.a = 0.5f;
                GhostTilemap.color = c;
                GhostTilemap.SetTile(hoveredTilePos, selectedBuildable.Tile);
            }

            if(Time.time > nextSecondTime)
            {
                nextSecondTime += 1f;
            }

            if(Alive)
            {
                float lookAngle = Vector2.SignedAngle(Vector2.up, (Vector2) (mousePos - transform.position));
                transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

                for(int i = 0; i < BuildableHotbar.Length; i++)
                {
                    if(Input.GetButtonDown($"Item {i + 1}"))
                    {
                        hotbarSelection = i;
                    }
                }

                if(Input.GetButtonDown("Left Click"))
                {
                    if(hotbarSelection >= 0 && canPlace)
                    {
                        Buildable selectedBuildable = BuildableHotbar[hotbarSelection];
                        FunctionalTilemap.SetTile(hoveredTilePos, selectedBuildable.Tile);
                        GameObject buildableObject = Instantiate(selectedBuildable.Object, hoveredTileCenter, Quaternion.identity);
                        Money -= selectedBuildable.Price;
                        Turret turret = buildableObject.GetComponent<Turret>();
                        if(turret != null)
                        {
                            turret.BeatTime = nextSecondTime;
                        }
                        ProximityMine mine = buildableObject.GetComponent<ProximityMine>();
                        if(mine != null)
                        {
                            mine.Tilemap = FunctionalTilemap;
                            mine.BeatTime = nextSecondTime;
                        }
                        ShieldEmitter shield = buildableObject.GetComponent<ShieldEmitter>();
                        if(shield != null)
                        {
                            shield.Tilemap = ShieldTilemap;
                        }
                    }
                }
                
                if(Input.GetButtonDown("Right Click"))
                {
                    hotbarSelection = -1;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(LevelStarted)
        {
            if(Alive)
            {
                rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1) * (Input.GetButton("Slow Move") ? SlowSpeed : Speed) * Time.fixedDeltaTime;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Alive = false;
            GetComponent<Collider2D>().enabled = false;
            hotbarSelection = -1;
        }
    }
}
