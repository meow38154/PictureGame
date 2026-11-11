namespace UI.DialogueSystem
{
    public interface IDialogueTextWriter
    {
        public bool IsWriting { get; }
        void WriteLine(DialogueLineData lineData);
        void SkipLine();
    }
}