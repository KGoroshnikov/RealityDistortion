using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Saves
{
    public abstract class SaveableBehaviour : MonoBehaviour
    {
        private SaveManager manager;
        private bool initialized = false;

        protected string Guid { get; private set; }
        
        protected void Initialize()
        {
            if (initialized) return;
            manager = FindAnyObjectByType<SaveManager>();
            manager.SaveObjects.Add(this);
            Guid = GUID.Generate().ToString();
            initialized = true;
        }

        protected void Dispose()
        {
            if (!initialized) return;
            manager.SaveObjects.Remove(this);
            initialized = false;
        }

        public abstract void ResetState(Dictionary<string, object> states);
        public abstract void ApplyState(Dictionary<string, object> states);
        public abstract void OnCommit();

        protected void Commit() => manager.Commit();
        protected void Revert() => manager.Revert();
        protected void SetState(string state) => manager.SetState(state);

        protected void SetState(string state, object value) => manager.SetState(state, value);
        protected void RemoveState(string state) => manager.RemoveState(state);
        protected void RemoveStateRegex(string regex) => manager.RemoveStateRegex(regex);
        
        public bool GetState(string state, [NotNullWhen(true)] out object value) => 
            manager.GetState(state, out value);
    }
}