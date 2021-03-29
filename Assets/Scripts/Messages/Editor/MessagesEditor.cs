using UnityEditor;

namespace SBabchuk.Databases
{
    [CustomEditor(typeof(MessagesDatabase))]
    public class MessagesEditor : BaseDatabaseEditor
    {
        public override void Draw()
        {
            MessagesDrawer.Draw((MessagesDatabase)database, selectedMode);
        }
    }
}