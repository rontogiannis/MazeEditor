using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CameraControl : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    public float cameraDistanceMax = 20f;
    public float cameraDistanceMin = 2.5f;
    public float scrollSpeed = 0.5f;

    float cameraDistance;

    void Awake()
    {
        cameraDistance = Camera.main.orthographicSize;
    }

    void UpdateZoom()
    {
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        Camera.main.orthographicSize = cameraDistance;
    }

    void UpdateMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector2 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector2 move = new Vector3(-pos.x * dragSpeed * Time.deltaTime, -pos.y * dragSpeed * Time.deltaTime);

        transform.Translate(move, Space.World);
    }

    void Update()
    {
        UpdateZoom();
        UpdateMove();
    }


}