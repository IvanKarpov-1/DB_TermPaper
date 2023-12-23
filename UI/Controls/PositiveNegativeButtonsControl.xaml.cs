using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Controls
{
	/// <summary>
	/// Interaction logic for SaveCancelButtonsControl.xaml
	/// </summary>
	public partial class PositiveNegativeButtonsControl : UserControl
    {
        public PositiveNegativeButtonsControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ContentOfPositiveButtonProperty =
			DependencyProperty.Register(nameof(ContentOfPositiveButton), typeof(string), typeof(PositiveNegativeButtonsControl));

	    public string ContentOfPositiveButton
	    {
		    get => (string)GetValue(ContentOfPositiveButtonProperty);
			set => SetValue(ContentOfPositiveButtonProperty, value);
		}

	    public static readonly DependencyProperty ContentOfNegativeButtonProperty =
		    DependencyProperty.Register(nameof(ContentOfNegativeButton), typeof(string), typeof(PositiveNegativeButtonsControl));

	    public string ContentOfNegativeButton
	    {
		    get => (string)GetValue(ContentOfNegativeButtonProperty);
		    set => SetValue(ContentOfNegativeButtonProperty, value);
	    }

		public static readonly DependencyProperty HasErrorsProperty =
	        DependencyProperty.Register(nameof(HasErrors), typeof(bool), typeof(PositiveNegativeButtonsControl), new PropertyMetadata(false));

        public bool HasErrors
		{
	        get => (bool)GetValue(HasErrorsProperty);
	        set => SetValue(HasErrorsProperty, value);
        }

        public static readonly DependencyProperty CommandOfPositiveButtonProperty =
	        DependencyProperty.Register(nameof(CommandOfPositiveButton), typeof(ICommand), typeof(PositiveNegativeButtonsControl));

        public ICommand CommandOfPositiveButton
        {
	        get => (ICommand)GetValue(CommandOfPositiveButtonProperty);
            set => SetValue(CommandOfPositiveButtonProperty, value);
        }

        public static readonly DependencyProperty CommandOfNegativeButtonProperty =
	        DependencyProperty.Register(nameof(CommandOfNegativeButton), typeof(ICommand), typeof(PositiveNegativeButtonsControl));

        public ICommand CommandOfNegativeButton
        {
	        get => (ICommand)GetValue(CommandOfNegativeButtonProperty);
	        set => SetValue(CommandOfNegativeButtonProperty, value);
        }
	}
}
