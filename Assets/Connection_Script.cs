using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection_Script : MonoBehaviour
{
    public GameObject player;
    public GameObject tape_End;

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<LineRenderer>().SetPosition(0, transform.position + new Vector3(0, tape_End.transform.localScale.y/2 - 0.3f, 0));
        player.GetComponent<LineRenderer>().SetPosition(1, tape_End.transform.position + new Vector3(0, tape_End.transform.localScale.y/2 - 0.3f, 0));
    }
}
