using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSenseSystem : MonoBehaviour
{
    private static AttackSenseSystem instance;
    public static AttackSenseSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Transform.FindObjectOfType<AttackSenseSystem>();
            }
            return instance;
        }
    }
    private bool isShaking = false;
    public float waitStartPauseTime;


    public void HitPause(int duration)
    {
        StartCoroutine(WaitStartPause(waitStartPauseTime,duration));
    }

    public void CameraShake(float duration,float strength)
    {
        if(isShaking == false)
        {
            StartCoroutine(Shake(duration, strength));
        }
    }


   IEnumerator Pause(int duration)
    {
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
    }
    IEnumerator Shake(float duration, float strength)
    {
        isShaking = true;
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;
        while(duration > 0)
        {
            camera.position = Random.insideUnitSphere * strength + startPosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }
    IEnumerator WaitStartPause(float WaitStartPauseTime,int duration)
    {
        yield return new WaitForSecondsRealtime(WaitStartPauseTime);
        StartCoroutine(Pause(duration));
    }
}
