using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

[RequireComponent( typeof( CircleCollider2D ) )]
public class Coin : MonoBehaviour, IPointerClickHandler
{
    [Header( "Value" )]
    public int Value = 50;

    [Header( "Move Speed" )]
    [Range( 5f, 30f )]
    public float MoveSpeed = 15f;

    public Vector2 FallingDestination { get; set; }
    public Vector2 CollectedDestination { get; set; }

    private CircleCollider2D Col2D;
    private bool WasClicked = false;

    private void Awake( )
    {
        Col2D = GetComponent<CircleCollider2D>( );
    }

    private void Start( )
    {
        FallingDestination = GameManager.Instance.Board.GetRandomTile( ).GetRandomPointInTile( );
        CollectedDestination = GameManager.Instance.Board.Destination.position;

        FallingDestination = new Vector2( transform.position.x, FallingDestination.y );
        StartCoroutine( MoveToPosition( FallingDestination ) );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        WasClicked = true;
        Col2D.enabled = false;
        StartCoroutine( MoveToPosition( CollectedDestination ) );
    }

    private IEnumerator MoveToPosition( Vector3 pos )
    {
        while ( transform.position != pos )
        {
            transform.position = Vector3.MoveTowards( transform.position, pos, MoveSpeed * Time.deltaTime );
            yield return null;
        }

        if ( WasClicked )
        {
            GameManager.Instance.AddGold( Value );
            Destroy( gameObject ); 
        }
    }
}