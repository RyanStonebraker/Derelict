using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Despawn : MonoBehaviour {

    public int DestroyTime = 3;

    public void Wait(float seconds, Action action)
    {
        StartCoroutine(_wait(seconds, action));
    }

    IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    private void deleteFragments()
    {
        Wait (DestroyTime, () => { Destroy(gameObject); });
    }

    //Enter the world and die in 3 seconds
    void Awake ()
    {
        deleteFragments();
    }

}
