using System;
using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.FindResult(product => product.Price < 500 && product.Price > 200 && product.Cost > 30);

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
            var actual = products.Where(x => x.Price < 500 && x.Price > 200 && x.Cost > 30);

            var expected = new List<Product>()
            {
                //new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employees_that_age_higher_30()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.FindResult(employee => employee.Age > 30);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employees_that_index()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.FindSourceofIndex((employee, index) => employee.Age > 30 && index > 1);

            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_urls_should_return_startwith_https()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = WithoutLinq.Tohttps(urls);

            var expected = new List<string>()
            {
                "https://tw.yahoo.com",
                "https://facebook.com",
                "https://twitter.com",
                "https://github.com",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_urls_should_return_url_length()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = WithoutLinq.GetUrlLength(urls);

            var expected = new List<int>()
            {
                19, 20, 19, 17,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employee_that_age_lower_then_25_should_show_role_name()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees
                .AthenaWhere(x => x.Age < 25)
                .AthenaSelect(x => $"{x.Role}:{x.Name}");

            foreach (var item in actual)
            {
                Console.WriteLine(item);
            }

            var expected = new List<string>()
            {
                "OP:Andy",
                "Engineer:Frank",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }
    }
}

internal static class WithoutLinq
{
    public static IEnumerable<T> FindResult<T>(this IEnumerable<T> sources, Func<T, bool> predicate)
    {
        foreach (var item in sources)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<T> FindSourceofIndex<T>(this IEnumerable<T> sources, Func<T, int, bool> predicate)
    {
        var index = 0;

        foreach (var item in sources)
        {
            if (predicate(item, index))
            {
                yield return item;
            }
            index++;
        }
    }

    internal static IEnumerable<string> Tohttps(IEnumerable<string> urls)
    {
        foreach (var url in urls)
        {
            yield return url.Replace("http:", "https:");
        }
    }

    public static IEnumerable<int> GetUrlLength(IEnumerable<string> urls)
    {
        foreach (var url in urls)
        {
            yield return url.Length;
        }
    }
}

internal static class YourOwnLinq
{
    public static IEnumerable<TSource> AthenaWhere<TSource>(this IEnumerable<TSource> sources, Predicate<TSource> predicate)
    {
        foreach (var item in sources)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<TSource> AthenaWhereofIndex<TSource>(this IEnumerable<TSource> sources, Func<TSource, int, bool> predicate)
    {
        var index = 0;

        foreach (var item in sources)
        {
            if (predicate(item, index))
            {
                yield return item;
            }
        }
        index++;
    }

    public static IEnumerable<TResult> AthenaSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        foreach (var item in source)
        {
            yield return selector(item);
        }
    }
}