using UnityEngine;

namespace SchedulingUtilities
{
    public class ToggleGameObjectActive : MonoBehaviour
    {
        public GameObject go;

        public void Toggle()
        {
            if (go == null) return;
            go.SetActive(!go.activeSelf);
        }
    }
}
