using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rota_Creator_App
{
    delegate void Undo();
    delegate void Redo();

    public class UndoRedo
    {
        protected static Stack<Undo> UndoCommands = new Stack<IUndoRedoObject>();
        protected static Stack<Redo> RedoCommands = new Stack<IUndoRedoObject>();

        public static void Add()
        {
            UndoCommands.Push(commandObject);
        }

        public static bool Undo()
        {
            if (UndoCommands.Count != 0)
            {
                IUndoRedoObject command = UndoCommands.Pop();
                command.Undo();
                RedoCommands.Push(command);
            }
        }
        public static bool Redo()
        {
            if (RedoCommands.Count != 0)
            {
                IUndoRedoObject command = RedoCommands.Pop();
                command.Redo();
                UndoCommands.Push(command);
            }
        }
    }
}