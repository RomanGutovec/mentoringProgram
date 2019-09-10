// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {
        private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Task 1")]
        [Description("This sample return all customers whom sum of orders greater then 100.")]
        public void Linq1()
        {
            var customers =
                from c in dataSource.Customers
                where c.Orders.Sum(o => o.Total) > 100
                select new { Id = c.CustomerID, Sum = c.Orders.Sum(o => o.Total) };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 2")]
        [Description("This sample return return all suppliers which located in the same country and city.")]
        public void Linq2()
        {
            var suppliers =
                        from s in dataSource.Suppliers
                        from c in dataSource.Customers
                        where s.City == c.City && c.Country == s.Country
                        select new { Id = c.CustomerID, s.Country, s.City, s.SupplierName };

            foreach (var s in suppliers)
            {
                ObjectDumper.Write(s);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 3")]
        [Description("This sample return all customers who had orders which greater then 100")]
        public void Linq3()
        {
            var customers =
                from c in dataSource.Customers
                where c.Orders.Any(o => o.Total > 100)
                select new { c.CustomerID };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 4")]
        [Description("This sample return all customers with data when they became a client first.")]
        public void Linq4()
        {
            var customers =
                from c in dataSource.Customers
                where c.Orders.Length != 0
                select new { c.CustomerID, data = c.Orders.Min(o => o.OrderDate) };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 5")]
        [Description("This sample return all customers with data when they became a client first and sorted by month, year, sum of orders.")]
        public void Linq5()
        {
            var customers =
                from c in dataSource.Customers
                where c.Orders.Length != 0
                orderby c.Orders.Min(o => o.OrderDate).Year,
                c.Orders.Min(o => o.OrderDate).Month,
                c.Orders.Sum(o => o.Total) descending
                select new { c.CustomerID, Data = c.Orders.Min(o => o.OrderDate), Sum = c.Orders.Sum(o => o.Total) };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 6")]
        [Description("This sample return all customers who has not numeric code or region is undefined or has incorrect phone number.")]
        public void Linq6()
        {
            var customers =
                from c in dataSource.Customers
                where
                (c.PostalCode != null && char.IsLetter(c.PostalCode.ToCharArray()[0])) ||
                c.Region == null ||
                c.Phone == null ||
                !c.Phone.StartsWith("(")
                select new { c.CustomerID, c.PostalCode, c.Region, c.Phone };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 7")]
        [Description("This sample return return all presented in market products ordered by categories, existing in a storage, price.")]
        public void Linq7()
        {
            var products =
                from p in dataSource.Products
                group p by p.Category
                into groupedByCategory
                select new
                {
                    Products =
                        from product in groupedByCategory
                        orderby product.UnitPrice
                        group product by product.UnitsInStock into groupedByUnits
                        select groupedByUnits
                };

            foreach (var product in products)
            {
                foreach (var productsByCategory in product.Products)
                {
                    foreach (var result in productsByCategory)
                    {
                        ObjectDumper.Write(result);
                    }
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 8")]
        [Description("This sample return return all presented in market products by categories cheap, middle, expensive.")]
        public void Linq8()
        {
            var products =
                from p in dataSource.Products
                let range = (
                p.UnitPrice < 10 ? "cheap" :
                p.UnitPrice >= 10 && p.UnitPrice < 20 ? "middle" :
                p.UnitPrice >= 20 ? "expensive" :
                "unknown_category"
                )
                orderby range
                select new { p.Category, p.UnitPrice, range };

            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 9")]
        [Description("This sample return profitability of each city anf average intensity.")]
        public void Linq9()
        {
            var customers =
                from c in dataSource.Customers
                group c by c.City into grouped
                select new
                {
                    grouped.Key,
                    profit = (grouped.Sum(x => x.Orders.Sum(o => o.Total)) / grouped.Sum(o => o.Orders.Count())),
                    intensity = grouped.Sum(o => o.Orders.Count()) / grouped.Count()
                };

            foreach (var c in customers)
            {
                ObjectDumper.Write(c);
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 10_1")]
        [Description("This sample return annual average statistic activity of customers by months.")]
        public void Linq10()
        {
            var customers = from c in dataSource.Customers

                            select new
                            {
                                c.CustomerID,
                                Month = from p in c.Orders
                                        group p by p.OrderDate.Month
                                        into groupedByMonth
                                        select new { Month = groupedByMonth.Key, Amount = groupedByMonth.Count() }
                            };

            foreach (var item in customers)
            {
                ObjectDumper.Write(item);
                foreach (var m in item.Month)
                {
                    ObjectDumper.Write(m);
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 10_2")]
        [Description("This sample return annual average statistic activity of customers by years.")]
        public void Linq11()
        {
            var customers = from c in dataSource.Customers

                            select new
                            {
                                c.CustomerID,
                                Year = from p in c.Orders
                                       group p by p.OrderDate.Year
                                       into groupedByYear
                                       select new
                                       {
                                           Year = groupedByYear.Key,
                                           Amount = groupedByYear.Count()
                                       }
                            };

            foreach (var item in customers)
            {
                ObjectDumper.Write(item);
                foreach (var y in item.Year)
                {
                    ObjectDumper.Write(y);
                }
            }
        }

        [Category("Restriction Operators")]
        [Title("Task 10_3")]
        [Description("This sample return annual average statistic activity of customers by months and years.")]
        public void Linq12()
        {
            var customers = from c in dataSource.Customers

                            select new
                            {
                                c.CustomerID,
                                Year = from p in c.Orders
                                       group p by p.OrderDate.Year

                                       into groupedByYear
                                       select new
                                       {
                                           Year = groupedByYear.Key,
                                           Month = from g in groupedByYear
                                                   group g by g.OrderDate.Month
                                           into groupedByMonth
                                                   select new
                                                   {
                                                       Month = groupedByMonth.Key,
                                                       Amount = groupedByMonth.Count()
                                                   }
                                       }
                            };

            foreach (var item in customers)
            {
                ObjectDumper.Write(item);
                foreach (var y in item.Year)
                {
                    ObjectDumper.Write(y);
                    foreach (var m in y.Month)
                    {
                        ObjectDumper.Write(m);
                    }
                }

            }
        }
    }
}