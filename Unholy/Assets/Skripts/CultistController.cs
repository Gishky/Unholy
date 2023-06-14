using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Managing;

public class CultistController : MonoBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 30f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    private Camera playerCamera;

    private void Start()
    {
        if (GameManager.Instance() == null || GameManager.Instance().clientPlayerType != PlayerType.Cultist)
        {
            gameObject.GetComponent<CultistController>().enabled = false;
            return;
        }

        characterController = GetComponent<CharacterController>();

        // Free cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        //get Camera
        playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        playerCamera.transform.SetParent(transform);
        playerCamera.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        transform.position += (moveDirection * Time.deltaTime);
    }
}