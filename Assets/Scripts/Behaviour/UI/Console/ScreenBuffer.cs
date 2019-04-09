using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Util;

namespace Behaviour.Console
{
    public class ScreenBuffer
    {
        private List<String> _buffer;

        public ScreenBuffer()
        {
            _buffer = new List<String>();
            _buffer.Add("");
        }

        public ScreenBuffer AppendToLastLine(string text)
        {
            _buffer[_buffer.Count - 1] += text;

            return this;
        }

        public ScreenBuffer AppendNewLineCharacter()
        {
            return AppendToLastLine("\n");
        }

        public List<String> GetLastLines(int n)
        {
            var l = _buffer.TakeLast(n).ToList();
            return l;
        }

        public List<String> GetLastLine()
        {
            return GetLastLines(1);
        }


        public ScreenBuffer DeleteCharFromLastLine()
        {
            if (_buffer.Count == 0) return this; 
            
            var line = _buffer[_buffer.Count - 1];

            if (line.Length == 0) return this;
            SetLastLine(line.Remove(line.Length - 1));

            return this;
        }

        private void SetLine(int line, string text)
        {
            if (_buffer.Count == 0) return;
            
            _buffer[line] = text;
        }

        public void SetLastLine(string text)
        {
            SetLine(_buffer.Count - 1, text);
        }

        public void PrependStringToLastLine(string text)
        {
            _buffer[_buffer.Count - 1] = text + _buffer[_buffer.Count - 1];
        }

        public ScreenBuffer AddLine(string line)
        {
            _buffer.Add(line + "\n");

            return this;
        }

        public ScreenBuffer AddEmptyLine()
        {
            _buffer.Add("\n");
            return this;
        }
    }
}