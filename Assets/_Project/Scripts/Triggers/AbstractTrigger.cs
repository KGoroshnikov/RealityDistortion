using _Project.Scripts.Saves;
using UnityEngine;

namespace Triggers
{
    public abstract class AbstractTrigger : SaveableBehaviour
    {
        public void LogInfo(string message) => Debug.Log(message);
        public void LogWarning(string message) => Debug.LogWarning(message);
        public void LogError(string message) => Debug.LogError(message);
        public new void Destroy(Object obj) => GameObject.Destroy(obj);
        public void DestroyGameObject(Component obj) => GameObject.Destroy(obj.gameObject);
    }
}