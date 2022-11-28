using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core;
using UnityEngine;

namespace StatsSystem.Scripts.Runtime
{
    public class TagRegister : MonoBehaviour, ITaggable
    {
        public ReadOnlyCollection<string> Tags => _tagCount.Keys.ToList().AsReadOnly();

        private Dictionary<string, int> _tagCount = new Dictionary<string, int>();
        private Dictionary<string, float> _tagDuration = new Dictionary<string, float>();

        public event Action<string> OnTagAdded;
        public event Action<string> OnTagRemoved;

        public bool Contains(string tag)
        {
            return _tagCount.ContainsKey(tag);
        }

        public bool ContainsAny(IEnumerable<string> tags)
        {
            return tags.Any(_tagCount.ContainsKey);
        }

        public bool ContainsAll(IEnumerable<string> tags)
        {
            return tags.All(_tagCount.ContainsKey);
        }

        public bool SatisfiesRequirements(IEnumerable<string> mustBePresentTags, IEnumerable<string> musBeAbsentTags)
        {
            return ContainsAll(mustBePresentTags) && !ContainsAny(musBeAbsentTags);
        }

        public void AddTag(string tag)
        {
            if (_tagCount.ContainsKey(tag))
            {
                _tagCount[tag]++;
            }
            else
            {
                _tagCount.Add(tag, 1);
                OnTagAdded?.Invoke(tag);
            }
        }

        public void RefreshDuration(string tag, float value)
        {
            if(tag == null)
                return;
            
            _tagDuration[tag] = value;
        }

        public void RemoveTag(string tag)
        {
            if (_tagCount.ContainsKey(tag))
            {
                _tagCount[tag]--;
                if (_tagCount[tag] == 0)
                {
                    _tagCount.Remove(tag);
                    OnTagRemoved?.Invoke(tag);
                }
            }
            else
            {
                Debug.Log("Does not exist");
            }
        }

        public float GetDurationOfTag(ReadOnlyCollection<string> cooldownGrantedTags)
        {
            if (cooldownGrantedTags.Count == 0) return 0; 
            _tagDuration.TryGetValue(cooldownGrantedTags[0], out var duration);
            return duration;
        }
    }
}