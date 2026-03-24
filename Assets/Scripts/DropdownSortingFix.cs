using UnityEngine;

public class DropdownSortingFix : MonoBehaviour
{
    private bool foundChild;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foundChild = false;
    }

    // Update is called once per frame
    void Update()
    {
        Transform lastChild = transform.GetChild(transform.childCount -1);
        if (lastChild.name == "Dropdown List")
        {
            foundChild = true;
            lastChild.gameObject.GetComponent<Canvas>().sortingLayerName = "OptionsBox";
        }
    }
}
