using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class DragUnitButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    public FriendlyUnit UnitPrefab;
    public Text PriceText;

    private FriendlyUnit UnitObject;
    private Vector2 StartingPosition = Vector2.zero;

    private void Awake( )
    {
        GameManager.OnGoldUpdated.AddListener( UpdateButtonState );
    }

    private void Start( )
    {
        PriceText.text = UnitPrefab.Cost.ToString( );

        UnitObject = Instantiate( UnitPrefab, Camera.main.ScreenToWorldPoint( transform.position ), Quaternion.identity ) as FriendlyUnit;
        UnitObject.transform.position = new Vector3( UnitObject.transform.position.x, UnitObject.transform.position.y, 0f );

        StartingPosition = Camera.main.ScreenToWorldPoint( transform.position );

        UnitObject.GetComponent<BoxCollider2D>( ).enabled = false;
        UnitObject.gameObject.SetActive( false );
    }

    public void OnBeginDrag( PointerEventData eventData )
    {
        if ( GameManager.Instance.HasEnoughGoldToPurchase( UnitPrefab.Cost ) )
        {
            GetComponent<Button>( ).interactable = true;
            UnitObject.gameObject.SetActive( true );
        }
    }

    public void OnDrag( PointerEventData eventData )
    {
        Vector2 dragPos = Camera.main.ScreenToWorldPoint( eventData.position );
        dragPos.y -= UnitObject.GetComponent<BoxCollider2D>( ).size.y / 3f;

        UnitObject.transform.position = dragPos;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        GameManager.OnUnitDrop.Invoke( UnitPrefab );
        EventSystem.current.SetSelectedGameObject( null );
        UnitObject.transform.position = StartingPosition;
        UnitObject.gameObject.SetActive( false );
    }

    private void UpdateButtonState( )
    {
        bool hasEnoughMoney = GameManager.Instance.HasEnoughGoldToPurchase( UnitPrefab.Cost );
        GetComponent<Button>( ).interactable = hasEnoughMoney;
    }
}