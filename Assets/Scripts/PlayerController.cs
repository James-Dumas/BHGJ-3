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
    public Buildable[] BuildableHotbar;

    private Rigidbody2D rigidbody;
    private int health;
    private int hotbarSelection;
    private bool canPlace;

    public int Money { get; set; }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        health = MaxHealth;
        hotbarSelection = -1;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float lookAngle = Vector2.SignedAngle(Vector2.up, (Vector2) (mousePos - transform.position));
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        for(int i = 0; i < BuildableHotbar.Length; i++)
        {
            if(Input.GetButtonDown($"Item {i + 1}"))
            {
                hotbarSelection = i;
            }
        }

        Vector3Int hoveredTilePos = FunctionalTilemap.WorldToCell(mousePos);
        Vector3 hoveredTileCenter = FunctionalTilemap.CellToWorld(hoveredTilePos) + new Vector3(0.5f, 0.5f, 0f);
        canPlace = false;
        if(hotbarSelection >= 0)
        {
            Buildable selectedBuildable = BuildableHotbar[hotbarSelection];
            if(Money >= selectedBuildable.Price && (hoveredTileCenter - transform.position).magnitude < PlacementDistance && FloorTilemap.HasTile(hoveredTilePos) && !(FullBlockingTilemap.HasTile(hoveredTilePos) || WalkBlockingTilemap.HasTile(hoveredTilePos) || FunctionalTilemap.HasTile(hoveredTilePos)))
            {
                canPlace = true;
            }
            else
            {

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
                ProximityMine mine = buildableObject.GetComponent<ProximityMine>();
                if(mine != null)
                {
                    mine.Tilemap = FunctionalTilemap;
                }
            }
        }
        
        if(Input.GetButtonDown("Right Click"))
        {
            hotbarSelection = -1;
        }
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1) * (Input.GetButton("Slow Move") ? SlowSpeed : Speed) * Time.fixedDeltaTime;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if(health == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
