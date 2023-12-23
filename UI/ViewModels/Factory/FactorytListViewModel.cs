using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UI.Commands;
using UI.Commands.Base;
using UI.Commands.Factory;
using UI.Services;
using UI.Stores;
using UI.ViewModels.Base;
using UI.ViewModels.Product;

namespace UI.ViewModels.Factory;

public class FactoryListViewModel : ViewModelBase, ILoadingViewModel, INeedFactoriesViewModel
{

	public FactoryListViewModel(
		FactoryStore factoryStore,
		NavigationService<FactoryDetailsViewModel> factoryDetailsViewNavigationService,
		NavigationService<AddFactoryViewModel> addFactoryViewNavigationService)
	{
		_factories = new ObservableCollection<FactoryListItemViewModel>();

		LoadFactoriesCommand = new LoadFactoriesCommand(this, factoryStore);
		NavigateToFactoryDetailsCommand = new NavigateCommand<FactoryDetailsViewModel>(factoryDetailsViewNavigationService);
		NavigateToAddFactoryCommand = new NavigateCommand<AddFactoryViewModel>(addFactoryViewNavigationService);
	}

	public static FactoryListViewModel LoadViewModel(
		FactoryStore factory,
		NavigationService<FactoryDetailsViewModel> factoryDetailsViewNavigationService,
		NavigationService<AddFactoryViewModel> addFactoryViewNavigationService)
	{
		var viewModel = new FactoryListViewModel(
			factory,
			factoryDetailsViewNavigationService,
			addFactoryViewNavigationService);
		viewModel.Load();
		return viewModel;
	}

	public async void Load()
	{
		await LoadFactoriesCommand.ExecuteAsync(null);
	}

	private readonly ObservableCollection<FactoryListItemViewModel> _factories;
	public IEnumerable<FactoryListItemViewModel> Factories => _factories;

	private bool _isLoading = true;
	public bool IsLoading
	{
		get => _isLoading;
		set => SetField(ref _isLoading, value);
	}

	public AsyncCommandBase LoadFactoriesCommand { get; }
	public ICommand NavigateToFactoryDetailsCommand { get; }
	public ICommand NavigateToAddFactoryCommand { get; }

	public bool? IsAllItemsSelected
	{
		get
		{
			if (_factories.Count == 0) return false;
			var selected = _factories.Select(x => x.IsSelected).Distinct().ToList();
			return selected.Count == 1 ? selected.Single() : null;
		}
		set
		{
			if (!value.HasValue) return;
			SelectAll(value.Value, _factories);
			OnPropertyChanged();
		}
	}

	private static void SelectAll(bool select, IEnumerable<FactoryListItemViewModel> products)
	{
		foreach (var product in products)
		{
			product.IsSelected = select;
		}
	}

	private void OnIsSelectedPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(ProductListItemViewModel.IsSelected))
			OnPropertyChanged(nameof(IsAllItemsSelected));
	}

	public void UpdateFactories(IEnumerable<Domain.Models.Factory> factories)
	{
		factories = factories.OrderBy(x => x.Id);

		_factories.Clear();

		foreach (var factory in factories)
		{
			var productListItemViewModel = new FactoryListItemViewModel(factory);
			_factories.Add(productListItemViewModel);
			productListItemViewModel.PropertyChanged += OnIsSelectedPropertyChanged;
		}

		IsLoading = false;
	}
}