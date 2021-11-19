using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    // The cinamachine camera.
    [SerializeField] private Transform playerCameraTransform;
    
    // The speed movment of the camera.
    [SerializeField] private float speed = 20f;
    
    // For the mouse moving the camera. 
    [SerializeField] private float screenBoarderThickness = 10;
    
    // Limit the camera movment.
    [SerializeField] private Vector2 screenXLimits = Vector2.zero;
    [SerializeField] private Vector2 screenZLimits = Vector2.zero;

    // store the last input we got from player.
    private Vector2 previousInput;

    // The input controller we created in inputs
    private Controls controls;


    public override void OnStartAuthority()
    {
        // Override the main camera
        playerCameraTransform.gameObject.SetActive(true);

        controls = new Controls();

        // Set the listners to the input event.
        controls.Player.MoveCamera.performed += SetPreviousInput;
        controls.Player.MoveCamera.canceled += SetPreviousInput;

        controls.Enable();
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority || !Application.isFocused) { return; }

        UpdateCameraPosition();
        
    }


    private void UpdateCameraPosition()
    {
        Vector3 pos = playerCameraTransform.position;

         // If the keyboard wasnt pressed check the mouse.
         if (previousInput == Vector2.zero)
         {
             Vector3 cursorMovment = Vector3.zero;

             Vector2 cursorPosition = Mouse.current.position.ReadValue();

             if (cursorPosition.y >= Screen.height - screenBoarderThickness)
             {
                 cursorMovment.z += 1;
             }
             else if (cursorPosition.y <= screenBoarderThickness)
             {
                 cursorMovment.z -= 1;
             }
             if (cursorPosition.x >= Screen.width - screenBoarderThickness)
             {
                 cursorMovment.x += 1;
             }
             else if (cursorPosition.x <= screenBoarderThickness)
             {
                 cursorMovment.x -= 1;
             }

             pos += cursorMovment.normalized * speed * Time.deltaTime;
         }
         else 
        {
            pos += new Vector3(previousInput.x, 0f, previousInput.y) * speed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, screenXLimits.x, screenXLimits.y);
        pos.z = Mathf.Clamp(pos.z, screenZLimits.x, screenZLimits.y);
       

        playerCameraTransform.position = pos;
    }


    // The function called by the listeners 'preform' and 'cancel'
    private void SetPreviousInput(InputAction.CallbackContext ctx)
    {
        previousInput = ctx.ReadValue<Vector2>();
    }

}
