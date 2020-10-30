using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.Services;
using System.Linq;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IService<Product> _productService;
        private IService<ProductOption> _productOptionService;

        public ProductsController()
        {
            _productService = ServiceFactory.GetServiceInstance(typeof(Product));
            _productOptionService = ServiceFactory.GetServiceInstance(typeof(ProductOption));
        }

        [HttpGet]
        public Products Get(string name)
        {
            string queryString = name == null ? string.Empty : _productService.GetFilterString(name);
            return new Products(_productService.LoadItems(queryString));
        }

        [HttpGet("{id}")]
        public Product Get(Guid id)
        {
            var product = _productService.GetItemById(id);
            if (product == null || product.IsNew)
                throw new Exception();

            return product;
        }

        [HttpPost]
        public void Post(Product product)
        {
            _productService.Add(product);
        }

        [HttpPut("{id}")]
        public void Update(Guid id, Product product)
        {
            var orig = _productService.GetItemById(id);
            if (orig != null)
            {
                orig.Name = product.Name == null ? orig.Name : product.Name;
                orig.Description = product.Description == null ? orig.Description : product.Description;
                orig.Price = (int)product.Price == 0 ? orig.Price : product.Price;
                orig.DeliveryPrice = (int)product.DeliveryPrice == 0 ? orig.DeliveryPrice : product.DeliveryPrice;
            }

            if (!orig.IsNew)
            {
                _productService.Update(orig);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var product = _productService.GetItemById(id);
            if (product != null)
            {
                String filterString = _productOptionService.GetFilterString(id.ToString());
                List<ProductOption> productOptionList = _productOptionService.LoadItems(filterString);
                productOptionList.ForEach(x => _productOptionService.Delete(x.Id));
                _productService.Delete(id);
            }
        }

        [HttpGet("{productId}/options")]
        public ProductOptions GetOptions(Guid productId)
        {
            String filterString = _productOptionService.GetFilterString(productId.ToString());
            return new ProductOptions(_productOptionService.LoadItems(filterString));
        }

        [HttpGet("{productId}/options/{id}")]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = _productOptionService.GetItemById(id);
            if (option.IsNew)
                throw new Exception();

            return option;
        }

        [HttpPost("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            _productOptionService.Add(option);
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = _productOptionService.GetItemById(id);
            if (orig != null)
            {
                orig.Name = option.Name == null ? orig.Name : option.Name;
                orig.Description = option.Description == null ? orig.Description : option.Description;
            };

            if (!orig.IsNew)
                _productOptionService.Update(orig);
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            _productOptionService.Delete(id);
        }
    }
}