using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;
using ShopOnline.Api.Extensions;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //用依赖注入的方式，把ProductRepository注入：1.创建构造函数；2.在构造函数中注入ProductRepository
        //注入后，我们就可以在Controller使用ProductRepository来访问数据层
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCategories = await this.productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                else
                {
                    //做个连接查询:Product里的CategoryId去找ProductCategory里的Id，然后再把ProductCategory的Name传给ProductDto里CategoryName，然后返回ProductDto这样一个对象的集合

                    //因为这个从实体到它的Dto对象的转换要经常用到，所以为了减少代码冗余，我们就专门创建一个扩展类
                    var productDtos = products.ConvertToDto(productCategories);

                    return Ok(productDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

        //4.1获取商品明细
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await this.productRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    //做个连接查询:Product里的CategoryId去找ProductCategory里的Id，然后再把ProductCategory的Name传给ProductDto里CategoryName，然后返回ProductDto单个对象

                    //因为这个从实体到它的Dto对象的转换要经常用到，所以为了减少代码冗余，我们专门创建了一个扩展类，去完善这个扩展类
                    var productCategory = await this.productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productCategory);

                    return Ok(productDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }


        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await productRepository.GetCategories();

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }

        }

        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await productRepository.GetItemsByCategory(categoryId);
                var productCategories = await productRepository.GetCategories();
                var productDtos = products.ConvertToDto(productCategories);

                return Ok(productDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
        }

    }
}