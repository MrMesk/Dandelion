using UnityEngine;
using System.Collections;

public class CameraFollowing : MonoBehaviour
{

    private Vector3 velocity;
    public float smoothTimeZ;
    public float smoothTimeX;
    public GameObject player;
    Vector3 pos;

    void Start()
    {
        //player = GameObject.Find("Character");
    }

    void FixedUpdate()
    {
        pos = player.transform.position;
        Vector3 toMouse = transform.position;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {

            toMouse = hit.point - player.transform.position;
            toMouse = toMouse / 5;
            float magnitude = Mathf.Clamp(toMouse.magnitude,0,5f);
            toMouse = toMouse.normalized * magnitude;
            toMouse = toMouse / 10;
            toMouse = toMouse.normalized * magnitude;          
            pos += toMouse;


        }
        float posX = Mathf.SmoothDamp(transform.position.x, pos.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.z, pos.z, ref velocity.z, smoothTimeZ);
        transform.position = new Vector3(posX,transform.position.y , posY);
    }
}
