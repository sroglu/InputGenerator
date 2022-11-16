using InputGenerator.DataContainer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Threading;
using InputGenerator.Utils;
using Xamarin.Forms.PlatformConfiguration;

namespace InputGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public enum TOUCH_MASK : uint
        {
            TOUCH_MASK_NONE = 0x00000000,
            TOUCH_MASK_CONTACTAREA = 0x00000001,
            TOUCH_MASK_ORIENTATION = 0x00000002,
            TOUCH_MASK_PRESSURE = 0x00000004
        }
        public enum POINTER_INPUT_TYPE : uint
        {
            PT_POINTER = 0x00000001,
            PT_TOUCH = 0x00000002,
            PT_PEN = 0x00000003,
            PT_MOUSE = 0x00000004
        }

        public enum POINTER_FLAGS : uint
        {
            POINTER_FLAG_NONE = 0x00000000,
            POINTER_FLAG_NEW = 0x00000001,
            POINTER_FLAG_INRANGE = 0x00000002,
            POINTER_FLAG_INCONTACT = 0x00000004,
            POINTER_FLAG_FIRSTBUTTON = 0x00000010,
            POINTER_FLAG_SECONDBUTTON = 0x00000020,
            POINTER_FLAG_THIRDBUTTON = 0x00000040,
            POINTER_FLAG_OTHERBUTTON = 0x00000080,
            POINTER_FLAG_PRIMARY = 0x00000100,
            POINTER_FLAG_CONFIDENCE = 0x00000200,
            POINTER_FLAG_CANCELLED = 0x00000400,
            POINTER_FLAG_DOWN = 0x00010000,
            POINTER_FLAG_UPDATE = 0x00020000,
            POINTER_FLAG_UP = 0x00040000,
            POINTER_FLAG_WHEEL = 0x00080000,
            POINTER_FLAG_HWHEEL = 0x00100000
        }
        public enum TOUCH_FEEDBACK : uint
        {
            TOUCH_FEEDBACK_DEFAULT = 0x1,
            TOUCH_FEEDBACK_INDIRECT = 0x2,
            TOUCH_FEEDBACK_NONE = 0x3
        }

        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool InjectTouch(int x, int y, POINTER_INPUT_TYPE pt_input, int pressure, int orientation, int id, int rcContactTop, int rcContactBottom, int rcContactLeft, int rcContactRight, POINTER_FLAGS pointerFlags, TOUCH_MASK touchMask);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setTouchFeedback(TOUCH_FEEDBACK fb);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultRectSize(int size);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultPressure(int pres);
        [DllImport("TouchInjectionDriver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setDefaultOrientation(int or);

        [DllImport("User32.dll")]
        static extern Boolean MessageBeep(UInt32 beepType);

        public static void GenerateTouch(int x, int y)
        {
            bool ret;
            setTouchFeedback(TOUCH_FEEDBACK.TOUCH_FEEDBACK_INDIRECT);
            ret = InjectTouch(x, y, POINTER_INPUT_TYPE.PT_TOUCH, 3200, 0, 0, x - 4, x + 4, y - 4, y + 4, POINTER_FLAGS.POINTER_FLAG_DOWN | POINTER_FLAGS.POINTER_FLAG_INCONTACT | POINTER_FLAGS.POINTER_FLAG_INRANGE, TOUCH_MASK.TOUCH_MASK_CONTACTAREA | TOUCH_MASK.TOUCH_MASK_ORIENTATION | TOUCH_MASK.TOUCH_MASK_PRESSURE);
            if (ret)
            {
                ret = InjectTouch(x, y, POINTER_INPUT_TYPE.PT_TOUCH, 3200, 0, 0, x - 4, x + 4, y - 4, y + 4, POINTER_FLAGS.POINTER_FLAG_UP, TOUCH_MASK.TOUCH_MASK_CONTACTAREA | TOUCH_MASK.TOUCH_MASK_ORIENTATION | TOUCH_MASK.TOUCH_MASK_PRESSURE);
            }
            else
            {
                MessageBeep(0);
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        #region Fields
        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        private const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        private const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        private const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        private const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */

        private SynchronizationContext context = null;
        #endregion

        CombinationData selectedCombination;

        Stopwatch countDownWatch;
        System.Timers.Timer tickCounter;
        static long selectedCounterTime;
        long counterTime;


        public MainWindow()
        {
            InitializeComponent();
            context = SynchronizationContext.Current;

            CombinationData.NameInputs();

            InputDetailPanel.Visibility = Visibility.Hidden;
            characterInputBox.Visibility = Visibility.Hidden;


            comboBox.ItemsSource = new BindingSource(CombinationData.CombinationDictionary, null);
            comboBox.DisplayMemberPath = "Value";
            comboBox.SelectionChanged += ComboBox_SelectionChanged;

            listBox.SelectionChanged += ListBox_SelectionChanged;

            countDownWatch = new Stopwatch();

            tickCounter = new System.Timers.Timer();
            tickCounter.Elapsed += new ElapsedEventHandler(OnTimerTick);
            tickCounter.Interval = 1;
            tickCounter.Enabled = false;

    }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(()=> {

                if (counterTime < 0)
                {
                    countDownLabel.Content = counterTime;
                    return;
                }

                if (counterTime < countDownWatch.ElapsedMilliseconds)
                {
                    countDownLabel.Content = 0;
                    tickCounter.Enabled = false;
                    countDownWatch.Stop();

                    OnTimeTick();

                    countDownWatch.Reset();

                    counterTime = selectedCounterTime;
                    countDownWatch.Start();
                    tickCounter.Enabled = true;
                }
                else
                    countDownLabel.Content = (counterTime - countDownWatch.ElapsedMilliseconds);

            });
        }

        Queue<CombinationData> actions = new Queue<CombinationData>();
        void OnTimeTick()
        {

            StringBuilder combinationCmdBuilder = new StringBuilder(100);
            actions.Clear();

            foreach (CombinationData combData in listBox.Items)
            {
                actions.Enqueue(combData);
            }

            while (actions.Count > 0)
            {
                Console.WriteLine(actions.Peek().command);
                if(actions.Peek().command== CombinationCommand.Execute)
                {
                    if (combinationCmdBuilder.Length>0)
                    {
                        SendKeys.SendWait(combinationCmdBuilder.ToString());
                        combinationCmdBuilder.Clear();
                    }
                    actions.Dequeue();
                }
                else if (actions.Peek().command == CombinationCommand.LeftClick ||
                    actions.Peek().command == CombinationCommand.RightClick||
                    actions.Peek().command == CombinationCommand.DoubleClick)
                {

                    if (combinationCmdBuilder.Length > 0)
                    {
                        Console.WriteLine(combinationCmdBuilder.ToString());

                        SendKeys.SendWait(combinationCmdBuilder.ToString());
                        combinationCmdBuilder.Clear();
                    }


                    OperateClicks(actions.Dequeue(), System.Drawing.Point.Empty);

                }
                else
                {
                    combinationCmdBuilder.Append(actions.Dequeue().SendKeyValues());
                }
            }

            if (combinationCmdBuilder.Length > 0)
            {
                SendKeys.SendWait(combinationCmdBuilder.ToString());
                combinationCmdBuilder.Clear();
            }

            TouchController.Touch(200, 200);

            //GenerateTouch(200, 200);

            //foreach (CombinationData combData in listBox.Items)
            //{
            //    combinationCmdBuilder.Append(combData.SendKeyValues());
            //}

            //SendKeys.SendWait(combinationCmdBuilder.ToString());

        }

        void OperateClicks(CombinationData combData, System.Drawing.Point p)
        {
            if(p!= System.Drawing.Point.Empty)
                SetCursorPos(p.X, p.Y);
            Thread.Sleep(100);
            if (combData.command == CombinationCommand.LeftClick)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            else if (combData.command == CombinationCommand.DoubleClick)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(100);
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            else if (combData.command == CombinationCommand.LeftClick)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            }
            else
            {
                //exception
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.Items.Count == 0) {
                comboBox.SelectedItem = null;
                return;
            }

            selectedCombination = (CombinationData)listBox.SelectedItem;
            if (selectedCombination == null) return;

            if ((CombinationCommand)comboBox.SelectedIndex == CombinationCommand.CharacterInput) characterInputBox.Text = selectedCombination.character;

            if (InputDetailPanel.Visibility != Visibility.Visible)
                InputDetailPanel.Visibility = Visibility.Visible;

            UpdateItemDetailView();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedCombination.command = (CombinationCommand)comboBox.SelectedIndex;

            listBox.Items.Refresh();
            listBox.UpdateLayout();

            UpdateItemDetailView();
        }


        void UpdateItemDetailView()
        {
            if (selectedCombination.command < 0)
                selectedCombination.command = CombinationCommand.COUNT;

            if (selectedCombination.command == CombinationCommand.COUNT)
                comboBox.SelectedItem = null;
            else
                comboBox.SelectedItem = new KeyValuePair<CombinationCommand, string>(selectedCombination.command, CombinationData.CombinationDictionary[selectedCombination.command]);
                //comboBox.SelectedItem = new KeyValuePair<CombinationCommand, string>(selectedCombination.command, selectedCombination.ToString());

            comboBox.UpdateLayout();

            characterInputBox.Visibility = (((CombinationCommand)comboBox.SelectedIndex) == CombinationCommand.CharacterInput) ? Visibility.Visible : Visibility.Hidden;
            if (characterInputBox.Visibility == Visibility.Visible)
                characterInputBox.Text = selectedCombination.character;
        }


        //Add Command
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            CombinationData combination = new CombinationData();
            listBox.Items.Add(combination);
            combination.index = listBox.Items.IndexOf(combination);
            listBox.SelectedIndex = listBox.Items.Count - 1;
        }

        private void CharacterInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((CombinationCommand)comboBox.SelectedIndex == CombinationCommand.CharacterInput) selectedCombination.character = characterInputBox.Text;
            UpdateItemDetailView();
        }
        static int lastSelectedIndex = -1;
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            lastSelectedIndex = listBox.Items.IndexOf(selectedCombination);

            if (listBox.Items.Count > 1)
            {
                if (listBox.Items.Count > lastSelectedIndex && listBox.Items[lastSelectedIndex] != null && lastSelectedIndex > 0)
                    listBox.SelectedItem = listBox.Items.GetItemAt(lastSelectedIndex - 1);
                else
                    listBox.SelectedItem = listBox.Items.GetItemAt(lastSelectedIndex + 1);
            }
            
            listBox.Items.Remove(listBox.Items.GetItemAt(lastSelectedIndex));
        }

        private void SetTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (timeText.Text == "") return;
            int time;
            int.TryParse(timeText.Text,out time);
            selectedCounterTime = time;
            counterTime = selectedCounterTime;

            if (counterTime < 0) return;
            countDownWatch.Start();
            tickCounter.Enabled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void StopTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            timeText.Text = (-1).ToString();
            SetTimerBtn_Click(sender, e);
        }

        private void OnApplicationExit(object sender, CancelEventArgs e)
        {
            comboBox.SelectionChanged -= ComboBox_SelectionChanged;

            listBox.SelectionChanged -= ListBox_SelectionChanged;

            countDownWatch.Stop();

            tickCounter.Elapsed -= new ElapsedEventHandler(OnTimerTick);

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = true;
            }
        }
    }
}
