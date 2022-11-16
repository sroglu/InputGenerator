using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using InputGenerator.Utils;

namespace InputGenerator.DataContainer
{

    public enum CombinationCommand
    {
        [Description("")]
        CharacterInput,
        [Description("Execute")]
        Execute,
        [Description("LeftClick")]
        LeftClick,
        [Description("DoubleClick")]
        DoubleClick,
        [Description("RightClick")]
        RightClick,
        [Description("%")]
        ALT,
        [Description("^")]
        CTRL,
        [Description("+")]
        SHIFT,
        [Description("{BACKSPACE}")]
        BACKSPACE,
        [Description("{BREAK}")]
        BREAK,
        [Description("{CAPSLOCK}")]
        CAPS_LOCK,
        [Description("{DELETE}")]
        DELETE,
        [Description("{DOWN}")]
        DOWN_ARROW,
        [Description("{END}")]
        END,
        [Description("{ENTER}")]
        ENTER,
        [Description("{ESC}")]
        ESC,
        [Description("{HELP}")]
        HELP,
        [Description("{HOME}")]
        HOME,
        [Description("{INSERT}")]
        INSERT,
        [Description("{LEFT}")]
        LEFT_ARROW,
        [Description("{LEFTNUMLOCK}")]
        NUM_LOCK,
        [Description("{PGDN}")]
        PAGE_DOWN,
        [Description("{PGUP}")]
        PAGE_UP,
        [Description("{PRTSC}")]
        PRINT_SCREEN,
        [Description("{RIGHT}")]
        RIGHT_ARROW,
        [Description("{SCROLLOCK}")]
        SCROLL_LOCK,
        [Description("{TAB}")]
        TAB,
        [Description("{UP}")]
        UP_ARROW,
        [Description("{F1}")]
        F1,
        [Description("{F2}")]
        F2,
        [Description("{F3}")]
        F3,
        [Description("{F4}")]
        F4,
        [Description("{F5}")]
        F5,
        [Description("{F6}")]
        F6,
        [Description("{F7}")]
        F7,
        [Description("{F8}")]
        F8,
        [Description("{F9}")]
        F9,
        [Description("{F10}")]
        F10,
        [Description("{F11}")]
        F11,
        [Description("{F12}")]
        F12,
        [Description("{F13}")]
        F13,
        [Description("{F14}")]
        F14,
        [Description("{F15}")]
        F15,
        [Description("{F16}")]
        F16,
        [Description("{ADD}")]
        Keypad_Add,
        [Description("{SUBTRACT}")]
        Keypad_Subtract,
        [Description("{MULTIPLY}")]
        Keypad_Multiply,
        [Description("{DEVIDE}")]
        Keypad_Divide,
        [Description("")]
        COUNT
    }

    public class CombinationData
    {
        public static Dictionary<CombinationCommand, string> CombinationDictionary;
        public static void NameInputs()
        {
            CombinationDictionary = new Dictionary<CombinationCommand, string>();
            CombinationDictionary.Add(CombinationCommand.CharacterInput, "CharacterInput");
            CombinationDictionary.Add(CombinationCommand.Execute, "Execute");
            CombinationDictionary.Add(CombinationCommand.LeftClick, "LeftClick");
            CombinationDictionary.Add(CombinationCommand.RightClick, "RightClick");
            CombinationDictionary.Add(CombinationCommand.DoubleClick, "DoubleClick");
            CombinationDictionary.Add(CombinationCommand.ALT, "ALT");
            CombinationDictionary.Add(CombinationCommand.CTRL, "CTRL");
            CombinationDictionary.Add(CombinationCommand.SHIFT, "SHIFT");
            CombinationDictionary.Add(CombinationCommand.BACKSPACE, "BACKSPACE");
            CombinationDictionary.Add(CombinationCommand.BREAK, "BREAK");
            CombinationDictionary.Add(CombinationCommand.CAPS_LOCK, "CAPS LOCK");
            CombinationDictionary.Add(CombinationCommand.DELETE, "DELETE");
            CombinationDictionary.Add(CombinationCommand.DOWN_ARROW, "DOWN ARROW");
            CombinationDictionary.Add(CombinationCommand.END, "END");
            CombinationDictionary.Add(CombinationCommand.ENTER, "ENTER");
            CombinationDictionary.Add(CombinationCommand.ESC, "ESC");
            CombinationDictionary.Add(CombinationCommand.HELP, "HELP");
            CombinationDictionary.Add(CombinationCommand.HOME, "HOME");
            CombinationDictionary.Add(CombinationCommand.INSERT, "INSERT");
            CombinationDictionary.Add(CombinationCommand.LEFT_ARROW, "LEFT_ARROW");
            CombinationDictionary.Add(CombinationCommand.NUM_LOCK, "NUM LOCK");
            CombinationDictionary.Add(CombinationCommand.PAGE_DOWN, "PAGE DOWN");
            CombinationDictionary.Add(CombinationCommand.PAGE_UP, "PAGE UP");
            CombinationDictionary.Add(CombinationCommand.PRINT_SCREEN, "PRINT SCREEN");
            CombinationDictionary.Add(CombinationCommand.RIGHT_ARROW, "RIGHT ARROW");
            CombinationDictionary.Add(CombinationCommand.SCROLL_LOCK, "SCROLL LOCK");
            CombinationDictionary.Add(CombinationCommand.TAB, "TAB");
            CombinationDictionary.Add(CombinationCommand.UP_ARROW, "UP ARROW");
            CombinationDictionary.Add(CombinationCommand.F1, "F1");
            CombinationDictionary.Add(CombinationCommand.F2, "F2");
            CombinationDictionary.Add(CombinationCommand.F3, "F3");
            CombinationDictionary.Add(CombinationCommand.F4, "F4");
            CombinationDictionary.Add(CombinationCommand.F5, "F5");
            CombinationDictionary.Add(CombinationCommand.F6, "F6");
            CombinationDictionary.Add(CombinationCommand.F7, "F7");
            CombinationDictionary.Add(CombinationCommand.F8, "F8");
            CombinationDictionary.Add(CombinationCommand.F9, "F9");
            CombinationDictionary.Add(CombinationCommand.F10, "F10");
            CombinationDictionary.Add(CombinationCommand.F11, "F11");
            CombinationDictionary.Add(CombinationCommand.F12, "F12");
            CombinationDictionary.Add(CombinationCommand.F13, "F13");
            CombinationDictionary.Add(CombinationCommand.F14, "F14");
            CombinationDictionary.Add(CombinationCommand.F15, "F15");
            CombinationDictionary.Add(CombinationCommand.F16, "F16");
            CombinationDictionary.Add(CombinationCommand.Keypad_Add, "Keypad Add");
            CombinationDictionary.Add(CombinationCommand.Keypad_Subtract, "Keypad Subtract");
            CombinationDictionary.Add(CombinationCommand.Keypad_Multiply, "Keypad Multiply");
            CombinationDictionary.Add(CombinationCommand.Keypad_Divide, "Keypad Divide");

        }

        public int index;
        public CombinationCommand command;
        public string character;

        public string Name
        {
            get
            {
                if ((int)command == -1) command = CombinationCommand.COUNT;
                if (command == CombinationCommand.COUNT) return "<Empty-Command>";
                if (command == CombinationCommand.CharacterInput) return character;
                return CombinationDictionary[command];
            }
        }

        public CombinationData()
        {
            index = -1;
            command = CombinationCommand.COUNT;
        }

        public override string ToString()
        {
            return Name;
        }

        public string SendKeyValues()
        {
            if (command == CombinationCommand.COUNT) return null;
            if (command == CombinationCommand.CharacterInput) return character.ToLower();
            return EnumUtils.GetDescription(command);
        }





    }

}