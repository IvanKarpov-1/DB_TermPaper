using Application.Abstractions.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Persistence.DbContext;
using System.Globalization;

namespace Persistence.Repositories;

public class OrderRepository : GenericRepository<DbModels.Order>, IOrderRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "\"order\"";

	public OrderRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		:base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<Order?> FindByIdAsync(int orderId)
	{
		var dbOrder = await FindEntityByColumnAsync(TableName, "id", orderId);

		if (dbOrder == null) return null;

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbOrderDetails = await context
			.OrderDetails
			.FromSql($@"
					SELECT
						*
					FROM
						order_detail
					WHERE
						order_id = {dbOrder.Id}").ToListAsync();

		var domainOrder = _mapper.Map(dbOrder);
		var domainOrderDetails = dbOrderDetails.Select(_mapper.Map);
		domainOrder.OrderDetails = domainOrderDetails;

		return domainOrder;
	}

	public async Task<IEnumerable<Order?>> GetCustomerOrdersAsync(int customerId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbOrders = await context
			.Orders
			.FromSql($@"
					SELECT
						*
					FROM
						""order""
					WHERE
						customer_id = {customerId}
					").ToListAsync();

		var modelOrders = dbOrders.Select(_mapper.Map).ToList();

		if (modelOrders.Count == 0) return modelOrders;

		var orderIds = dbOrders.Select(x => x.Id).ToList();

		var orderDetailSelectQuery = "SELECT * FROM order_detail WHERE order_id IN ({0})";

		var parameters = string.Join(", ", Enumerable.Range(0, orderIds.Count).Select(i => $"@p{i}"));

		orderDetailSelectQuery = string.Format(orderDetailSelectQuery, parameters);

		object[] sqlParameters = orderIds
			.Select((id, index) => new NpgsqlParameter($"@p{index}", id))
			.ToArray();

		var dbOrderDetails = await context
			.OrderDetails
			.FromSqlRaw(orderDetailSelectQuery, sqlParameters)
			.ToListAsync();

		foreach (var modelOrder in modelOrders)
		{
			var modelOrderDetails = dbOrderDetails
				.Where(x => x.OrderId == modelOrder.Id)
				.Select(_mapper.Map);

			modelOrder.OrderDetails = modelOrderDetails;
		}

		return modelOrders;
	}

	public async Task<int> AddAsync(Order order)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync(@$"
					INSERT INTO ""order"" (customer_id, address_id, order_date, delivery_date)
					VALUES ({order.CustomerId}, {order.AddressId}, {order.OrderDate}, {order.DeliveryDate})");

		var orderDetailInsertQuery = "INSERT INTO order_detail (order_id, product_id, total_price, quantity) VALUES";

		var orderId = (await GetLastAddedEntity(TableName))!.Id;

		var count = 0;
		foreach (var orderDetail in order.OrderDetails)
		{
			var totalPrice = orderDetail.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture);

			if (count is 0)
				orderDetailInsertQuery +=
					$"({orderId}, {orderDetail.ProductId}, {totalPrice}, {orderDetail.Quantity})";
			else
				orderDetailInsertQuery +=
					$",({orderId}, {orderDetail.ProductId}, {totalPrice}, {orderDetail.Quantity})";
			
			count++;
		}

		await context
			.Database
			.ExecuteSqlRawAsync(orderDetailInsertQuery);
			
		return orderId;
	}

	public async Task DeleteAsync(int orderId)
	{
		await DeleteEntityAsync(TableName, orderId);
	}

	public async Task UpdateAsync(Order order)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE ""order""
					SET
						customer_id = {order.CustomerId},
						address_id = {order.AddressId},
						order_date = {order.OrderDate},
						delivery_date = {order.DeliveryDate}
					WHERE
						id = {order.Id}");
	}
}