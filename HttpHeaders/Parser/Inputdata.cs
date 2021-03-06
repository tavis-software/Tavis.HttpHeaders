﻿using System.Collections.Generic;

namespace Tavis.Parser
{
    public class Inputdata
    {
        private int Index; 
        public string Input { get; private set; }
        public Dictionary<string, Terminal> Expressions { get; set; } 
        public Inputdata(string input)
        {
            Index = 0;
            Input = input;
            Expressions = new Dictionary<string, Terminal>();
        }

        public bool AtEnd {
            get { return Index == Input.Length; }
        }

        public char GetNext()
        {
            return Input[Index++];
        }
        public string GetNext(int chars)
        {
            if (Index + chars > Input.Length) return null;
            var result= Input.Substring(Index, chars);
            Index += chars;
            return result;
        }

        internal void MoveBack()
        {
            Index--;
        }

        public string Processed
        {
            get
            {
                return Input.Substring(0, Index);
            }
        }
        public string SinceMark
        {
            get
            {
                return Input.Substring(_Mark, Index - _Mark);
            }
        }

        internal void Mark()
        {
            _Mark = Index;
        }
        internal void MoveToMark()
        {
            Index =_Mark ;
        }
        public string GetSinceMark()
        {
            return Input.Substring(_Mark, Index - _Mark);
        }
        private int _Mark;
    }
}