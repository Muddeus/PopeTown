using System;
using Unity.VisualScripting;
using UnityEngine;
[ExecuteInEditMode]
public class ParticleFramerate : MonoBehaviour
{
    public bool active;
    public float updateTime;
    private float updateTimer;
    ParticleSystem ps;
    //private float timeElapsed = 0f;
    //private float displayTime = 0f;
    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        updateTimer = 0;
    }
    private void Update()
    {
        if (!active)
        {
            return;
        }
        
        if (updateTimer < updateTime)
        {
            updateTimer += Time.deltaTime;
        }
        else
        {
            ps.Simulate(updateTime, false, false, true);
            updateTimer = 0f;
        }
    }
}