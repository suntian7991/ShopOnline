using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;


namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await this.shopOnlineDbContext.ProductCategories.ToListAsync();

            return categories;

        }

        //4.1③实现获取商品明细API
        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
            return category;
        }

        //4.1实现获取商品明细API
        public async Task<Product> GetItem(int id)
        {
            var product = await shopOnlineDbContext.Products.FindAsync(id);

            return product;
        }

        //此方法实际上是从数据库中的Product去取数据,但是我们不需要用SQL语句去做
        //而是通过DbContext来实现：1.创建构造函数;2.在构造函数中注入ShopOnlineDbContext
        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await this.shopOnlineDbContext.Products.ToListAsync();

            return products;

        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await (from product in shopOnlineDbContext.Products
                                     where product.CategoryId == id
                                     select product).ToListAsync();
            return products;
        }
    }
}