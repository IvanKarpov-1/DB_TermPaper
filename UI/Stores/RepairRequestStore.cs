using Application.RepairRequest.Commands;
using Application.RepairRequest.Queries;
using Domain.Models;
using MediatR;

namespace UI.Stores;

public class RepairRequestStore
{
	private readonly IMediator _mediator;
	//private readonly HashSet<RepairRequest> _repairRequests;
	//private readonly Dictionary<int, List<RepairRequest>> _customerRepairRequests;
	
	//public IEnumerable<RepairRequest?> RepairRequestsFromCustomer(int customerId)
	//{
	//	if (_customerRepairRequests.ContainsKey(customerId) == false)
	//		_customerRepairRequests[customerId] = new List<RepairRequest>();
	//	return _customerRepairRequests[customerId];
	//}

	//public IEnumerable<RepairRequest> RepairRequests => _repairRequests;

	//public event Action<RepairRequest>? RepairRequestAdded;
	//public event Action<int>? RepairRequestRemoved; 

	public RepairRequestStore(IMediator mediator)
	{
		_mediator = mediator;
		//_repairRequests = new HashSet<RepairRequest>();
		//_customerRepairRequests = new Dictionary<int, List<RepairRequest>>();
	}

	public async Task<RepairRequest?> GetById(int repairRequestId)
	{
		return await _mediator.Send(new GetRepairRequestByIdQuery { Id = repairRequestId });
	}

	public async Task<IEnumerable<RepairRequest>> GetRepairRequestsOfSpecificCustomer(int customerId)
	{
		//if (_customerRepairRequests.TryGetValue(customerId, out var value) && value.Count > 0) return;

		var repairRequests = (await _mediator.Send(new GetAllCustomerRepairRequestsQuery { CustomerId = customerId })).ToList();
		return repairRequests;

		//_customerRepairRequests[customerId] = repairRequests;

		//foreach (var repairRequest in repairRequests)
		//{
		//	_repairRequests.Add(repairRequest);
		//}
	}

	public async Task Add(RepairRequest repairRequest)
	{
		var repairRequestId = await _mediator.Send(new AddRepairRequestCommand { RepairRequest = repairRequest });

		//if (repairRequestId != null) repairRequest.Id = (int)repairRequestId;

		//_repairRequests.Add(repairRequest);

		//if (_customerRepairRequests.ContainsKey((int)repairRequest.CustomerId!) == false)
		//	_customerRepairRequests[(int)repairRequest.CustomerId!] = new List<RepairRequest>();
		//_customerRepairRequests[(int)repairRequest.CustomerId!].Add(repairRequest);

		//OnRepairRequestAdded(repairRequest);
	}

	public async Task Update(RepairRequest repairRequest)
	{
		await _mediator.Send(new UpdateRepairRequestCommand { RepairRequest = repairRequest });

		//_repairRequests.RemoveWhere(x => x.Id == repairRequest.Id);
		//_repairRequests.Add(repairRequest);

		//if (_customerRepairRequests.ContainsKey((int)repairRequest.CustomerId!) == false)
		//{
		//	_customerRepairRequests[(int)repairRequest.CustomerId!] = new List<RepairRequest>();
		//}
		//else
		//{
		//	_customerRepairRequests[(int)repairRequest.CustomerId!].RemoveAt(
		//		_customerRepairRequests[(int)repairRequest.CustomerId!].FindIndex(x => x.Id == repairRequest.Id));
		//	_customerRepairRequests[(int)repairRequest.CustomerId!].Add(repairRequest);
		//}

		//OnRepairRequestAdded(repairRequest);
	}

	public async Task Delete(int repairRequestId)
	{
		await _mediator.Send(new DeleteRepairRequestCommand { Id = repairRequestId });

		//_repairRequests.RemoveWhere(x => x.Id == repairRequestId);

		//var customerId = _customerRepairRequests.FirstOrDefault(x => x.Value.Exists(x => x.Id == repairRequestId)).Key;

		//if (_customerRepairRequests.ContainsKey(customerId))
		//	_customerRepairRequests[customerId].RemoveAt(_customerRepairRequests[customerId].FindIndex(x => x.Id == repairRequestId));

		//RepairRequestRemoved?.Invoke(repairRequestId);
	}

	//private void OnRepairRequestAdded(RepairRequest repairRequest) => RepairRequestAdded?.Invoke(repairRequest);
}