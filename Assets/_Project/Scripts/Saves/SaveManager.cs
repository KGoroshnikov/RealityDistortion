using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Saves
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private List<SaveableBehaviour> saveObjects = new();
        private readonly Dictionary<string, object> _stateChanges = new();
        private readonly Dictionary<string, object> _savedState = new();
        
        public List<SaveableBehaviour> SaveObjects => saveObjects;

        public void Revert()
        {
            _stateChanges.Clear();
            foreach (var saveable in saveObjects)
                saveable.ResetState(_savedState);
            foreach (var saveable in saveObjects)
                saveable.ApplyState(_savedState);
                
        }

        public void Commit()
        {
            foreach (var (key, value) in _stateChanges)
                _savedState.Add(key, value);
        }

        public void AddState(string state) => _stateChanges.Add(state, true);
        public void AddState(string state, object value) => _stateChanges.Add(state, value);

        public void RemoveState(string state) => _stateChanges.Remove(state);

        // public void Save()
        // {
        //     
        // }
        //
        // public void Load()
        // {
        //     
        // }
    }
}