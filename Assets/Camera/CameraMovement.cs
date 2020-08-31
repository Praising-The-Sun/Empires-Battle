using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera m_camera;

    [Header("Horizontal & Vertical movement")]
    
    [Header("Camera parametors")]
    [SerializeField]
    private float m_speed = 100f;
    [SerializeField]
    private float m_movementLerp = 0.5f;
    
    [Header("Mouse input")]
    [SerializeField]
    private bool m_mouseInputActive = true;
    [SerializeField]
    private int m_outline = 30;
    

    [Header("Key input")]
    [SerializeField]
    private bool m_keyInputActive = true;
    [SerializeField]
    private string m_horizontalAxis = "Horizontal";
    [SerializeField]
    private string m_verticalAxis = "Vertical";

    [Header("Scaling")]
    [SerializeField]
    private bool m_scalingActive = true;
    [SerializeField]
    private float m_scalingSpeed = 10f;
    [SerializeField]
    private float m_scalingLerp = .5f;
    [SerializeField]
    private float m_minimalOrtographicSize = 5f;
    private float m_maximalOrtographicSize;

    private float m_cameraFormat;

    private void Start()
    {
        m_camera = GetComponent<Camera>();

        m_cameraFormat = m_camera.pixelWidth * 1f / m_camera.pixelHeight;
        m_maximalOrtographicSize = Mathf.Min(
            WorldMap.instance.width / m_cameraFormat / 2f,
            WorldMap.instance.height / 2f);
    }

    private void Update()
    {
        float horizontalDelta = 0;
        float verticalDelta = 0;

        if (m_mouseInputActive)
        {
            Vector2 mousePosition = Input.mousePosition;

            if (Mathf.Abs(mousePosition.x) <= m_outline)
                horizontalDelta = -1;
            else if (Mathf.Abs(m_camera.pixelWidth - mousePosition.x) <= m_outline)
                horizontalDelta = 1;

            if (Mathf.Abs(mousePosition.y) <= m_outline)
                verticalDelta = -1;
            else if (Mathf.Abs(m_camera.pixelHeight - mousePosition.y) <= m_outline)
                verticalDelta = 1;
        }

        if (m_keyInputActive)
        {
            horizontalDelta += Input.GetAxis(m_horizontalAxis);

            verticalDelta += Input.GetAxis(m_verticalAxis);
        }

        if (m_scalingActive)
        {
            m_camera.orthographicSize = Mathf.Min(m_maximalOrtographicSize, Mathf.Max(m_minimalOrtographicSize,
                Mathf.Lerp(m_camera.orthographicSize,
                m_camera.orthographicSize + -1f * Input.mouseScrollDelta.y *
                m_scalingSpeed * Time.deltaTime, 
                m_scalingLerp)));
        }

        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position +
            new Vector3(horizontalDelta, verticalDelta) * m_speed *
            m_camera.orthographicSize / m_minimalOrtographicSize * Time.deltaTime, m_movementLerp);

        float widthPadding = m_camera.orthographicSize * m_cameraFormat;
        float heightPadding = m_camera.orthographicSize;

        newPosition.x = Mathf.Max(widthPadding,
            Mathf.Min(newPosition.x, WorldMap.instance.width - widthPadding));
        newPosition.y = Mathf.Max(heightPadding,
            Mathf.Min(newPosition.y, WorldMap.instance.height - heightPadding));

        transform.position = newPosition;
    }
}
