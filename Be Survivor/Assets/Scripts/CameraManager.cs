using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void LateUpdate()
    {
        Vector3 pos = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.2f, -10); //Kamera playeri takip eder
        transform.position = pos;
    }
}
