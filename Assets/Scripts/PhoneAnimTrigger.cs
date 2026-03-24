using UnityEngine;

public class PhoneAnimTrigger : MonoBehaviour
{
    public GameObject objToEnable;

    private void OnDestroy()
    {
        objToEnable.SetActive(true);
    }
}
