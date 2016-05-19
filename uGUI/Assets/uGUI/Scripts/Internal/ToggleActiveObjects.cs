using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace uTools
{
    public class ToggleActiveObjects : MonoBehaviour
    {
        public GameObject[] actives;
        public GameObject[] inactives;

        Toggle mToggle;

        void Awake()
        {
            mToggle = GetComponent<Toggle>();
            mToggle.onValueChanged.AddListener(OnClick);
        }

        void Start()
        {
            ActiveObject(mToggle.isOn);
        }

        void OnClick(bool isOn)
        {
            ActiveObject(isOn);
        }

        void ActiveObject(bool toggle)
        {
            foreach (var item in actives)
            {
                item.SetActive(toggle);
            }
            foreach (var item in inactives)
            {
                item.SetActive(!toggle);
            }
        }
    }
}
