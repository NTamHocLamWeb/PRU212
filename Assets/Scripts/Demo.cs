using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Demo1");
        Debug.LogWarning("Demo2");
        Debug.LogError("Demo3");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            print("Up arrow");
        }
		if (Input.GetKey(KeyCode.DownArrow))
		{
			print("Down arrow");
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			print("Left arrow");
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			print("Right arrow");
		}
	}
}
