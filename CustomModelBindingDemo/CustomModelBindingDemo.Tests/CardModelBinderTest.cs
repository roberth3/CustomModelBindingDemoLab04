using CustomModelBindingDemo.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using CustomModelBindingDemo.Models;

using Moq;
using System.Web;

namespace CustomModelBindingDemo.Tests
{


    /// <summary>
    ///This is a test class for MemberModelBinderTest and is intended
    ///to contain all MemberModelBinderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CardModelBinderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        // this is an example test method a more thorough set of tests would be required to deal with all possible
        // binding scenarios
        [TestMethod()]
        public void CanBindToExpiry()
        {
            // Arrange
            var formCollection = new NameValueCollection { 
                { "foo.CardNo", "123456789" },
                { "foo.Code", "123" }
            };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Card));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };            
			
			var mockHttpContext = new Mock<HttpContextBase>();
			mockHttpContext
				.SetupGet(c => c.Request["Expiry.Value.Month"])
				.Returns(() => "7");
			mockHttpContext
				.SetupGet(c => c.Request["Expiry.Value.Year"])
				.Returns(() => "2014");
				
			ControllerContext controllerContext = new ControllerContext()
			{
				HttpContext = mockHttpContext.Object
			};
			
			
			CardModelBinder b = new CardModelBinder();
            
            // Act
            Card result = (Card)b.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.AreEqual("123456789", result.CardNo, "Incorrect value for CardNo");
            Assert.AreEqual("123", result.Code, "Incorrect value for Code");
			
			Assert.AreEqual(7, result.Expiry.Value.Month, "Incorrect value for Month");
            Assert.AreEqual(2014, result.Expiry.Value.Year, "Incorrect value for Year");
        }
    }
}
