using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quantum : MonoBehaviour
{
    [Header("持续时间")]
    public float duration;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(duration);

        MyObjectPool.Instance.PushObject(gameObject);
    }
}
