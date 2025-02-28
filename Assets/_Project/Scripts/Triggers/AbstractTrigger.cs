using UnityEngine;

namespace Triggers
{
    public class AbstractTrigger : MonoBehaviour
    {
        public void LogInfo(string message) => Debug.Log(message);
        public void LogWarning(string message) => Debug.LogWarning(message);
        public void LogError(string message) => Debug.LogError(message);
        public new void Destroy(Object obj) => GameObject.Destroy(obj);
    }
}