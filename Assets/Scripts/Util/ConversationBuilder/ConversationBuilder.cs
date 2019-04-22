using System.Collections.Generic;
using Behaviour;
using CreativeSpore.RPGConversationEditor;
using UnityEngine.Analytics;
using UnityEngine.Events;

namespace Util.ConversationBuilder
{
    public class ConversationBuilder
    {
        private ConversationData _conversation;

        private ConversationBuilder()
        {
            _conversation = new ConversationData();
            
            _conversation.dialogList = new List<Dialog>();
        }

        public static ConversationBuilder Create()
        {
            return new ConversationBuilder();
        }

        public ConversationBuilder AddDialogue(Dialog dialog)
        {
            //if at any point this does no longer work: just make the id prop setter public again
            dialog.id = _conversation.dialogList.Count;
            _conversation.dialogList.Add(dialog);

            return this;
        }
        
        public ConversationBuilder AddDialogueAsStartDialog(Dialog dialog)
        {
            AddDialogue(dialog);
            _conversation.StartDialog = dialog;

            return this;
        }

        public ConversationBuilder SetConversationName(string name)
        {
            _conversation.name = name;
            return this;
        }

        public ConversationData End()
        {
            return _conversation;
        }
    }

    public class DialogBuilder
    {
        private Dialog _dialog;

        private DialogBuilder()
        {
            _dialog = new Dialog();
            
            _dialog.onEnter = new UnityEvent();
            _dialog.onExit = new UnityEvent();
        }

        public static DialogBuilder Create()
        {
            return new DialogBuilder();
        }

        public Dialog End()
        {
            return _dialog;
        }

        public DialogBuilder SetFreezesPlayerMovement()
        {
            var ic = GameManager.Instance.Player.InputController;

            _dialog.onEnter.AddListener(ic.FreezePlayerMovement);
            _dialog.onExit.AddListener(ic.UnfreezePlayerMovement);

            return this;
        }
        
        public DialogBuilder SetSentence(int id, string sentence)
        {
            _dialog.SetSentence(id, sentence);

            return this;
        }
        
        public DialogBuilder AddSentence(string sentence)
        {
            _dialog.SetSentence(_dialog.sentences.Length, sentence);

            return this;
        }

        public DialogBuilder AddAction(Dialog.DialogAction action)
        {
            _dialog.dialogActions.Add(action);

            return this;
        }
    }

    public class DialogActionBuilder
    {
        private Dialog.DialogAction _action;

        private DialogActionBuilder()
        {
            _action = new Dialog.DialogAction();
        }

        public static DialogActionBuilder Create()
        {
            return new DialogActionBuilder();
        }

        public Dialog.DialogAction End()
        {
            return _action;
        }

        public DialogActionBuilder SetTargetDialog(Dialog dialog)
        {
            _action.targetDialogId = dialog.id;

            return this;
        }

        public DialogActionBuilder SetOnSubmitEvent(UnityAction action)
        {
            if(_action.onSubmit == null)
                _action.onSubmit = new UnityEvent();
            
            _action.onSubmit.AddListener(action);

            return this;
        }

        public DialogActionBuilder SetText(string text)
        {
            _action.name = text;

            return this;
        }
    }
}