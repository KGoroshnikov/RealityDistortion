using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Saves
{
    public class SaveManager : MonoBehaviour
    {
        private readonly List<SaveableBehaviour> saveObjects = new();
        private readonly HashSet<string> _stateChanges = new();
        private readonly HashSet<string> _savedState = new();
        
        public List<SaveableBehaviour> SaveObjects => saveObjects;

        public void Revert()
        {
            _stateChanges.Clear();
            foreach (var saveable in saveObjects)
                saveable.ResetState(_savedState);
                
        }

        public void Commit()
        {
            foreach (var change in _stateChanges)
                _savedState.Add(change);
        }

        public void AddState(string state) => _stateChanges.Add(state);

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