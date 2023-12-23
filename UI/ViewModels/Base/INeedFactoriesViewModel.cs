namespace UI.ViewModels.Base;

public interface INeedFactoriesViewModel
{
	public void UpdateFactories(IEnumerable<Domain.Models.Factory> factories);
}