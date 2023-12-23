using Application.Abstractions.Repositories;
using Application.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class ProductRepository : GenericRepository<DbModels.Product>, IProductRepository
{
	private readonly ICompanyDbContextFactory _companyDbContextFactory;
	private readonly MapperlyMapper _mapper;
	private const string TableName = "product";

	public ProductRepository(ICompanyDbContextFactory companyDbContextFactory, MapperlyMapper mapper)
		: base(companyDbContextFactory)
	{
		_companyDbContextFactory = companyDbContextFactory;
		_mapper = mapper;
	}

	public async Task<Product?> FindByIdAsync(int productId)
	{
		var dbProduct = await FindEntityByColumnAsync(TableName, "id", productId);

		if (dbProduct == null) return null;

		var domainProduct = _mapper.Map(dbProduct);

		return domainProduct;
	}

	public async Task<IEnumerable<Product>> FindByNameAsync(string name)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var nameParameter = new NpgsqlParameter("@name", $"%{name}%");

		var dbProducts = await context
			.Products
			.FromSqlRaw(@$"
					SELECT
					    *
					FROM
					    product
					WHERE
					    name ILIKE @name", nameParameter).ToListAsync();

		var modelProducts = dbProducts.Select(_mapper.Map);

		return modelProducts;
	}

	public async Task<IEnumerable<Product>> GetAllAsync(PagingParams? pagingParams = null)
	{
		pagingParams ??= PagingParams.Default;

		var dbProducts = await GetAllEntitiesAsync(TableName, pagingParams);

		var modelProducts = dbProducts.Select(_mapper.Map);

		return modelProducts;
	}

	public async Task<IEnumerable<Product>> GetAllFromSpecificFactoryAsync(int factoryId, PagingParams? pagingParams = null)
	{
		pagingParams ??= PagingParams.Default;

		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbProducts = await context
			.Products
			.FromSql($@"
					SELECT
					    p.id,
					    p.name,
					    p.model,
					    p.code,
					    p.year_of_launch,
					    p.number_per_year,
					    p.tech_specs,
					    p.selling_price
					FROM product AS p
					    JOIN factory_product AS fp ON fp.product_id = p.id
					    JOIN factory         AS f  ON f.id = fp.factory_id
					WHERE
					    f.id = {factoryId}
					ORDER BY
						p.id
					LIMIT
						{pagingParams.PageSize}
					OFFSET
						{(pagingParams.PageNumber - 1) * pagingParams.PageSize}").ToListAsync();

		var modelProducts = dbProducts.Select(_mapper.Map);

		return modelProducts;
	}

	public async Task<IEnumerable<Product>> GetAllFromSpecificOrderAsync(int orderId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		var dbProducts = await context
			.Products
			.FromSql($@"
					SELECT
					    p.id,
					    p.name,
					    p.model,
					    p.code,
					    p.year_of_launch,
					    p.number_per_year,
					    p.tech_specs,
					    p.selling_price
					FROM ""order""        AS o
					    JOIN order_detail AS od ON o.id = od.order_id
					    JOIN product      AS p  ON od.product_id = p.id
					WHERE
					    o.id = {orderId}").ToListAsync();

		var modelProducts = dbProducts.Select(_mapper.Map);

		return modelProducts;
	}

	public async Task<int> AddAsync(Product product, int factoryId)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO product (name, model, code, year_of_launch, number_per_year, tech_specs, selling_price)
					VALUES ({product.Name}, {product.Model}, {product.Code}, {product.YearOfLaunch}, 
						{product.NumberPerYear}, {product.TechSpecs}, {product.SellingPrice})");

		var productId = (await GetLastAddedEntity(TableName))!.Id;

		await context
			.Database
			.ExecuteSqlAsync($@"
					INSERT INTO factory_product (factory_id, product_id)
					VALUES ({factoryId}, {productId})");

		return productId;
	}

	public async Task DeleteAsync(int productId)
	{
		await DeleteEntityAsync(TableName, productId);
	}

	public async Task UpdateAsync(Product product)
	{
		await using var context = _companyDbContextFactory.CreateCompanyDbContext();

		await context
			.Database
			.ExecuteSqlAsync($@"
					UPDATE product
					SET 
					    name = {product.Name},
					    model = {product.Model},
					    code = {product.Code},
					    year_of_launch = {product.YearOfLaunch},
					    number_per_year = {product.NumberPerYear},
					    tech_specs = {product.TechSpecs},
					    selling_price = {product.SellingPrice}
					WHERE
						id = {product.Id}");
	}
}