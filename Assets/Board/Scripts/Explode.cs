using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    [Tooltip("Fragments to be spawned on impact")]
    public GameObject fragments = null;

    private void spawnFragments()
    {
        Instantiate(fragments, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }

    //Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("StartBlock").GetComponent<StartGame>().editMode)
            spawnFragments();
    }
}
