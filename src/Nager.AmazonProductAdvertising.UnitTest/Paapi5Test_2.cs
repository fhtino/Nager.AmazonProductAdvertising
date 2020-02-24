using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nager.AmazonProductAdvertising.UnitTest
{

    [TestClass]
    public class Paapi5Test_2
    {

        private AmazonProductAdvertisingClient _client;

        public TestContext TestContext { get; set; }


        [TestInitialize]
        public void TestInitialize()
        {
            // read config
            string accessKey = TestContext.Properties["accessKey"].ToString();
            string secretKey = TestContext.Properties["secretKey"].ToString();
            string parnterTag = TestContext.Properties["parnterTag"].ToString();
            AmazonEndpoint endpoint = Enum.Parse<AmazonEndpoint>(TestContext.Properties["endpoint"].ToString());

            // setup client
            var amazonAuthentication = new AmazonAuthentication(accessKey, secretKey);
            this._client = new AmazonProductAdvertisingClient(amazonAuthentication, endpoint, parnterTag, strictJsonMapping: true);
        }


        [TestMethod]
        [DataRow("1594484805")]
        public async Task GetItemsTest(string asin)
        {
            // Note: 1594484805 --> Daniel Pink - "Drive: The Surprising Truth About What Motivates Us"
            var itemsResponse = await _client.GetItemsAsync(new[] { asin });
            Assert.IsNotNull(itemsResponse.ItemsResult.Items[0].BrowseNodeInfo.WebsiteSalesRank);
        }


        [TestMethod]
        [DataRow("1594484805")]
        public async Task GetItemsTestLessDetails(string asin)
        {
            var request = new Model.ItemsRequest()
            {
                ItemIds = new[] { asin },
                Resources = new[]
                {
                    "BrowseNodeInfo.BrowseNodes",
                    "BrowseNodeInfo.BrowseNodes.SalesRank",
                    "BrowseNodeInfo.WebsiteSalesRank",

                    "Images.Primary.Small",
                    "Images.Primary.Medium",
                    "Images.Primary.Large",

                    "ItemInfo.ByLineInfo",
                    "ItemInfo.Classifications",
                    "ItemInfo.ContentInfo",
                    "ItemInfo.ContentRating",
                    "ItemInfo.ExternalIds",
                    "ItemInfo.Features",
                    "ItemInfo.ManufactureInfo",
                    "ItemInfo.ProductInfo",
                    "ItemInfo.TechnicalInfo",
                    "ItemInfo.Title",
                    "ItemInfo.TradeInInfo",
                }
            };

            var itemsResponse = await _client.GetItemsAsync(request);
            Assert.IsNotNull(itemsResponse.ItemsResult.Items[0].BrowseNodeInfo.WebsiteSalesRank);
        }


        [TestMethod]
        [DataRow("Drive: The Surprising Truth About What Motivates Us", "1594484805")]
        public async Task SearchItemsTest(string keyword, string asin)
        {
            var searchItemsResp = await _client.SearchItemsAsync(keyword);
            var item = searchItemsResp.SearchResult.Items.FirstOrDefault(x => x.ASIN == asin);
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.BrowseNodeInfo.WebsiteSalesRank);
        }

    }
}
