namespace UI.ViewModels.Base;

public interface ILoadingViewModel
{
	public bool IsLoading { get; }
	public void Load();
}