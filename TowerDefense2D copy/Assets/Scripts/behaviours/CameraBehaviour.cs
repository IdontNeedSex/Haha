using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a minimal script to move and zoom the camera. You can extend this script e.g. with interpolated movement, cursor drag controls etc.
/// This camera has no influence on the game itself
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private Vector2 minMaxZoom = new(3, 10);

    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) // If left arrow button cicked 
        {
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime,0,0));
        }

        if (Input.GetKey(KeyCode.RightArrow)) // If right arrow button cicked 
        {
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        }
        
        if (Input.GetKey(KeyCode.UpArrow)) // If if up arrow button cicked 
        {
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime,0));
        }

        if (Input.GetKey(KeyCode.DownArrow)) // If down arrow button cicked 
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
        
        // manages zooming, clmaped to a max and min Size. Input.mouseScrollDelta => amount the mouse wheel was turned
        _camera.orthographicSize =
            Math.Clamp(_camera.orthographicSize + (-Input.mouseScrollDelta.y * zoomSpeed), minMaxZoom.x, minMaxZoom.y);

    }
}
