using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Gudetama : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown() {
      FindObjectOfType<DialogueRunner>().StartDialogue("Gudetama");
    }
}
