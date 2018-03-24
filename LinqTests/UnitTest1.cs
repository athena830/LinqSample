using System;
using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = WithoutLinq.FindProductByPrice(products, product => product.Price < 500 && product.Price>200 && product.Cost > 30);

            var expected = new List<Product>()
            {
                //new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_that_price_between_200_and_500_by_where()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.Where(x => x.Price < 500 && x.Price > 200 && x.Cost>30);

            var expected = new List<Product>()
            {
                //new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

    }
}

internal class WithoutLinq
{
    public static IEnumerable<Product> FindProductByPrice(IEnumerable<Product> products, Func<Product, bool> predicate)
    {
        foreach (var product in products)
        {
            if (predicate(product))
            {
                yield return product;
            }
        }
    }
}

internal class YourOwnLinq
{
}