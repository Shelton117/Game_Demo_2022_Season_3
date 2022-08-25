using System.Collections;
using System.Collections.Generic;
using Scripts.Utilities;
using UnityEngine;
using _2022_Season_3.New_Folder.Scripts.command_mode;
using EventHandler = _2022_Season_3.New_Folder.Scripts.Utilities.EventHandler;

namespace _2022_Season_3.New_Folder.Scripts.Manager
{
    public class CommandManager : Singleton<CommandManager>
    {
        private readonly List<Command> mCommands = new List<Command>();
        private readonly List<Command> mReadyCommands = new List<Command>();

        private void OnEnable()
        {
            EventHandler.Ready2Play += OnReady2Play;
        }

        private void OnDisable()
        {
            EventHandler.Ready2Play -= OnReady2Play;
        }

        private void OnReady2Play()
        {
            var commands = GameManager.Instance.ids;
            foreach (var command in commands)
            {
                var readyCommand = InputHandler.SetCommand(command);
                mReadyCommands.Add(readyCommand);
            }

            Debug.Log("OnReady2Play");
        }

        void Start()
        {
            gameObject.AddComponent<InputHandler>();
        }

        public void AddCommands(Command command)
        {
            mCommands.Add(command);
        }

        public IEnumerator StartPlay()
        {
            foreach (var command in mReadyCommands)
            {
                yield return new WaitForSeconds(0.2f);
                command.Execute();
            }

            Debug.Log("StartPlay");
        }

        public IEnumerator UndoStart()
        {
            mCommands.Reverse();

            foreach (var command in mCommands)
            {
                yield return new WaitForSeconds(0.2f);
                command.Undo();
            }

            mCommands.Clear();
        }
    }
}