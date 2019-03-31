using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour
{

    public float a;
    private bool selected;
    private GameObject obstacle;

    void Start()
    {
        selected = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Obstacle")
            {
                print("It's working");
                obstacle = hitInfo.transform.gameObject;
                selected = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (selected == true) { 
            obstacle.GetComponent<Rigidbody>().AddForce(Input.GetAxis("Horizontal") * transform.right * a);
            obstacle.GetComponent<Rigidbody>().AddForce(Input.GetAxis("Vertical") * transform.forward * a);
        }
    }
}
