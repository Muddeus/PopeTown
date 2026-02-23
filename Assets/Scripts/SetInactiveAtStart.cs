using UnityEngine;

public class SetInactiveAtStart : MonoBehaviour
{
    void Start()
    {
        if(gameObject.activeInHierarchy)gameObject.SetActive(false);
    }

}
