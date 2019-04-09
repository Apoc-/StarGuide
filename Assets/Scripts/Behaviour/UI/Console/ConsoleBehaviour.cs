using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Behaviour.Console
{

    [RequireComponent(typeof(ConsoleCommandHandler))]
    public class ConsoleBehaviour : MonoBehaviour
    {
        private ScreenBuffer _screenBuffer = new ScreenBuffer();
        private String _currentLineBuffer = "";
        private int _maxLines = 11;
        private int _maxLineLength = 52;
        private String _promptString = "cmd> ";
        private String _inputCaret = "_";
        public TextMeshProUGUI Screen;
        public TMP_FontAsset FontAsset;
        private ConsoleCommandHandler _consoleCommandHandler;

        private void Start()
        {
            _consoleCommandHandler = GetComponent<ConsoleCommandHandler>();
            _screenBuffer
                .AddLine("Starting up...")
                .AddLine("Welcome to Starship BOS!")
                .AddEmptyLine();
            
            _currentLineBuffer = "";
            ApplyCurrentLineBufferToScreenBuffer();
        }

        public void ClearScreen()
        {
            _screenBuffer = new ScreenBuffer();
        }

        private void Update()
        {
            HandleInput();
            ApplyCurrentLineBufferToScreenBuffer();
            WriteBufferToScreen();
        }
        
        private void ApplyCurrentLineBufferToScreenBuffer()
        {
            _screenBuffer.SetLastLine(_promptString + _currentLineBuffer + _inputCaret);
        }

        private void WriteBufferToScreen()
        {
            //var lineBufferLines = (_currentLineBuffer.Length-1) /_maxLineLength;
            
            Screen.text = _screenBuffer
                .GetLastLines(_maxLines)
                .SelectMany(s => s)
                .ToArray()
                .ArrayToString();
        }

        private void HandleInput()
        {
            if (!Input.anyKeyDown) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.DisableConsole();
                return;
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                GameManager.Instance.ToggleConsoleVisibility();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                HandleReturnInput();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                HandleBackspaceInput();
                return;
            }

            HandleTextInput();
        }

        private void HandleTextInput()
        {
            _currentLineBuffer += EscapeString(Input.inputString);
        }

        private void HandleBackspaceInput()
        {
            if (_currentLineBuffer.Length < 1) return;
            
            _currentLineBuffer = _currentLineBuffer.Remove(_currentLineBuffer.Length - 1);
        }

        private void HandleReturnInput()
        {
            var answer = _consoleCommandHandler.ExecuteCommand(_currentLineBuffer, this);

            if (!answer.Equals("0"))
            {
                _screenBuffer
                    .DeleteCharFromLastLine()
                    .AppendNewLineCharacter()
                    .AddLine(EscapeString(answer))
                    .AddEmptyLine();
            }
            
            _currentLineBuffer = "";
        }

        private string EscapeString(string s)
        {
            var escapedString = s;

            FontAsset.HasCharacters(s, out var missingChars);
            
            foreach (var missingChar in missingChars)
            {
                escapedString = escapedString.Replace(missingChar, ' ');
            }

            return escapedString;
        }
    }
}