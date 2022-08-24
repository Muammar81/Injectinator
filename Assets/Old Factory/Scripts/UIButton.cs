using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] private XBoxController inputDevicePrefab;

    private void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(SpawnButton);
    }

    private void SpawnButton()
    {
        var existingButtons = FindObjectsOfType<XBoxController>(); //can't use with interface
        if (existingButtons.Length > 0) return;
        Instantiate(inputDevicePrefab);
    }
}