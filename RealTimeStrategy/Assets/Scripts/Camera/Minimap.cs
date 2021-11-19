using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Minimap : MonoBehaviour,IPointerDownHandler,IDragHandler
{
    [SerializeField] private RectTransform minimapRect = null;
    [SerializeField] private float mapScale = 10f;
    [SerializeField] private float offset = -6f;

    private Transform playerCameraTransform;

    private void Update()
    {
        if(playerCameraTransform != null) { return; }

        if(NetworkClient.connection.identity == null) { return; }

        playerCameraTransform = NetworkClient.connection.identity.
            GetComponent<RTSPlayer>().GetCameraTransform();

    }


    // Move the camera to mouse position.
    private void MoveCamera()
    {
        // Get the mouse position.
        Vector2 mousePos = Mouse.current.position.ReadValue();
    
        // Check if the mouse is inside the rect given,.
        // if does return to the point relative to the rect.
        if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect,
            mousePos,null,out Vector2 localPoint)){ return; }

        // The position on the minimap is relative to the orginal map
        // therefore we get the relative values of the position on the
        // minimap,and use it in the original map.
        Vector2 lerp = new Vector2(
            (localPoint.x - minimapRect.rect.x)/minimapRect.rect.width,  
            (localPoint.y - minimapRect.rect.y)/ minimapRect.rect.height);

        // Mathf.lerp return a value according the precenentage given
        // For example: if given values between 0-100, and the third 
        // value is 0.5,we will get 50 value.
        Vector3 newCameraPos = new Vector3(
            Mathf.Lerp(-mapScale,mapScale,lerp.x),
            playerCameraTransform.position.y,
            Mathf.Lerp(-mapScale,mapScale,lerp.y));

        playerCameraTransform.position = newCameraPos + new Vector3(0f,0f,offset);

        Debug.Log("Clicked on minimap");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveCamera();
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveCamera();
    }
}
