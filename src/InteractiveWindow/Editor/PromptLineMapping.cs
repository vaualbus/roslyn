﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Roslyn.Editor.InteractiveWindow
{
    /// <summary>
    /// Maps line numbers in projection buffer to indices of projection spans corresponding to primary and stdin prompts.
    /// </summary>
    internal sealed class PromptLineMapping
    {
        private List<KeyValuePair<int, int>> map = new List<KeyValuePair<int, int>>();

        /// <summary>
        /// If true the map might not be consistent with projection spans.
        /// Used to work around the lack of an editor feature that would allow us 
        /// to edit a subject buffer and projection spans atomically.
        /// </summary>
        public bool IsInconsistentWithProjections { get; set; }

        public PromptLineMapping()
        {
        }

        public void Clear()
        {
            map = new List<KeyValuePair<int, int>>();
        }

        public int Count
        {
            get { return map.Count; }
        }

        public void Add(int lineNumber, int projectionIndex)
        {
            map.Add(new KeyValuePair<int, int>(lineNumber, projectionIndex));
        }

        public void RemoveLast()
        {
            map.RemoveAt(map.Count - 1);
        }

        public KeyValuePair<int, int> this[int index]
        {
            get { return map[index]; }
            set { map[index] = value; }
        }

        /// <summary>
        /// Binary search for a prompt located on given line number. 
        /// If no prompt is on the given line number returns the closest preceding prompt.
        /// </summary>
        /// <returns>An index in the prompt line map.</returns>
        internal int GetMappingIndexByLineNumber(int lineNumber)
        {
            int start = 0;
            int end = map.Count - 1;
            while (true)
            {
                Debug.Assert(start <= end);

                int mid = start + ((end - start) >> 1);
                int key = map[mid].Key;

                if (lineNumber == key)
                {
                    return mid;
                }

                if (mid == start)
                {
                    Debug.Assert(start == end || start == end - 1);
                    return (lineNumber >= map[end].Key) ? end : mid;
                }

                if (lineNumber > key)
                {
                    start = mid;
                }
                else
                {
                    end = mid;
                }
            }
        }

        [Conditional("DEBUG")]
        public void Dump(string name)
        {
            Debug.Write("PLM (" + name + "): ");
            foreach (var plm in map)
            {
                Debug.Write(string.Format("{0} -> {1}; ", plm.Key, plm.Value));
            }

            Debug.WriteLine("");
        }
    }
}
