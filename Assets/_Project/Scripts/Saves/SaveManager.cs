using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
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
                try
                {
                    saveable.ResetState(_savedState);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            foreach (var saveable in saveObjects) try
                {
                    saveable.ApplyState(_savedState);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
        }

        public void Commit()
        {
            // Save state direct
            foreach (var saveable in saveObjects)
                try
                {
                    saveable.OnCommit();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            
            // Rewrite save state
            _savedState.Clear();
            foreach (var (key, value) in _stateChanges)
                _savedState[key] = value;
        }

        public void Clear()
        {
            _stateChanges.Clear();
            _savedState.Clear();
        }

        public void SetState(string state) => _stateChanges[state] = true;
        public void SetState(string state, object value) => _stateChanges[state] = value;

        public void RemoveState(string state) => _stateChanges.Remove(state);
        public void RemoveStateRegex(string regex)
        {
            foreach (var state in _stateChanges.Keys.ToArray()
                         .Where(key => Regex.IsMatch(key, regex)))
                _stateChanges.Remove(state);
        }
        
        public bool GetState(string state, [NotNullWhen(true)] out object value) => 
            _stateChanges.TryGetValue(state, out value);


#if UNITY_EDITOR
        [SerializeField] private bool debugRender;
        private void OnGUI()
        {
            if (!debugRender) return;
            var style = new GUIStyle(GUI.skin.box) {
                fontSize = 32
            };
            if(GUI.Button(new Rect(10, 10, 200, 50),"Save", style)) Commit();
            if(GUI.Button(new Rect(10, 80, 200, 50),"Restart", style)) Revert();
        }
#endif

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