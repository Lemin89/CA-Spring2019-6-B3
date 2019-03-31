using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonDoorController : MonoBehaviour
{
    public GameObject button;
    public GameObject door;
    public MyBehaviorTree bt;

    private Renderer rend;
    private Vector3 defaultRot;
    private Vector3 openRot;
    private bool open;
    private bool opened;

    // Use this for initialization
    void Start()
    {
        rend = button.GetComponent<Renderer>();
        open = false;
        opened = false;
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + 90, defaultRot.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(open+" "+bt.trigger);
        open = bt.trigger;
        if (open == true && opened == false)
        {
            opened = true;
            door.transform.Rotate(door.transform.rotation.x, door.transform.rotation.y + 90, door.transform.rotation.z);
            door.transform.position = new Vector3(door.transform.position.x+1, door.transform.position.y, door.transform.position.z+1);
            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);
        }
    }
    void OnTriggerEnter()
    {
        Debug.Log("triggered switch");
    }
}

