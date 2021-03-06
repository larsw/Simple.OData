﻿using System.Linq;
using Xunit;

namespace Simple.OData.Client.Tests
{
    using Entry = System.Collections.Generic.Dictionary<string, object>;

    public class ODataClientTest : TestBase
    {
        [Fact]
        public void FindEntries()
        {
            var products = _client.FindEntries("Products");
            Assert.True(products.Count() > 0);
        }

        [Fact]
        public void FindEntryExisting()
        {
            var product = _client.FindEntry("Products?$filter=ProductName eq 'Chai'");
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void FindEntryNonExisting()
        {
            var product = _client.FindEntry("Products?$filter=ProductName eq 'XYZ'");
            Assert.Null(product);
        }

        [Fact]
        public void GetEntryExisting()
        {
            var product = _client.GetEntry("Products", new Entry() { { "ProductID", 1 } });
            Assert.Equal("Chai", product["ProductName"]);
        }

        [Fact]
        public void GetEntryNonExisting()
        {
            Assert.Throws<WebRequestException>(() => _client.GetEntry("Products", new Entry() { { "ProductID", -1 } }));
        }

        [Fact]
        public void InsertEntryWithResult()
        {
            var product = _client.InsertEntry("Products", new Entry() {{"ProductName", "Test1"}, {"UnitPrice", 18m}}, true);

            Assert.Equal("Test1", product["ProductName"]);
        }

        [Fact]
        public void InsertEntryNoResult()
        {
            var product = _client.InsertEntry("Products", new Entry() { { "ProductName", "Test2" }, { "UnitPrice", 18m } }, false);

            Assert.Null(product);
        }

        [Fact]
        public void UpdateEntry()
        {
            var key = new Entry() {{"ProductID", 1}};
            _client.UpdateEntry("Products", key, new Entry() { { "ProductName", "Chai" }, { "UnitPrice", 123m } });

            var product = _client.GetEntry("Products", key);
            Assert.Equal(123m, product["UnitPrice"]);
        }

        [Fact]
        public void DeleteEntry()
        {
            var product = _client.InsertEntry("Products", new Entry() { { "ProductName", "Test3" }, { "UnitPrice", 18m } }, true);
            product = _client.FindEntry("Products?$filter=ProductName eq 'Test3'");
            Assert.NotNull(product);

            _client.DeleteEntry("Products", product);

            product = _client.FindEntry("Products?$filter=ProductName eq 'Test3'");
            Assert.Null(product);
        }

        [Fact]
        public void LinkEntry()
        {
            var category = _client.InsertEntry("Categories", new Entry() { { "CategoryName", "Test4" } }, true);
            var product = _client.InsertEntry("Products", new Entry() { { "ProductName", "Test5" } }, true);

            _client.LinkEntry("Products", product, "Category", category);

            product = _client.FindEntry("Products?$filter=ProductName eq 'Test5'");
            Assert.NotNull(product["CategoryID"]);
            Assert.Equal(category["CategoryID"], product["CategoryID"]);
        }

        [Fact]
        public void UnlinkEntry()
        {
            var category = _client.InsertEntry("Categories", new Entry() { { "CategoryName", "Test6" } }, true);
            var product = _client.InsertEntry("Products", new Entry() { { "ProductName", "Test7" }, { "CategoryID", category["CategoryID"] } }, true);
            product = _client.FindEntry("Products?$filter=ProductName eq 'Test7'");
            Assert.NotNull(product["CategoryID"]);
            Assert.Equal(category["CategoryID"], product["CategoryID"]);

            _client.UnlinkEntry("Products", product, "Category");

            product = _client.FindEntry("Products?$filter=ProductName eq 'Test7'");
            Assert.Null(product["CategoryID"]);
        }
    }
}