using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorScript : MonoBehaviour
{
    [SerializeField] private GameObject mLeftArmToSliceOff;
    [SerializeField] private GameObject mRightArmToSliceOff;
    [SerializeField] private GameObject mSmokeEffect;
    [SerializeField] private Texture2D m_HeartGoneTex;
    private int sliceIndex = 0;

    private void OnEnable()
    {
        EventSystem.OnSwapEnvironments += DestroyDoc;
        EventSystem.OnFixConfrontationIssue += DestroyDoc;
        EventSystem.OnSliceDoctor += HandleSlice;
    }

    private void OnDisable()
    {
        EventSystem.OnSwapEnvironments -= DestroyDoc;
        EventSystem.OnFixConfrontationIssue -= DestroyDoc;
        EventSystem.OnSliceDoctor -= HandleSlice;
    }

    private void DestroyDoc(EnvironmentSwitchManager.Environments env, GameEvent gameEvent)
    {
        Destroy(gameObject);
    }

    private void DestroyDoc(FinalConfrontationManager.Issues issueFixed)
    {
        Destroy(gameObject);
    }

    private void StealHeart()
    {
        GameObject heart = GameObject.FindGameObjectWithTag("Heart");
        if (heart != null)
        {
            heart.transform.SetParent(transform);
        }
    }

    public void DeactivateSmokeEffect()
    {
        mSmokeEffect.SetActive(false);
    }

    public void TriggerSketchChange()
    {
        EventSystem.ChangeSketch();
    }

    private void HandleSlice()
    {
        switch (sliceIndex)
        {
            case 0:
                mRightArmToSliceOff.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                break;
            case 1:
                mLeftArmToSliceOff.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
                break;
            case 2:
                gameObject.GetComponentInChildren<Renderer>().materials[1].SetTexture("_UnlitTex", m_HeartGoneTex);
                break;
        }

        sliceIndex++;
    }
}
