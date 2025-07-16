using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool mFreeMode = true;

    public bool IsFreeMode { get { return mFreeMode; } set { mFreeMode = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        Destroy(this);
    }
}
