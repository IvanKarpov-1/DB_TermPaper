using UI.ViewModels.Base;

namespace UI.ViewModels.RepairRequest;

public class RepairRequestListItemViewModel : ViewModelBaseWithValidation, ISelectableListItem
{
	private bool _isSelected;

	public bool IsSelected
	{
		get => _isSelected;
		set => SetField(ref _isSelected, value);
	}

	public RepairRequestListItemViewModel(Domain.Models.RepairRequest repairRequest)
	{
		RepairRequest = repairRequest;
	}

	public Domain.Models.RepairRequest RepairRequest { get; }

	public int Id
	{
		get => RepairRequest.Id;
		set
		{
			RepairRequest.Id = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public DateOnly RequestDate
	{
		get => RepairRequest.RequestDate;
		set
		{
			RepairRequest.RequestDate = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string StatusName
	{
		get => RepairRequest.StatusName;
		set
		{
			RepairRequest.StatusName = value;
			OnPropertyChanged();
			ValidateProperty(value);
		}
	}

	public string? Description
	{
		get => RepairRequest.Description;
		set
		{
			RepairRequest.Description = value;
			OnPropertyChanged();
			if (value != null) ValidateProperty(value);
		}
	}

	public Domain.Models.RepairRequest GetRepairRequest()
	{
		return new Domain.Models.RepairRequest
		{
			Id = Id,
			RequestDate = RequestDate,
			StatusName = StatusName,
			Description = Description,
			CustomerId = RepairRequest.CustomerId,
			StatusId = RepairRequest.StatusId
		};
	}
}