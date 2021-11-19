using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// Button for placing building in the map.
public class BuildingButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    // Serlize of the current building info.
    [SerializeField] private Building building = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text priveText = null;
    // To set the physics layer of the floor we want to put the building on.
    [SerializeField] private LayerMask floorMask = new LayerMask();


    // Camera for using ray casting.
    private Camera mainCamera;
    // The Building collider.
    private BoxCollider buildingCollider;
    // Note the player for a new building if he has enough sum.
    private RTSPlayer player;
    // Preview Of the visual building we want to put.
    private GameObject buildingPreviewInstance;
    // Change the color to visual if we can put the building or not.
    private Renderer buildingRenderInstance;



    void Start()
    {
        mainCamera = Camera.main;

        iconImage.sprite = building.GetIcon();

        priveText.text = building.GetPrice().ToString();

        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        buildingCollider = building.GetComponent<BoxCollider>();
    }


    void Update()
    {

        if (buildingPreviewInstance == null) { return; }

        UpdateBuildingPreview();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if(eventData.button != PointerEventData.InputButton.Left) { return; }

        if(player.GetResources() < building.GetPrice()) { return; }

        buildingPreviewInstance = Instantiate(building.GetBuildingPreview());

        buildingRenderInstance = buildingPreviewInstance.GetComponentInChildren<Renderer>();

        buildingPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(buildingPreviewInstance == null) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity,floorMask))
        {
            player.CmdTryPlaceBuilding(building.GetId(), hit.point);
        }

        Destroy(buildingPreviewInstance);
    }


    private void UpdateBuildingPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }

        buildingPreviewInstance.transform.position = hit.point;

        if (!buildingPreviewInstance.activeSelf)
        {
            buildingPreviewInstance.SetActive(true);
        }

        Color color = player.CanPlaceBuilding(buildingCollider,hit.point) ? Color.green : Color.red;

        buildingRenderInstance.material.SetColor("_BaseColor", color);

    }



}
