using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax; // stops over rotate
    [SerializeField] private GameObject cameraTarget;

    float camRotX; // camera x axis rotation

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        //  only need x and y because mouse only moves in two directions
        float mouseX = Input.GetAxisRaw("Mouse X") * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sens;

        // Up / Down
        camRotX -= mouseY;
        // need to clamp the rotation max so we dont go to far down or back
        camRotX = Mathf.Clamp(camRotX, lockVertMin, lockVertMax);
        transform.localRotation = Quaternion.Euler(camRotX, 0, 0);

        //The camera is dragged under the player so parent will target the player
        //transform.parent.transform.localRotation = Quaternion.Euler(0, mouseX, 0);

        //Vector3.up is a short cut for 0, 1, 0
        //transform.parent.Rotate(Vector3.up * mouseX);
        cameraTarget.transform.parent.Rotate(Vector3.up * mouseX);
        //player.transform.Rotate(Vector3.up * mouseX);


    }
}

