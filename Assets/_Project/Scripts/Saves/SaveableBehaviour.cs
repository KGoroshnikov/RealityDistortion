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
        public abstract void OnCommit();

        protected void Commit() => manager.Commit();
        protected void Revert() => manager.Revert();
        protected void SetState(string state) => manager.SetState(state);

        protected void SetState(string state, object value) => manager.SetState(state, value);

        protected void RemoveState(string state) => manager.RemoveState(state);
    }
}