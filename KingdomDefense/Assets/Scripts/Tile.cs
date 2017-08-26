using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[RequireComponent( typeof( BoxCollider2D ) )]
public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public SpriteRenderer ActiveTile;
    public Transform UnitSpawn;

    public bool IsEmpty { get; set; }

    private BoxCollider2D Col2D;
    private FriendlyUnit UnitOnTile;

    private void Awake( )
    {
        ActiveTile.gameObject.SetActive( false );
        Col2D = GetComponent<BoxCollider2D>( );
        IsEmpty = true;
        GameManager.OnUnitDrop.AddListener( SetUnitOnTile );
        GameManager.OnFriendlyUnitDeath.AddListener( ResetTIle );
    }

    public Vector2 GetRandomPointInTile( )
    {
        Vector2 pos = Vector2.zero;
        pos.x = UnityEngine.Random.Range( Col2D.bounds.min.x, Col2D.bounds.max.x );
        pos.y = UnityEngine.Random.Range( Col2D.bounds.min.y, Col2D.bounds.max.y );
        return pos;
    }

    public void OnPointerEnter( PointerEventData eventData )
    {
        if ( !IsEmpty ) return;
        ActiveTile.gameObject.SetActive( true );
        ActiveTile.color = Color.green;
    }

    public void OnPointerExit( PointerEventData eventData )
    {
        if ( !IsEmpty ) return;
        ActiveTile.gameObject.SetActive( false );
    }

    public void OnDrop( PointerEventData eventData )
    {
        if ( !IsEmpty || !GameManager.Instance.HasEnoughGoldToPurchase( UnitOnTile.Cost ) )
            return;

        SpawnUnit( UnitOnTile );
    }

    private void SetUnitOnTile( FriendlyUnit unit )
    {
        if ( IsEmpty ) UnitOnTile = unit;
    }

    private void SpawnUnit( FriendlyUnit unit )
    {
        if ( unit == null ) return;

        UnitOnTile = Instantiate( unit, UnitSpawn.transform.position, Quaternion.identity ) as FriendlyUnit;
        UnitOnTile.GetComponent<BoxCollider2D>( ).enabled = true;
        UnitOnTile.Init( );

        IsEmpty = false;
        ActiveTile.gameObject.SetActive( false );

        GameManager.OnUnitPurchased.Invoke( unit );
    }

    private void ResetTIle( )
    {
        if ( UnitOnTile.IsDead )
            IsEmpty = true;
    }
}