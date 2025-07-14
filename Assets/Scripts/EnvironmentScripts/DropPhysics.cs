using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPhysics : MonoBehaviour
{
    [SerializeField] private Vector3 mGravity = new Vector3(0, -9.81f, 0);
    [SerializeField] private bool destroysAtKillFloor = true;
    private Vector3 mVelocity;
    private Vector3 mAcceleration;
    //private float mInvMass = 1;
    private float mDampening = 0.5f;
    private float mKillFloor = -10.0f;


    private void FixedUpdate()
    {
        Integrate(Time.fixedDeltaTime);

        HandleKillFloor();
    }

    private void Integrate(float dt)
    {
        transform.position += mVelocity * dt;

        mAcceleration += mGravity;

        mVelocity += mAcceleration * dt;

        mVelocity *= Mathf.Pow(mDampening, dt);
    }

    private void HandleKillFloor()
    {
        if (destroysAtKillFloor)
        {
            if (transform.position.y <= mKillFloor)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
