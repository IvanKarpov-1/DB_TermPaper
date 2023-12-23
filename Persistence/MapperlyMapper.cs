using Persistence.DbModels;
using Riok.Mapperly.Abstractions;

namespace Persistence;

[Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive, UseReferenceHandling = true)]
public partial class MapperlyMapper
{
	[MapperIgnoreSource(nameof(Product.Factories))]
	[MapperIgnoreSource(nameof(Product.OrderDetails))]
	public partial Domain.Models.Product Map(Product dbProduct);

	[MapperIgnoreTarget(nameof(Domain.Models.Factory.Address))]
	[MapperIgnoreSource(nameof(Factory.Products))]
	[MapperIgnoreSource(nameof(Factory.Address))]
	public partial Domain.Models.Factory Map(Factory dbFactory);

	[MapperIgnoreSource(nameof(Address.CustomerAddress))]
	[MapperIgnoreSource(nameof(Address.Factory))]
	public partial Domain.Models.Address Map(Address dbAddress);
	
	[MapperIgnoreSource(nameof(Order.CustomerAddress))]
	public partial Domain.Models.Order Map(Order dbOrder);

	[MapperIgnoreSource(nameof(OrderDetail.Order))]
	[MapperIgnoreSource(nameof(OrderDetail.Product))]
	public partial Domain.Models.OrderDetail Map(OrderDetail dbOrderDetail);

	[MapperIgnoreTarget(nameof(Domain.Models.Customer.Address))]
	[MapperIgnoreSource(nameof(Customer.CustomerAddresses))]
	[MapperIgnoreSource(nameof(Customer.RepairRequests))]
	public partial Domain.Models.Customer Map(Customer dbCustomer);

	[MapperIgnoreTarget(nameof(Domain.Models.RepairRequest.StatusName))]
	[MapperIgnoreSource(nameof(RepairRequest.Customer))]
	[MapperIgnoreSource(nameof(RepairRequest.Status))]
	public partial Domain.Models.RepairRequest Map(RepairRequest dbCustomer);
}