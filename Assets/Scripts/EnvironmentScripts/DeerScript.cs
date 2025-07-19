using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerScript : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AudioClip m_FootstepsSFX;

    private void OnEnable()
    {
        EventSystem.OnSpawnDoctor += HandleDestroy;
    }

    private void OnDisable()
    {
        EventSystem.OnSpawnDoctor -= HandleDestroy;
    }

    private void HandleSetActivateThingsToDraw()
    {
        EventSystem.ActivateSketchChoosing();
    }

    private void HandleDestroy()
    {
        Destroy(gameObject);
    }

    private void PlayFootstepsSFX()
    {
        SoundManager.instance.PlayOneShotSound(m_FootstepsSFX, 0.3f, transform.position);
    }
}
