using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Saves
{
    public abstract class SaveableBehaviour : MonoBehaviour
    {
        private SaveManager manager;
        
        protected void Initialize()
        {
            manager = FindAnyObjectByType<SaveManager>();
            manager.SaveObjects.Add(this);
        }

        protected void Dispose() => manager.SaveObjects.Remove(this);

        public abstract void ResetState(Dictionary<string, object> states);
        public abstract void ApplyState(Dictionary<string, object> states);

        public void SetState(string state) => manager.AddState(state);

        public void SetState(string state, object value) => manager.AddState(state, value);

        public void RemoveState(string state) => manager.RemoveState(state);
    }
}