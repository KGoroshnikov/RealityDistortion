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

        public abstract void ResetState(HashSet<string> states);

        public void AddState(string state) => manager.AddState(state);

        public void RemoveState(string state) => manager.RemoveState(state);
    }
}