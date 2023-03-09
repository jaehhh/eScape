using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private Transform cameraPosition;

    [SerializeField]
    private float rotCamXAxisSpeed = 5; // 위아래
    [SerializeField]
    private float rotCamYAxisSpeed = 3; // 좌우

    private float limitMinX = -80; // 위 시선 각도
    private float limitMaxX = 70; // 아래 시선 각도
    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);

        camera.transform.position = cameraPosition.position;
        camera.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
