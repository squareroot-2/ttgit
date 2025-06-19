using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using TMPro.Examples;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject tape_End;
    public GameObject spawn;
    public GameObject spawn_2;
    public GameObject locked_cam_pos;
    public Camera cam;
    public LayerMask collisionMask;
    public Image lockedIn;
    public float tossStrength = 50f;
    public Sprite locked;
    public Sprite unlocked;
    public bool followCam = true;
    public bool lockedCam = false;
    public float posY = 0;
    public float offset = 2;
    private DistanceJoint2D distanceJoint2D;
    private bool ready = false;
    private bool hold = false;
    private bool lockedMode = false;
    private int saveState;
    // Start is called before the first frame update
    void Start()
    {   
        posY = 0;

        saveState = PlayerPrefs.GetInt("SaveState", 0);

        if (saveState == 0)
        {
            transform.position = spawn.transform.position;
            tape_End.transform.position = spawn.transform.position;
        }
        else if (saveState == 1)
        {    
            transform.position = spawn_2.transform.position;
            tape_End.transform.position = spawn_2.transform.position;
            followCam = false;
            cam.orthographicSize = 10;
        }

        distanceJoint2D = player.GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20 || tape_End.transform.position.y < -20)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            tape_End.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            saveState = PlayerPrefs.GetInt("SaveState", 0);
            if (saveState == 0)
            {
                transform.position = spawn.transform.position;
                tape_End.transform.position = spawn.transform.position;
            }
            else if (saveState == 1)
            {
                transform.position = spawn_2.transform.position;
                tape_End.transform.position = spawn_2.transform.position;
            }
        }
        
        if (lockedCam)
        {
            cam.transform.position = locked_cam_pos.transform.position;
        }
        else if (followCam)
        {
            cam.transform.position = new Vector3(transform.position.x + offset, posY, -10);
        }
        else
        {
            cam.transform.position = new Vector3(transform.position.x + offset, transform.position.y, -10);
            cam.orthographicSize = 10;
        }

        if (lockedMode)
        {
            lockedIn.sprite = locked;
        }
        else
        {
            lockedIn.sprite = unlocked;
        }

        player.GetComponent<LineRenderer>().SetPosition(0, transform.position + new Vector3(0, tape_End.transform.localScale.y/2, 0));
        player.GetComponent<LineRenderer>().SetPosition(1, tape_End.transform.position + new Vector3(0, tape_End.transform.localScale.y/2 - 0.3f, 0));

        RaycastHit2D platformCheck = Physics2D.Raycast(tape_End.transform.position, Vector2.down, 0.5f, collisionMask);
            
        if (platformCheck)
        {
            ready = true;
        }
        else
        {
            ready = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (ready == true && ((distanceJoint2D.enabled == true && distanceJoint2D.distance < 1) == false))
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

                Vector3 rotation = mousePos - tape_End.transform.position;
                
                Vector2 rotation_2d = rotation;

                tape_End.GetComponent<Rigidbody2D>().AddForce(rotation_2d.normalized * tossStrength);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.W) && ready == true)
        {
            distanceJoint2D.autoConfigureDistance = true;
            distanceJoint2D.enabled = true;
            hold = true;
            tape_End.GetComponent<Rigidbody2D>().mass = 25;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (lockedMode == false)
                distanceJoint2D.enabled = false;

            hold = false;
            tape_End.GetComponent<Rigidbody2D>().mass = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lockedMode == true)
            {
                distanceJoint2D.enabled = false;
                lockedMode = false;
            }
            else
            {
                distanceJoint2D.autoConfigureDistance = true;
                distanceJoint2D.enabled = true;
                lockedMode = true;
            }
        }

        if (hold == true)
        {
            if (distanceJoint2D.distance > 100)
            {
                distanceJoint2D.distance = 100;
            }
            
            distanceJoint2D.autoConfigureDistance = false; 
            distanceJoint2D.distance -= Time.deltaTime * (distanceJoint2D.distance/2);
        }
    }
}
