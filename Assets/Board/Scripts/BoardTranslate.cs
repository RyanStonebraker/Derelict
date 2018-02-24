using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTranslate : MonoBehaviour {

    public float distance = 3;

    public bool snappedInPlace = false;

    void Update()
    {

        if (GameObject.Find("StartBlock"))
            return;
        else if (!snappedInPlace && !GameObject.Find("StartBlock"))
        {
            Vector3 snappedPos = gameObject.transform.position;

            transform.position = new Vector3(snappedPos.x + distance, snappedPos.y, snappedPos.z);
            // transform.Rotate(0, -20, 0);
            snappedInPlace = true;
        }
    }
}
