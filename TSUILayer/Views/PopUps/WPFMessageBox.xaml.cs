using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.InteropServices;
//using Microsoft.Practices.Prism.Commands;
using System.Windows.Interop;

namespace TSUILayer.Views.PopUps
{
    /// <summary>
    /// Interaction logic for WPFMessageBox.xaml
    /// </summary>
    public partial class WPFMessageBox : UserControl
    {
        public WPFMessageBox()
        {
            InitializeComponent();
        }

        public WPFMessageBoxResult Result { get; set; }

        public static WPFMessageBoxResult Show(string message)
        {
            return Show(string.Empty, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption)
        {
            return Show(title, message, string.Empty, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption)
        {
            return Show(title, message, details, buttonOption, WPFMessageBoxImage.Default);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxImage image)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, image);
        }

        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxImage image)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, image);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
            return Show(title, message, string.Empty, buttonOption, image);
        }


        delegate WPFMessageBoxResult messageBoxDel(string title, string message, string details, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image);
        private static WPFMessageBoxResult displayMessageBox(string title, string message, string details, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
            try
            {
                MessageBoxWindow wnd = new MessageBoxWindow();
                if (title == string.Empty)
                {
                    title = "TRACTOR SHOWROOM";
                }
                else if (title.ToUpper().Trim() == "TRACTORSHOWROOM")
                {
                    title = "TRACTOR SHOWROOM";
                }
                wnd.Title = title;
                //  wnd.SizeToContent = SizeToContent.WidthAndHeight;
                bool setWidth = DisplayErrorMessageInCustomWindow(message);
                if (setWidth == false)
                {
                    wnd.SizeToContent = SizeToContent.WidthAndHeight;
                }
                else
                {
                    wnd.Width = 500;
                    wnd.SizeToContent = SizeToContent.Height;

                }

                wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                wnd.ResizeMode = ResizeMode.NoResize;
                ___MessageBox = new WPFMessageBox();
                MessageBoxViewModel __ViewModel = new MessageBoxViewModel(___MessageBox, title, message, details, buttonOption, image);
                ___MessageBox.DataContext = __ViewModel;
                wnd.Content = ___MessageBox;
                wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                wnd.ShowDialog();             
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ___MessageBox.Result;
        }
        private static bool DisplayErrorMessageInCustomWindow(string errorMessage)
        {
            bool DisplayCustom = false;
            if (errorMessage != null)
            {
                if (errorMessage.Contains("\n"))
                {
                    string[] arr = errorMessage.Split('\n');

                    if (arr != null)
                    {
                        for (int index = 0; index < arr.Length; index++)
                        {
                            if (arr[index].Length > 75)
                            {
                                DisplayCustom = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (errorMessage.Length > 75)
                    {
                        DisplayCustom = true;
                    }
                }
            }
            return DisplayCustom;
        }


        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {

            object result = System.Windows.Application.Current.Dispatcher.Invoke(new messageBoxDel(displayMessageBox), new object[] { title, message, details, buttonOption, image });

            return (WPFMessageBoxResult)result;
        }

        [ThreadStatic]
        static WPFMessageBox ___MessageBox;

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    Window wnd = this.Parent as Window;
        //    IconHelper.RemoveIcon(wnd);
        //}

    }


    public class MessageBoxViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get { return ___Title; }
            set
            {
                if (___Title != value)
                {
                    ___Title = value;
                    NotifyPropertyChange("Title");
                }
            }
        }

        public string Message
        {
            get { return ___Message; }
            set
            {
                if (___Message != value)
                {
                    ___Message = value;
                    NotifyPropertyChange("Message");
                }
            }
        }

        public string InnerMessageDetails
        {
            get { return ___InnerMessageDetails; }
            set
            {
                if (___InnerMessageDetails != value)
                {
                    ___InnerMessageDetails = value;
                    NotifyPropertyChange("InnerMessageDetails");
                }
            }
        }

        public ImageSource MessageImageSource
        {
            get { return ___MessageImageSource; }
            set
            {
                ___MessageImageSource = value;
                NotifyPropertyChange("MessageImageSource");
            }
        }

        public Visibility YesNoVisibility
        {
            get { return ___YesNoVisibility; }
            set
            {
                if (___YesNoVisibility != value)
                {
                    ___YesNoVisibility = value;
                    NotifyPropertyChange("YesNoVisibility");
                }
            }
        }

        public Visibility CancelVisibility
        {
            get { return ___CancelVisibility; }
            set
            {
                if (___CancelVisibility != value)
                {
                    ___CancelVisibility = value;
                    NotifyPropertyChange("CancelVisibility");
                }
            }
        }

        public Visibility OkVisibility
        {
            get { return ___OKVisibility; }
            set
            {
                if (___OKVisibility != value)
                {
                    ___OKVisibility = value;
                    NotifyPropertyChange("OkVisibility");
                }
            }
        }

        public Visibility CloseVisibility
        {
            get { return ___CloseVisibility; }
            set
            {
                if (___CloseVisibility != value)
                {
                    ___CloseVisibility = value;
                    NotifyPropertyChange("CloseVisibility");
                }
            }
        }

        public Visibility ShowDetails
        {
            get { return ___ShowDetails; }
            set
            {
                if (___ShowDetails != value)
                {
                    ___ShowDetails = value;
                    NotifyPropertyChange("ShowDetails");
                }
            }
        }

        public ICommand YesCommand
        {
            get
            {
                if (___YesCommand == null)
                    ___YesCommand = new DelegateCommand(() =>
                    {
                        ___View.Result = WPFMessageBoxResult.Yes;

                        Window wnd = ___View.Parent as Window;
                        wnd.Close();
                    });
                return ___YesCommand;
            }
        }

        public ICommand NoCommand
        {
            get
            {
                if (___NoCommand == null)
                    ___NoCommand = new DelegateCommand(() =>
                    {
                        ___View.Result = WPFMessageBoxResult.No;
                        Window wnd = ___View.Parent as Window;
                        wnd.Close();
                    });
                return ___NoCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (___CancelCommand == null)
                    ___CancelCommand = new DelegateCommand(() =>
                    {
                        ___View.Result = WPFMessageBoxResult.Cancel;
                        Window wnd = ___View.Parent as Window;
                        wnd.Close();
                    });
                return ___CancelCommand;
            }
        }


        public ICommand CloseCommand
        {
            get
            {
                if (___CloseCommand == null)
                    ___CloseCommand = new DelegateCommand(() =>
                    {
                        ___View.Result = WPFMessageBoxResult.Close;
                        Window wnd = ___View.Parent as Window;
                        wnd.Close();
                    });
                return ___CloseCommand;
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if (___OKCommand == null)
                    ___OKCommand = new DelegateCommand(() =>
                    {
                        ___View.Result = WPFMessageBoxResult.Ok;
                        Window wnd = ___View.Parent as Window;
                        wnd.Close();
                    });
                return ___OKCommand;
            }
        }

        public MessageBoxViewModel(WPFMessageBox view,
            string title, string message, string innerMessage,
            WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
            Title = title;
            Message = message;
            InnerMessageDetails = innerMessage;
            SetButtonVisibility(buttonOption);
            SetImageSource(image);
            ___View = view;
        }

        void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        void SetButtonVisibility(WPFMessageBoxButtons buttonOption)
        {
            switch (buttonOption)
            {
                case WPFMessageBoxButtons.YesNo:
                    OkVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.YesNoCancel:
                    OkVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.OK:
                    YesNoVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.OKClose:
                    YesNoVisibility = CancelVisibility = Visibility.Collapsed;
                    break;
                default:
                    OkVisibility = CancelVisibility = YesNoVisibility = Visibility.Collapsed;
                    break;
            }
            if (string.IsNullOrEmpty(InnerMessageDetails)) ShowDetails = Visibility.Collapsed;
            else ShowDetails = Visibility.Visible;
        }

        void SetImageSource(WPFMessageBoxImage image)
        {
            string __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Default.png");
            switch (image)
            {
                case WPFMessageBoxImage.Alert:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Alert.png");
                    break;
                case WPFMessageBoxImage.Error:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Error.png");
                    break;
                case WPFMessageBoxImage.Information:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Info.png");
                    break;
                case WPFMessageBoxImage.OK:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\OK.png");
                    break;
                case WPFMessageBoxImage.Question:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Help.png");
                    break;
                default:
                    __Source = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "images\\Default.png");
                    break;

            }
            Uri __ImageUri = new Uri(__Source, UriKind.RelativeOrAbsolute);
            MessageImageSource = new BitmapImage(__ImageUri);
        }

        string ___Title;
        string ___Message;
        string ___InnerMessageDetails;

        Visibility ___YesNoVisibility;
        Visibility ___CancelVisibility;
        Visibility ___OKVisibility;
        Visibility ___CloseVisibility;
        Visibility ___ShowDetails;

        ICommand ___YesCommand;
        ICommand ___NoCommand;
        ICommand ___CancelCommand;
        ICommand ___CloseCommand;
        ICommand ___OKCommand;

        WPFMessageBox ___View;
        ImageSource ___MessageImageSource;
    }


    public static class IconHelper
    {
        //[DllImport("user32.dll")]
        //static extern int GetWindowLong(IntPtr hwnd, int index);

        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                   int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hwnd, uint msg,
                   IntPtr wParam, IntPtr lParam);

        const int GWL_EXSTYLE = -20;
        const int WS_EX_DLGMODALFRAME = 0x0001;
        const int SWP_NOSIZE = 0x0001;
        const int SWP_NOMOVE = 0x0002;
        const int SWP_NOZORDER = 0x0004;
        const int SWP_FRAMECHANGED = 0x0020;
        const uint WM_SETICON = 0x0080;

        public static void RemoveIcon(Window window)
        {
            // Get this window's handle
            ///IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            //int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            //SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

            // Update the window's non-client area to reflect the changes
            //SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
            //      SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            // base.OnSourceInitialized(e);
        }

    }

    public enum WPFMessageBoxButtons
    {
        YesNo,
        YesNoCancel,
        OKCancel,
        OKClose,
        OK,
        Close
    }

    public enum WPFMessageBoxImage
    {
        Information,
        Question,
        Error,
        OK,
        Alert,
        Default
    }

    public enum WPFMessageBoxResult
    {
        Yes,
        No,
        Ok,
        Cancel,
        Close
    }


    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {

        private readonly Action m_ExecuteMethod = null;
        private readonly Func<bool> m_CanExecuteMethod = null;
        private bool m_IsAutomaticRequeryDisabled = false;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod");

            m_ExecuteMethod = executeMethod;
            m_CanExecuteMethod = canExecuteMethod;
            m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute()
        {
            if (m_CanExecuteMethod != null) return m_CanExecuteMethod();
            return true;
        }

        /// <summary>
        ///  Execution of the command
        /// </summary>
        public void Execute()
        {
            if (m_ExecuteMethod != null) m_ExecuteMethod();
        }

        /// <summary>
        ///  Property to enable or disable CommandManager's automatic requery on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get { return m_IsAutomaticRequeryDisabled; }
            set
            {
                if (m_IsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(m_CanExecuteChangedHandlers);
                    else CommandManagerHelper.AddHandlersToRequerySuggested(m_CanExecuteChangedHandlers);
                    m_IsAutomaticRequeryDisabled = value;
                }
            }
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(m_CanExecuteChangedHandlers);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value;
                CommandManagerHelper.AddWeakReferenceHandler(ref m_CanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value;
                CommandManagerHelper.RemoveWeakReferenceHandler(m_CanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            Execute();
        }
    }

    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
    public class DelegateCommand<T> : ICommand
    {

        private readonly Action<T> m_ExecuteMethod = null;
        private readonly Func<T, bool> m_CanExecuteMethod = null;
        private bool m_IsAutomaticRequeryDisabled = false;
        private List<WeakReference> m_CanExecuteChangedHandlers;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false) { }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false) { }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null) throw new ArgumentNullException("executeMethod");

            m_ExecuteMethod = executeMethod;
            m_CanExecuteMethod = canExecuteMethod;
            m_IsAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        public bool CanExecute(T parameter)
        {
            if (m_CanExecuteMethod != null) return m_CanExecuteMethod(parameter);
            return true;
        }

        public void Execute(T parameter)
        {
            if (m_ExecuteMethod != null) m_ExecuteMethod(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CommandManagerHelper.CallWeakReferenceHandlers(m_CanExecuteChangedHandlers);
        }

        public bool IsAutomaticRequeryDisabled
        {
            get { return m_IsAutomaticRequeryDisabled; }
            set
            {
                if (m_IsAutomaticRequeryDisabled != value)
                {
                    if (value) CommandManagerHelper.RemoveHandlersFromRequerySuggested(m_CanExecuteChangedHandlers);
                    else CommandManagerHelper.AddHandlersToRequerySuggested(m_CanExecuteChangedHandlers);
                    m_IsAutomaticRequeryDisabled = value;
                }
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested += value;
                CommandManagerHelper.AddWeakReferenceHandler(ref m_CanExecuteChangedHandlers, value, 2);
            }
            remove
            {
                if (!m_IsAutomaticRequeryDisabled) CommandManager.RequerySuggested -= value;
                CommandManagerHelper.RemoveWeakReferenceHandler(m_CanExecuteChangedHandlers, value);
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true
            if (parameter == null && typeof(T).IsValueType) return (m_CanExecuteMethod == null);
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
    }

    //commands

    /// <summary>
    ///     This class contains methods for the CommandManager that help avoid memory leaks by
    ///     using weak references.
    /// </summary>
    internal class CommandManagerHelper
    {
        internal static Action<List<WeakReference>> CallWeakReferenceHandlers = x =>
        {
            if (x != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.

                var __Callers = new EventHandler[x.Count];
                int __Count = 0;

                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var __Reference = x[i];
                    var __Handler = __Reference.Target as EventHandler;
                    if (__Handler == null)
                    {
                        // Clean up old handlers that have been collected
                        x.RemoveAt(i);
                    }
                    else
                    {
                        __Callers[__Count] = __Handler;
                        __Count++;
                    }
                }

                // Call the handlers that we snapshotted
                for (int i = 0; i < __Count; i++)
                {
                    var __Handler = __Callers[i];
                    __Handler(null, EventArgs.Empty);
                }
            }
        };

        internal static Action<List<WeakReference>> AddHandlersToRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var __Handler = y.Target as EventHandler;
                    if (__Handler != null) CommandManager.RequerySuggested += __Handler;
                });
            }
        };

        internal static Action<List<WeakReference>> RemoveHandlersFromRequerySuggested = x =>
        {
            if (x != null)
            {
                x.ForEach(y =>
                {
                    var __Handler = y.Target as EventHandler;
                    if (__Handler != null) CommandManager.RequerySuggested -= __Handler;
                });
            }
        };

        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler)
        {
            AddWeakReferenceHandler(ref handlers, handler, -1);
        }

        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        internal static Action<List<WeakReference>, EventHandler> RemoveWeakReferenceHandler = (x, y) =>
        {
            if (x != null)
            {
                for (int i = x.Count - 1; i >= 0; i--)
                {
                    var __Reference = x[i];
                    var __ExistingHandler = __Reference.Target as EventHandler;
                    if ((__ExistingHandler == null) || (__ExistingHandler == y))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        x.RemoveAt(i);
                    }
                }
            }
        };
    }

    /// <summary>
    /// This class facilitates associating a key binding in XAML markup to a command
    /// defined in a View Model by exposing a Command dependency property.
    /// The class derives from Freezable to work around a limitation in WPF when data-binding from XAML.
    /// </summary>
    public class CommandReference : Freezable, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
        (
            "Command",
            typeof(ICommand),
            typeof(CommandReference),
            new PropertyMetadata(new PropertyChangedCallback((x, y) =>
            {
                var commandReference = x as CommandReference;
                var oldCommand = y.OldValue as ICommand;
                var newCommand = y.NewValue as ICommand;

                if (oldCommand != null) oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
                if (newCommand != null) newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }))
         );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool CanExecute(object parameter)
        {
            if (Command != null) return Command.CanExecute(parameter);
            return false;
        }

        public void Execute(object parameter)
        {
            Command.Execute(parameter);
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

    }

    public class MessageBoxWindow : Window
    {

        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            base.OnSourceInitialized(e);
        }
    }
}
