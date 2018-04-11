using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BookStore.Models;
using JetBrains.Annotations;
using log4net;
using log4net.Config;
using Microsoft.WindowsAzure.Storage;

namespace BookStore
{
    public class MvcApplication : HttpApplication
    {
        [NotNull] private readonly ILog _logger = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            Database.SetInitializer(new BookDbInitializer());
            Database.SetInitializer(new CourseDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            XmlConfigurator.Configure();

            WriteToBlobStorage();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            if (ex != null)
            {
                _logger.Fatal(ex.ToString());
            }
        }

        private static void WriteToBlobStorage()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Storage"].ConnectionString;
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference("logs");
            container.CreateIfNotExists();

            var now = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            var blockBlob = container.GetBlockBlobReference($"Hello_{now}.txt");
            blockBlob.Properties.ContentType = "text/plain";

            var content = $"{Environment.MachineName} {now}";
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(content)))
            {
                blockBlob.UploadFromStream(stream);
            }
        }
    }
}