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
        public IReadOnlyDictionary<string, object> StateChanges => _stateChanges;
        public IReadOnlyDictionary<string, object> SavedState => _savedState;

        public void Revert()
        {
            // Rewrite temp state
            _stateChanges.Clear();
            foreach (var (key, value) in _savedState) 
                _stateChanges[key] = value;
            
            // Reset Objects
            foreach (var saveable in saveObjects)
                saveable.ResetState(_savedState);
            foreach (var saveable in saveObjects)
                saveable.ApplyState(_savedState);
                
        }

        public void Commit()
        {
            // Rewrite save state
            _savedState.Clear();
            foreach (var (key, value) in _stateChanges)
                _savedState[key] = value;
        }

        public void SetState(string state) => _stateChanges[state] = true;
        public void SetState(string state, object value) => _stateChanges[state] = value;

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