using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera m_camera;

    [Header("Horizontal & Vertical movement")]
    
    [Header("Camera parametors")]
    [SerializeField]
    private float m_speed = 100f; // Скорость перемещения камеры 
    [SerializeField]
    private float m_movementLerp = 0.5f; // Скорость поспевания камеры (сглаживание)
    
    [Header("Mouse input")]
    [SerializeField]
    private bool m_mouseInputActive = true;
    [SerializeField]
    private int m_outline = 30; // Отсуп в пикселях, сколько нужно, чтобы камера начала двигаться
    

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
    private float m_scalingSpeed = 300f; // Скорость приближение/отдаления камеры
    [SerializeField]
    private float m_scalingLerp = .5f; // Сглаживание перемещения
    [SerializeField]
    private float m_minimalOrtographicSize = 5f; // Минимальный масштаб камеры
    private float m_maximalOrtographicSize; // Максимальный масштаб камеры

    private float m_cameraFormat; // = (Ширина / Высоту)

    private void Start()
    {
        m_camera = GetComponent<Camera>();

        m_cameraFormat = m_camera.pixelWidth * 1f / m_camera.pixelHeight;

        // Минимальное расстояние между вертикальной и горизонтальной границей
        m_maximalOrtographicSize = Mathf.Min(
            WorldMap.instance.width / m_cameraFormat / 2f,
            WorldMap.instance.height / 2f); 
    }

    private void Update()
    {
        float horizontalDelta = 0;
        float verticalDelta = 0;

        // Движение камеры за счёт курсора
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

        // Движение камеры за счёт кнопок
        if (m_keyInputActive)
        {
            horizontalDelta += Input.GetAxis(m_horizontalAxis);

            verticalDelta += Input.GetAxis(m_verticalAxis);
        }

        // Изменение масштаба камеры за счёт колёсика мыши
        if (m_scalingActive)
        {
            m_camera.orthographicSize =
            Mathf.Max(
                m_minimalOrtographicSize,
                Mathf.Min(
                    Mathf.Lerp(m_camera.orthographicSize,
                    m_camera.orthographicSize + -1f * Input.mouseScrollDelta.y *
                    m_scalingSpeed * Time.deltaTime, 
                    m_scalingLerp),
                    m_maximalOrtographicSize
                    )
                );
        }
        if (Input.GetKey(KeyCode.E))
        {
            m_camera.orthographicSize = Mathf.Max(
                m_minimalOrtographicSize,
                Mathf.Min(Mathf.Lerp(m_camera.orthographicSize,
                m_camera.orthographicSize + -1f * m_scalingSpeed * Time.deltaTime *0.75f,
                m_scalingLerp),
                m_maximalOrtographicSize)
           );
        }
        if (Input.GetKey(KeyCode.Q))
        {
            m_camera.orthographicSize = Mathf.Max(
                m_minimalOrtographicSize,
                Mathf.Min(Mathf.Lerp(m_camera.orthographicSize,
                m_camera.orthographicSize + m_scalingSpeed * Time.deltaTime * 0.75f,
                m_scalingLerp),
                m_maximalOrtographicSize)
           );
        }

        // Определение новой позиции камеры
        Vector3 newPosition = Vector3.Lerp(transform.position, transform.position +
            new Vector3(horizontalDelta, verticalDelta) * m_speed *
            m_camera.orthographicSize / m_minimalOrtographicSize * Time.deltaTime, m_movementLerp);

        // Минимальное расстояние до горизонтальной границы
        float widthPadding = m_camera.orthographicSize * m_cameraFormat;
        // Минимальное расстояние до вертикальной границы
        float heightPadding = m_camera.orthographicSize;

        // Если камера вышла за пределы карты, то данный код её туда не пускает
        newPosition.x = Mathf.Max(widthPadding,
            Mathf.Min(newPosition.x, WorldMap.instance.width - widthPadding));
        newPosition.y = Mathf.Max(heightPadding,
            Mathf.Min(newPosition.y, WorldMap.instance.height - heightPadding));

        transform.position = newPosition;
    }
}
