using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Factory;
using UI.Commands.Product;
using UI.Stores;
using UI.ViewModels.Base;
using UI.ViewModels.Factory;

namespace UI.ViewModels.Product;

public class AddProductViewModel : ViewModelBaseWithValidation, ILoadingViewModel, INeedFactoriesViewModel
{
	public AddProductViewModel(
		int factoryId,
		NavigationStore navigationStore,
		ProductStore productStore,
		FactoryStore factoryStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		FactoryId = factoryId;
		_product = new ProductListItemViewModel(new Domain.Models.Product());
		_factories = new ObservableCollection<FactoryListItemViewModel>();
		_selectedFactory = new FactoryListItemViewModel(new Domain.Models.Factory());

		SaveCommand = new AddProductCommand(this, productStore, snackbarMessageQueue);
		CancelCommand = new NavigateBackCommand(navigationStore);

		LoadFactoriesCommand = new LoadFactoriesCommand(this, factoryStore);
	}

	public static AddProductViewModel LoadViewModel(
		object? factoryId,
		NavigationStore navigationStore,
		ProductStore productStore,
		FactoryStore factoryStore,
		ISnackbarMessageQueue snackbarMessageQueue)
	{
		int.TryParse(factoryId?.ToString(), out var id);

		var viewModel = new AddProductViewModel(
			id,
			navigationStore,
			productStore, 
			factoryStore,
			snackbarMessageQueue);
		viewModel.Load();
		return viewModel;
	}
	
	public async void Load()
	{
		await LoadFactoriesCommand.ExecuteAsync(null);
	}

	private ProductListItemViewModel _product;
	public ProductListItemViewModel Product
	{
		get => _product;
		set => SetField(ref _product, value);
	}

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}
	
	public int FactoryId { get; }

	public ICommand SaveCommand { get; }
	public ICommand CancelCommand { get; }

	public AsyncCommandBase LoadFactoriesCommand { get; }

	private readonly ObservableCollection<FactoryListItemViewModel> _factories;
	public IEnumerable<FactoryListItemViewModel> Factories => _factories;

	private FactoryListItemViewModel _selectedFactory;
	public FactoryListItemViewModel SelectedFactory
	{
		get => _selectedFactory;
		set => SetField(ref _selectedFactory, value);
	}

	public void UpdateFactories(IEnumerable<Domain.Models.Factory> factories)
	{
		factories = factories.OrderBy(x => x.Id);

		_factories.Clear();

		foreach (var factory in factories)
		{
			var productListItemViewModel = new FactoryListItemViewModel(factory);
			_factories.Add(productListItemViewModel);
		}

		var selectedFactory = Factories.FirstOrDefault(x => x.Id == FactoryId);
		SelectedFactory = new FactoryListItemViewModel(selectedFactory?.Factory ?? new Domain.Models.Factory());
		IsLoading = false;
	}
}