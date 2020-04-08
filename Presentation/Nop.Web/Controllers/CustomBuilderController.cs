using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using Nop.Core;
using System.Data.SqlClient;
using System.Text;

using Microsoft.Extensions.Primitives;
using Nop.Services.Media;
using Nop.Web.Models.Media;
using Microsoft.AspNetCore.Session;
using Nop.Core.Http.Extensions;
using Nop.Core.Data;


namespace Nop.Web.Controllers
{
    public class CustomBuilderController : Controller
    {
        //   string connectionString = new DataSettingsManager().LoadSettings().DataConnectionString;
        // SqlConnection con = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
        SqlConnection con = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);

        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public bool IsCustomerImpersonated;

        public CustomBuilderController(IPictureService pictureService, IWebHelper webHelper, IWorkContext workContext)
        {
            this._pictureService = pictureService;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }

        public IActionResult Index()
        {
            ViewBag.IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null;

            //Flag Product Variable

            string prdidflag = Request.Query["prdidflag"];
            string prdimgflag = Request.Query["prdimgflag"];
            string prdnameflag = Request.Query["prdnameflag"];
            string prdskuflag = Request.Query["prdskuflag"];
            //string prdpriceflag = Request.Query["prdpriceflag"];
            string prdpriceflag = "";

            //Flag Shaft Product Variable

            string prdidflagshaft = Request.Query["prdidflagshaft"];
            string prdimgflagshaft = Request.Query["prdimgflagshaft"];
            string prdnameflagshaft = Request.Query["prdnameflagshaft"];
            string prdskuflagshaft = Request.Query["prdskuflagshaft"];
            //string prdpriceflagshaft = Request.Query["prdpriceflagshaft"];
            string prdpriceflagshaft = "";

            //Quick Disconnect Product Variable

            string prdidqd = Request.Query["prdidqd"];
            string prdimgqd = Request.Query["prdimgqd"];
            string prdnameqd = Request.Query["prdnameqd"];
            string prdskuqd = Request.Query["prdskuqd"];
            //string prdpriceqd = Request.Query["prdpriceqd"];
            string prdpriceqd = "";

            //Mounting Bracket Product Variable

            string prdidmb = Request.Query["prdidmb"];
            string prdimgmb = Request.Query["prdimgmb"];
            string prdnamemb = Request.Query["prdnamemb"];
            string prdskumb = Request.Query["prdskumb"];
            //string prdpricemb = Request.Query["prdpricemb"];
            string prdpricemb = "";

            //Whip Lighting Product Variable

            string prdidwl = Request.Query["prdidwl"];
            string prdimgwl = Request.Query["prdimgwl"];
            string prdnamewl = Request.Query["prdnamewl"];
            string prdskuwl = Request.Query["prdskuwl"];
            //string prdpricewl = Request.Query["prdpricewl"];
            string prdpricewl = "";

            /* To set flag product from query string */

            if (prdidflag == null)
            {
                ViewBag.prdpriceflag = HttpContext.Session.GetString("prdpriceflag");
                if (HttpContext.Session.GetString("prdidflag") != null)
                {
                    ViewBag.prdidflag = HttpContext.Session.GetString("prdidflag");
                    ViewBag.prdimgflag = HttpContext.Session.GetString("prdimgflag");
                    ViewBag.prdnameflag = HttpContext.Session.GetString("prdnameflag");
                    ViewBag.prdskuflag = HttpContext.Session.GetString("prdskuflag");
                    ViewBag.resultattrflag = HttpContext.Session.GetString("resultattrflag");
                }
            }
            else
            {
                // con = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                con.Open();
                SqlCommand prdprcflg = new SqlCommand("SELECT Price FROM Product WHERE Id =" + prdidflag, con);
                SqlDataReader prcflag = prdprcflg.ExecuteReader();
                prcflag.Read();
                var prdpricequeryold = String.Format("{0:0.00}", prcflag["Price"]);
                prdpriceflag = "$" + prdpricequeryold.ToString();

                HttpContext.Session.SetString("prdidflag", Convert.ToString(prdidflag));
                if (prdimgflag == null)
                {
                    HttpContext.Session.SetString("prdimgflag", " http://localhost:15536/images/thumbs/default-image_550.png");
                }
                else
                {
                    HttpContext.Session.SetString("prdimgflag", prdimgflag);
                }
                HttpContext.Session.SetString("prdnameflag", prdnameflag);
                if (prdskuflag != null)
                {
                    HttpContext.Session.SetString("prdskuflag", prdskuflag);
                }
                if (prdpriceflag != null)
                {
                    HttpContext.Session.SetString("prdpriceflag", prdpriceflag);
                }

                // con = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                con = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                con.Open();
                SqlCommand attrflag = new SqlCommand("SELECT Name FROM ProductAttributeValue INNER JOIN Product_ProductAttribute_Mapping " +
                    "ON ProductAttributeValue.ProductAttributeMappingId = Product_ProductAttribute_Mapping.Id " +
                    "WHERE Product_ProductAttribute_Mapping.ProductId =" + prdidflag, con);
                SqlDataReader sdrflag = attrflag.ExecuteReader();
                List<string> prdattflag = new List<string>();

                while (sdrflag.Read())
                {
                    prdattflag.Add(Convert.ToString(sdrflag["Name"]));
                }

                string resultattrflag = string.Join(",", prdattflag.Select(i => i.ToString()).ToArray());

                HttpContext.Session.SetString("resultattrflag", resultattrflag);

                // string prdattflag = "3/4\" Fully lit";
                //string[] prdattflag = { "3/4\" Fully lit", "1/2\" Tapered", "1/2\"" ,"3' x 5'" };

                ViewBag.prdidflag = HttpContext.Session.GetString("prdidflag");
                ViewBag.prdimgflag = HttpContext.Session.GetString("prdimgflag");
                ViewBag.prdnameflag = HttpContext.Session.GetString("prdnameflag");
                ViewBag.prdskuflag = HttpContext.Session.GetString("prdskuflag");
                ViewBag.prdpriceflag = HttpContext.Session.GetString("prdpriceflag");
                ViewBag.resultattrflag = HttpContext.Session.GetString("resultattrflag");

                con.Close();

            }

            /* To set flag shaft product from query string */

            if (prdidflagshaft == null)
            {
                ViewBag.prdpriceflagshaft = HttpContext.Session.GetString("prdpriceflagshaft");
                if (HttpContext.Session.GetString("prdidflagshaft") != null)
                {
                    ViewBag.prdidflagshaft = HttpContext.Session.GetString("prdidflagshaft");
                    ViewBag.prdimgflagshaft = HttpContext.Session.GetString("prdimgflagshaft");
                    ViewBag.prdnameflagshaft = HttpContext.Session.GetString("prdnameflagshaft");
                    ViewBag.prdskuflagshaft = HttpContext.Session.GetString("prdskuflagshaft");
                    ViewBag.resultattrflagshaft = HttpContext.Session.GetString("resultattrflagshaft");
                }

            }
            else
            {
                SqlConnection conflagshaft = null;
                //conflagshaft = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                conflagshaft = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                conflagshaft.Open();
                SqlCommand prdprcflg = new SqlCommand("SELECT Price FROM Product WHERE Id =" + prdidflagshaft, conflagshaft);
                SqlDataReader prcflag = prdprcflg.ExecuteReader();
                prcflag.Read();
                var prdpricequeryold = String.Format("{0:0.00}", prcflag["Price"]);
                prdpriceflagshaft = "$" + prdpricequeryold.ToString();


                HttpContext.Session.SetString("prdidflagshaft", prdidflagshaft);
                if (prdimgflagshaft == null)
                {
                    HttpContext.Session.SetString("prdimgflagshaft", " http://localhost:15536/images/thumbs/default-image_550.png");
                }
                else
                {
                    HttpContext.Session.SetString("prdimgflagshaft", prdimgflagshaft);
                }
                HttpContext.Session.SetString("prdnameflagshaft", prdnameflagshaft);
                if (prdskuflagshaft != null)
                {
                    HttpContext.Session.SetString("prdskuflagshaft", prdskuflagshaft);
                }
                if (prdpriceflagshaft != null)
                {
                    HttpContext.Session.SetString("prdpriceflagshaft", prdpriceflagshaft);
                }

                SqlConnection con = null;
                //con = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                con = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                con.Open();
                SqlCommand attrflagshaft = new SqlCommand("SELECT Name FROM ProductAttributeValue INNER JOIN Product_ProductAttribute_Mapping " +
                    "ON ProductAttributeValue.ProductAttributeMappingId = Product_ProductAttribute_Mapping.Id " +
                    "WHERE Product_ProductAttribute_Mapping.ProductId =" + prdidflagshaft, con);
                SqlDataReader sdrflagshaft = attrflagshaft.ExecuteReader();
                List<string> prdattflagshaft = new List<string>();

                while (sdrflagshaft.Read())
                {
                    prdattflagshaft.Add(Convert.ToString(sdrflagshaft["Name"]));
                }

                string resultattrflagshaft = string.Join(",", prdattflagshaft.Select(i => i.ToString()).ToArray());

                HttpContext.Session.SetString("resultattrflagshaft", resultattrflagshaft);

                ViewBag.prdidflagshaft = HttpContext.Session.GetString("prdidflagshaft");
                ViewBag.prdimgflagshaft = HttpContext.Session.GetString("prdimgflagshaft");
                ViewBag.prdnameflagshaft = HttpContext.Session.GetString("prdnameflagshaft");
                ViewBag.prdskuflagshaft = HttpContext.Session.GetString("prdskuflagshaft");
                ViewBag.prdpriceflagshaft = HttpContext.Session.GetString("prdpriceflagshaft");
                ViewBag.resultattrflagshaft = HttpContext.Session.GetString("resultattrflagshaft");
            }

            /* To set Quick Disconnect product from query string */

            if (prdidqd == null)
            {
                ViewBag.prdpriceqd = HttpContext.Session.GetString("prdpriceqd");
                if (HttpContext.Session.GetString("prdidqd") != null)
                {
                    ViewBag.prdidqd = HttpContext.Session.GetString("prdidqd");
                    ViewBag.prdimgqd = HttpContext.Session.GetString("prdimgqd");
                    ViewBag.prdnameqd = HttpContext.Session.GetString("prdnameqd");
                    ViewBag.prdskuqd = HttpContext.Session.GetString("prdskuqd");
                }
            }
            else
            {
                SqlConnection conqd = null;
                //conqd = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                conqd = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                conqd.Open();
                SqlCommand prdprcflg = new SqlCommand("SELECT Price FROM Product WHERE Id =" + prdidqd, conqd);
                SqlDataReader prcflag = prdprcflg.ExecuteReader();
                prcflag.Read();
                var prdpricequeryold = String.Format("{0:0.00}", prcflag["Price"]);
                prdpriceqd = "$" + prdpricequeryold.ToString();


                HttpContext.Session.SetString("prdidqd", prdidqd);
                if (prdimgqd == null)
                {
                    HttpContext.Session.SetString("prdimgqd", " http://localhost:15536/images/thumbs/default-image_550.png");
                }
                else
                {
                    HttpContext.Session.SetString("prdimgqd", prdimgqd);
                }
                HttpContext.Session.SetString("prdnameqd", prdnameqd);
                if (prdskuqd != null)
                {
                    HttpContext.Session.SetString("prdskuqd", prdskuqd);
                }
                HttpContext.Session.SetString("prdpriceqd", prdpriceqd);

                ViewBag.prdidqd = HttpContext.Session.GetString("prdidqd");
                ViewBag.prdimgqd = HttpContext.Session.GetString("prdimgqd");
                ViewBag.prdnameqd = HttpContext.Session.GetString("prdnameqd");
                ViewBag.prdskuqd = HttpContext.Session.GetString("prdskuqd");
                ViewBag.prdpriceqd = HttpContext.Session.GetString("prdpriceqd");
            }

            /* To set Mouting Bracket Products from Query String */

            if (prdidmb == null)
            {
                if (HttpContext.Session.GetString("prdpricemb") != null)
                {
                    ViewBag.prdpricemb = HttpContext.Session.GetString("prdpricemb");
                } else
                {
                    HttpContext.Session.SetString("prdpricemb", "$0.00");
                }
                if (HttpContext.Session.GetString("prdidmb") != null)
                {
                    ViewBag.prdidmb = HttpContext.Session.GetString("prdidmb");
                    ViewBag.prdimgmb = HttpContext.Session.GetString("prdimgmb");
                    ViewBag.prdnamemb = HttpContext.Session.GetString("prdnamemb");
                    ViewBag.prdskumb = HttpContext.Session.GetString("prdskumb");
                }

            }
            else
            {
                SqlConnection conmb = null;
                //conmb = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                conmb = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                conmb.Open();
                SqlCommand prdprcflg = new SqlCommand("SELECT Price FROM Product WHERE Id =" + prdidmb, conmb);
                SqlDataReader prcflag = prdprcflg.ExecuteReader();
                prcflag.Read();
                var prdpricequeryold = String.Format("{0:0.00}", prcflag["Price"]);
                prdpricemb = "$" + prdpricequeryold.ToString();

                HttpContext.Session.SetString("prdidmb", prdidmb);
                if (prdimgmb == null)
                {
                    HttpContext.Session.SetString("prdimgmb", " http://localhost:15536/images/thumbs/default-image_550.png");
                }
                else
                {
                    HttpContext.Session.SetString("prdimgmb", prdimgmb);
                }
                HttpContext.Session.SetString("prdnamemb", prdnamemb);
                if (prdskumb != null)
                {
                    HttpContext.Session.SetString("prdskumb", prdskumb);
                }
                HttpContext.Session.SetString("prdpricemb", prdpricemb);

                ViewBag.prdidmb = HttpContext.Session.GetString("prdidmb");
                ViewBag.prdimgmb = HttpContext.Session.GetString("prdimgmb");
                ViewBag.prdnamemb = HttpContext.Session.GetString("prdnamemb");
                ViewBag.prdskumb = HttpContext.Session.GetString("prdskumb");
                ViewBag.prdpricemb = HttpContext.Session.GetString("prdpricemb");
            }

            /* To set Whip Lighting Products from Query String */

            if (prdidwl == null)
            {
                if (HttpContext.Session.GetString("prdpricewl") != null)
                {
                    ViewBag.prdpricewl = HttpContext.Session.GetString("prdpricewl");
                }
                else
                {
                    HttpContext.Session.SetString("prdpricewl", "$0.00");
                }

                if (HttpContext.Session.GetString("prdidwl") != null)
                {
                    ViewBag.prdidwl = HttpContext.Session.GetString("prdidwl");
                    ViewBag.prdimgwl = HttpContext.Session.GetString("prdimgwl");
                    ViewBag.prdnamewl = HttpContext.Session.GetString("prdnamewl");
                    ViewBag.prdskuwl = HttpContext.Session.GetString("prdskuwl");
                }

            }
            else
            {
                SqlConnection conwl = null;
                //conwl = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
                conwl = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
                conwl.Open();
                SqlCommand prdprcflg = new SqlCommand("SELECT Price FROM Product WHERE Id =" + prdidwl, conwl);
                SqlDataReader prcflag = prdprcflg.ExecuteReader();
                prcflag.Read();
                var prdpricequeryold = String.Format("{0:0.00}", prcflag["Price"]);
                prdpricewl = "$" + prdpricequeryold.ToString();

                HttpContext.Session.SetString("prdidwl", prdidwl);
                if (prdimgwl == null)
                {
                    HttpContext.Session.SetString("prdimgwl", " http://localhost:15536/images/thumbs/default-image_550.png");
                }
                else
                {
                    HttpContext.Session.SetString("prdimgwl", prdimgwl);
                }
                HttpContext.Session.SetString("prdnamewl", prdnamewl);
                HttpContext.Session.SetString("prdskuwl", prdskuwl);
                HttpContext.Session.SetString("prdpricewl", prdpricewl);

                ViewBag.prdidwl = HttpContext.Session.GetString("prdidwl");
                ViewBag.prdimgwl = HttpContext.Session.GetString("prdimgwl");
                ViewBag.prdnamewl = HttpContext.Session.GetString("prdnamewl");
                ViewBag.prdskuwl = HttpContext.Session.GetString("prdskuwl");
                ViewBag.prdpricewl = HttpContext.Session.GetString("prdpricewl");
            }

            /* cod for pro setup product load */



            return View();
        }

        /* Action for complete wheep product */

        public IActionResult Prddetil(int id)
        {
            SqlConnection con = null;
            //con = new SqlConnection("data source=.; database=npbuilder; integrated security=SSPI;MultipleActiveResultSets=True");
            con = new SqlConnection(new DataSettingsManager().LoadSettings().DataConnectionString);
            con.Open();

            /* For Flag product from complete wheep  */

            SqlCommand firstflag = new SqlCommand("SELECT ProductId2 FROM RelatedProduct WHERE ProductId1 = " + id + "AND DisplayOrder = 1", con);
            SqlDataReader firstflagreader = firstflag.ExecuteReader();
            firstflagreader.Read();

            SqlCommand prddetailflag = new SqlCommand("SELECT Name,Sku,Price FROM Product WHERE Id = " + firstflagreader["ProductId2"], con);
            SqlDataReader sdrdetailflag = prddetailflag.ExecuteReader();
            sdrdetailflag.Read();

            /* For product image */

            var prdid = firstflagreader["ProductId2"];
            SqlCommand slugcmd = new SqlCommand("SELECT UrlRecord.Slug FROM UrlRecord INNER JOIN Product ON UrlRecord.EntityId = Product.Id WHERE Product.Id =" + prdid + " AND UrlRecord.EntityName = 'Product'", con);
            SqlDataReader slugsdr = slugcmd.ExecuteReader();
            var prdimgquery = new StringBuilder();
            while (slugsdr.Read())
            {
                var seName = slugsdr["Slug"].ToString();
                SqlCommand picdcmd = new SqlCommand("SELECT id FROM Picture where SeoFilename = '" + seName + "'", con);
                SqlDataReader picidsdr = picdcmd.ExecuteReader();
                while (picidsdr.Read())
                {
                    var picture = _pictureService.GetPicturesByProductId(Convert.ToInt32(prdid)).FirstOrDefault();
                    if (picture != null)
                    {
                        seName = slugsdr["Slug"].ToString();
                        var pictureId = picidsdr["id"].ToString();
                        var mimeType = "image/jpeg";

                        prdimgquery.Append(_webHelper.GetStoreLocation());
                        prdimgquery.Append("images/thumbs/");
                        prdimgquery.Append(pictureId.ToString().PadLeft(7, '0'));
                        prdimgquery.Append("_" + seName);
                        prdimgquery.Append("." + mimeType.Replace("image/", ""));
                    }
                }
            }

            /* For price */

            var prdpricequeryold = String.Format("{0:0.00}", sdrdetailflag["Price"]);

            string prdidflag = Convert.ToString(firstflagreader["ProductId2"]);
            string prdimgflag = Convert.ToString(prdimgquery);
            string prdnameflag = Convert.ToString(sdrdetailflag["Name"]);
            string prdskuflag = Convert.ToString(sdrdetailflag["Sku"]);
            string prdpriceflag = prdpricequeryold.ToString();

            if (prdimgflag == null)
            {
                HttpContext.Session.SetString("prdimgflag", " http://localhost:15536/images/thumbs/default-image_550.png");
            }
            else
            {
                HttpContext.Session.SetString("prdimgflag", prdimgflag);
            }
            HttpContext.Session.SetString("prdnameflag", prdnameflag);
            HttpContext.Session.SetString("prdskuflag", prdskuflag);
            if (prdpriceflag != null)
            {
                HttpContext.Session.SetString("prdpriceflag", prdpriceflag);
            }

            /* For flag shaft product from complete wheep */

            SqlCommand firstflagshaft = new SqlCommand("SELECT ProductId2 FROM RelatedProduct WHERE ProductId1 = " + id + "AND DisplayOrder = 2", con);
            SqlDataReader firstflagshaftreader = firstflagshaft.ExecuteReader();
            firstflagshaftreader.Read();

            SqlCommand prddetailflagshaft = new SqlCommand("SELECT Name,Sku,Price FROM Product WHERE Id = " + firstflagshaftreader["ProductId2"], con);
            SqlDataReader sdrdetailflagshaft = prddetailflagshaft.ExecuteReader();
            sdrdetailflagshaft.Read();

            /* For product image */

            var prdidflagshaft = firstflagshaftreader["ProductId2"];
            SqlCommand slugcmdflagshaft = new SqlCommand("SELECT UrlRecord.Slug FROM UrlRecord INNER JOIN Product ON UrlRecord.EntityId = Product.Id WHERE Product.Id =" + prdidflagshaft + " AND UrlRecord.EntityName = 'Product'", con);
            SqlDataReader slugsdrflagshaft = slugcmdflagshaft.ExecuteReader();
            var prdimgqueryflagshaft = new StringBuilder();
            while (slugsdrflagshaft.Read())
            {
                var seName = slugsdrflagshaft["Slug"].ToString();
                SqlCommand picdcmd = new SqlCommand("SELECT id FROM Picture where SeoFilename = '" + seName + "'", con);
                SqlDataReader picidsdr = picdcmd.ExecuteReader();
                while (picidsdr.Read())
                {
                    var picture = _pictureService.GetPicturesByProductId(Convert.ToInt32(prdidflagshaft)).FirstOrDefault();
                    if (picture != null)
                    {
                        seName = slugsdrflagshaft["Slug"].ToString();
                        var pictureId = picidsdr["id"].ToString();
                        var mimeType = "image/jpeg";

                        prdimgqueryflagshaft.Append(_webHelper.GetStoreLocation());
                        prdimgqueryflagshaft.Append("images/thumbs/");
                        prdimgqueryflagshaft.Append(pictureId.ToString().PadLeft(7, '0'));
                        prdimgqueryflagshaft.Append("_" + seName);
                        prdimgqueryflagshaft.Append("." + mimeType.Replace("image/", ""));
                    }
                }
            }

            /* For price */

            var prdpricequeryoldflagshaft = String.Format("{0:0.00}", sdrdetailflagshaft["Price"]);

            prdidflagshaft = Convert.ToString(firstflagshaftreader["ProductId2"]);
            string prdimgflagshaft = Convert.ToString(prdimgqueryflagshaft);
            string prdnameflagshaft = Convert.ToString(sdrdetailflagshaft["Name"]);
            string prdskuflagshaft = Convert.ToString(sdrdetailflagshaft["Sku"]);
            string prdpriceflagshaft = prdpricequeryoldflagshaft.ToString();

            if (prdimgflagshaft == null)
            {
                HttpContext.Session.SetString("prdimgflagshaft", " http://localhost:15536/images/thumbs/default-image_550.png");
            }
            else
            {
                HttpContext.Session.SetString("prdimgflagshaft", prdimgflagshaft);
            }
            HttpContext.Session.SetString("prdnameflagshaft", prdnameflagshaft);
            HttpContext.Session.SetString("prdskuflagshaft", prdskuflagshaft);
            if (prdpriceflagshaft != null)
            {
                HttpContext.Session.SetString("prdpriceflagshaft", prdpriceflagshaft);
            }

            /* For Quick Disconnect product from complete wheep */

            SqlCommand firstqd = new SqlCommand("SELECT ProductId2 FROM RelatedProduct WHERE ProductId1 = " + id + "AND DisplayOrder = 3", con);
            SqlDataReader firstqdreader = firstqd.ExecuteReader();
            firstqdreader.Read();

            SqlCommand prddetailqd = new SqlCommand("SELECT Name,Sku,Price FROM Product WHERE Id = " + firstqdreader["ProductId2"], con);
            SqlDataReader sdrdetailqd = prddetailqd.ExecuteReader();
            sdrdetailqd.Read();

            /* For product image */

            var prdidqd = firstqdreader["ProductId2"];
            SqlCommand slugcmdqd = new SqlCommand("SELECT UrlRecord.Slug FROM UrlRecord INNER JOIN Product ON UrlRecord.EntityId = Product.Id WHERE Product.Id =" + prdidqd + " AND UrlRecord.EntityName = 'Product'", con);
            SqlDataReader slugsdrqd = slugcmdqd.ExecuteReader();
            var prdimgqueryqd = new StringBuilder();
            while (slugsdrqd.Read())
            {
                var seName = slugsdrqd["Slug"].ToString();
                SqlCommand picdcmd = new SqlCommand("SELECT id FROM Picture where SeoFilename = '" + seName + "'", con);
                SqlDataReader picidsdr = picdcmd.ExecuteReader();
                while (picidsdr.Read())
                {
                    var picture = _pictureService.GetPicturesByProductId(Convert.ToInt32(prdidqd)).FirstOrDefault();
                    if (picture != null)
                    {
                        seName = slugsdrqd["Slug"].ToString();
                        var pictureId = picidsdr["id"].ToString();
                        var mimeType = "image/jpeg";

                        prdimgqueryqd.Append(_webHelper.GetStoreLocation());
                        prdimgqueryqd.Append("images/thumbs/");
                        prdimgqueryqd.Append(pictureId.ToString().PadLeft(7, '0'));
                        prdimgqueryqd.Append("_" + seName);
                        prdimgqueryqd.Append("." + mimeType.Replace("image/", ""));
                    }
                }
            }

            /* For price */

            var prdpricequeryoldqd = String.Format("{0:0.00}", sdrdetailqd["Price"]);

            prdidqd = Convert.ToString(firstqdreader["ProductId2"]);
            string prdimgqd = Convert.ToString(prdimgqueryqd);
            string prdnameqd = Convert.ToString(sdrdetailqd["Name"]);
            string prdskuqd = Convert.ToString(sdrdetailqd["Sku"]);
            string prdpriceqd = prdpricequeryoldqd.ToString();

            if (prdimgqd == null)
            {
                HttpContext.Session.SetString("prdimgqd", " http://localhost:15536/images/thumbs/default-image_550.png");
            }
            else
            {
                HttpContext.Session.SetString("prdimgqd", prdimgqd);
            }
            HttpContext.Session.SetString("prdnameqd", prdnameqd);
            HttpContext.Session.SetString("prdskuqd", prdskuqd);
            if (prdpriceqd != null)
            {
                HttpContext.Session.SetString("prdpriceqd", prdpriceqd);
            }

            /* For Mouting Bracket product from complete wheep */

            SqlCommand firstmb = new SqlCommand("SELECT ProductId2 FROM RelatedProduct WHERE ProductId1 = " + id + "AND DisplayOrder = 4", con);
            SqlDataReader firstmbreader = firstmb.ExecuteReader();
            firstmbreader.Read();

            SqlCommand prddetailmb = new SqlCommand("SELECT Name,Sku,Price FROM Product WHERE Id = " + firstmbreader["ProductId2"], con);
            SqlDataReader sdrdetailmb = prddetailmb.ExecuteReader();
            sdrdetailmb.Read();

            /* For product image */

            var prdidmb = firstmbreader["ProductId2"];
            SqlCommand slugcmdmb = new SqlCommand("SELECT UrlRecord.Slug FROM UrlRecord INNER JOIN Product ON UrlRecord.EntityId = Product.Id WHERE Product.Id =" + prdidmb + " AND UrlRecord.EntityName = 'Product'", con);
            SqlDataReader slugsdrmb = slugcmdmb.ExecuteReader();
            var prdimgquerymb = new StringBuilder();
            while (slugsdrmb.Read())
            {
                var seName = slugsdrmb["Slug"].ToString();
                SqlCommand picdcmd = new SqlCommand("SELECT id FROM Picture where SeoFilename = '" + seName + "'", con);
                SqlDataReader picidsdr = picdcmd.ExecuteReader();
                while (picidsdr.Read())
                {
                    var picture = _pictureService.GetPicturesByProductId(Convert.ToInt32(prdidmb)).FirstOrDefault();
                    if (picture != null)
                    {
                        seName = slugsdrmb["Slug"].ToString();
                        var pictureId = picidsdr["id"].ToString();
                        var mimeType = "image/jpeg";

                        prdimgquerymb.Append(_webHelper.GetStoreLocation());
                        prdimgquerymb.Append("images/thumbs/");
                        prdimgquerymb.Append(pictureId.ToString().PadLeft(7, '0'));
                        prdimgquerymb.Append("_" + seName);
                        prdimgquerymb.Append("." + mimeType.Replace("image/", ""));
                    }
                }
            }

            /* For price */

            var prdpricequeryoldmb = String.Format("{0:0.00}", sdrdetailmb["Price"]);

            prdidmb = Convert.ToString(firstmbreader["ProductId2"]);
            string prdimgmb = Convert.ToString(prdimgquerymb);
            string prdnamemb = Convert.ToString(sdrdetailmb["Name"]);
            string prdskumb = Convert.ToString(sdrdetailmb["Sku"]);
            string prdpricemb = prdpricequeryoldmb.ToString();

            if (prdimgmb == null)
            {
                HttpContext.Session.SetString("prdimgmb", " http://localhost:15536/images/thumbs/default-image_550.png");
            }
            else
            {
                HttpContext.Session.SetString("prdimgmb", prdimgmb);
            }
            HttpContext.Session.SetString("prdnamemb", prdnamemb);
            HttpContext.Session.SetString("prdskumb", prdskumb);
            if (prdpricemb != null)
            {
                HttpContext.Session.SetString("prdpricemb", prdpricemb);
            }

            /* For Whip Lighting product from complete wheep */

            SqlCommand firstwl = new SqlCommand("SELECT ProductId2 FROM RelatedProduct WHERE ProductId1 = " + id + "AND DisplayOrder = 5", con);
            SqlDataReader firstwlreader = firstwl.ExecuteReader();
            firstwlreader.Read();

            SqlCommand prddetailwl = new SqlCommand("SELECT Name,Sku,Price FROM Product WHERE Id = " + firstwlreader["ProductId2"], con);
            SqlDataReader sdrdetailwl = prddetailwl.ExecuteReader();
            sdrdetailwl.Read();

            /* For product image */

            var prdidwl = firstwlreader["ProductId2"];
            SqlCommand slugcmdwl = new SqlCommand("SELECT UrlRecord.Slug FROM UrlRecord INNER JOIN Product ON UrlRecord.EntityId = Product.Id WHERE Product.Id =" + prdidwl + " AND UrlRecord.EntityName = 'Product'", con);
            SqlDataReader slugsdrwl = slugcmdwl.ExecuteReader();
            var prdimgquerywl = new StringBuilder();
            while (slugsdrwl.Read())
            {
                var seName = slugsdrwl["Slug"].ToString();
                SqlCommand picdcmd = new SqlCommand("SELECT id FROM Picture where SeoFilename = '" + seName + "'", con);
                SqlDataReader picidsdr = picdcmd.ExecuteReader();
                while (picidsdr.Read())
                {
                    var picture = _pictureService.GetPicturesByProductId(Convert.ToInt32(prdidwl)).FirstOrDefault();
                    if (picture != null)
                    {
                        seName = slugsdrwl["Slug"].ToString();
                        var pictureId = picidsdr["id"].ToString();
                        var mimeType = "image/jpeg";

                        prdimgquerywl.Append(_webHelper.GetStoreLocation());
                        prdimgquerywl.Append("images/thumbs/");
                        prdimgquerywl.Append(pictureId.ToString().PadLeft(7, '0'));
                        prdimgquerywl.Append("_" + seName);
                        prdimgquerywl.Append("." + mimeType.Replace("image/", ""));
                    }
                }
            }

            /* For price */

            var prdpricequeryoldwl = String.Format("{0:0.00}", sdrdetailwl["Price"]);

            prdidwl = Convert.ToString(firstwlreader["ProductId2"]);
            string prdimgwl = Convert.ToString(prdimgquerywl);
            string prdnamewl = Convert.ToString(sdrdetailwl["Name"]);
            string prdskuwl = Convert.ToString(sdrdetailwl["Sku"]);
            string prdpricewl = prdpricequeryoldwl.ToString();

            if (prdimgwl == null)
            {
                HttpContext.Session.SetString("prdimgwl", " http://localhost:15536/images/thumbs/default-image_550.png");
            }
            else
            {
                HttpContext.Session.SetString("prdimgwl", prdimgwl);
            }
            HttpContext.Session.SetString("prdnamewl", prdnamewl);
            HttpContext.Session.SetString("prdskuwl", prdskuwl);
            if (prdpricewl != null)
            {
                HttpContext.Session.SetString("prdpricewl", prdpricewl);
            }
            

            return RedirectToAction("Index", "CustomBuilder", new
            {
                prdidflag = prdidflag, prdimgflag = prdimgflag, prdnameflag = prdnameflag, prdskuflag = prdskuflag, prdpriceflag = prdpriceflag,
                prdidflagshaft = prdidflagshaft, prdimgflagshaft = prdimgflagshaft, prdnameflagshaft = prdnameflagshaft, prdskuflagshaft = prdskuflagshaft, prdpriceflagshaft = prdpriceflagshaft,
                prdidqd = prdidqd, prdimgqd = prdimgqd, prdnameqd = prdnameqd, prdskuqd = prdskuqd, prdpriceqd = prdpriceqd,
                prdidmb = prdidmb, prdimgmb = prdimgmb, prdnamemb = prdnamemb, prdskumb = prdskumb, prdpricemb = prdpricemb,
                prdidwl = prdidwl, prdimgwl = prdimgwl, prdnamewl = prdnamewl, prdskuwl = prdskuwl, prdpricewl = prdpricewl,
            });
        }


        public IActionResult MainPage()
        {
            return View();
        }

        public IActionResult Flag()
        {
            if (HttpContext.Session.GetString("prdidflag") != null)
            {
                HttpContext.Session.Remove("prdidflag");
                HttpContext.Session.Remove("prdimgflag");
                HttpContext.Session.Remove("prdnameflag");
                HttpContext.Session.Remove("prdskuflag");
                HttpContext.Session.SetString("prdpriceflag", "$0.00");
            }
            return RedirectToAction("Index", "CustomBuilder");
        }

        public IActionResult FlagShaft()
        {
            if (HttpContext.Session.GetString("prdidflagshaft") != null)
            {
                HttpContext.Session.Remove("prdidflagshaft");
                HttpContext.Session.Remove("prdimgflagshaft");
                HttpContext.Session.Remove("prdnameflagshaft");
                HttpContext.Session.Remove("prdskuflagshaft");
                HttpContext.Session.SetString("prdpriceflagshaft", "$0.00");
            }
            return RedirectToAction("Index", "CustomBuilder");
        }

        public IActionResult Quickdis()
        {
            if (HttpContext.Session.GetString("prdidqd") != null)
            {
                HttpContext.Session.Remove("prdidqd");
                HttpContext.Session.Remove("prdimgqd");
                HttpContext.Session.Remove("prdnameqd");
                HttpContext.Session.Remove("prdskuqd");
                HttpContext.Session.SetString("prdpriceqd", "$0.00");
            }
            return RedirectToAction("Index", "CustomBuilder");
        }

        public IActionResult Mountingbr()
        {
            if (HttpContext.Session.GetString("prdidmb") != null)
            {
                HttpContext.Session.Remove("prdidmb");
                HttpContext.Session.Remove("prdimgmb");
                HttpContext.Session.Remove("prdnamemb");
                HttpContext.Session.Remove("prdskumb");
                HttpContext.Session.SetString("prdpricemb", "$0.00");
            }
            return RedirectToAction("Index", "CustomBuilder");
        }

        public IActionResult Whiplight()
        {
            if (HttpContext.Session.GetString("prdidwl") != null)
            {
                HttpContext.Session.Remove("prdidwl");
                HttpContext.Session.Remove("prdimgwl");
                HttpContext.Session.Remove("prdnamewl");
                HttpContext.Session.Remove("prdskuwl");
                HttpContext.Session.SetString("prdpricewl", "$0.00");
            }

            return RedirectToAction("Index", "CustomBuilder");
        }

        /* Action to add product in admin as custom wheep */

        public IActionResult SaveCustomWheepAdmin()
        {
            con.Open();

            SqlCommand lastnameprdcmd = new SqlCommand("SELECT max(Id) AS newnameid FROM Product", con);
            SqlDataReader lastnameprdsdr = lastnameprdcmd.ExecuteReader();
            lastnameprdsdr.Read();
            var lastprdname = "CustomWhip" + lastnameprdsdr["newnameid"];


            String Mainproductqry = "INSERT INTO Product (ProductTypeId ,ParentGroupedProductId ,VisibleIndividually ,Name ,ShortDescription ,FullDescription," +
                "AdminComment, ProductTemplateId, VendorId, ShowOnHomePage, MetaKeywords, MetaDescription, MetaTitle, AllowCustomerReviews," +
                "ApprovedRatingSum, NotApprovedRatingSum, ApprovedTotalReviews, NotApprovedTotalReviews, SubjectToAcl, LimitedToStores," +
                "Sku, ManufacturerPartNumber, Gtin, IsGiftCard, GiftCardTypeId, OverriddenGiftCardAmount, RequireOtherProducts, RequiredProductIds," +
                "AutomaticallyAddRequiredProducts, IsDownload, DownloadId, UnlimitedDownloads, MaxNumberOfDownloads, DownloadExpirationDays," +
                "DownloadActivationTypeId, HasSampleDownload, SampleDownloadId, HasUserAgreement, UserAgreementText, IsRecurring, RecurringCycleLength," +
                "RecurringCyclePeriodId, RecurringTotalCycles, IsRental, RentalPriceLength, RentalPricePeriodId, IsShipEnabled, IsFreeShipping," +
                "ShipSeparately, AdditionalShippingCharge, DeliveryDateId, IsTaxExempt, TaxCategoryId, IsTelecommunicationsOrBroadcastingOrElectronicServices," +
                "ManageInventoryMethodId, ProductAvailabilityRangeId, UseMultipleWarehouses, WarehouseId, StockQuantity, DisplayStockAvailability," +
                "DisplayStockQuantity, MinStockQuantity, LowStockActivityId, NotifyAdminForQuantityBelow, BackorderModeId, AllowBackInStockSubscriptions," +
                "OrderMinimumQuantity, OrderMaximumQuantity, AllowedQuantities, AllowAddingOnlyExistingAttributeCombinations, NotReturnable," +
                "DisableBuyButton, DisableWishlistButton, AvailableForPreOrder, PreOrderAvailabilityStartDateTimeUtc, CallForPrice, Price," +
                "OldPrice, ProductCost, CustomerEntersPrice, MinimumCustomerEnteredPrice, MaximumCustomerEnteredPrice, BasepriceEnabled," +
                "BasepriceAmount, BasepriceUnitId, BasepriceBaseAmount, BasepriceBaseUnitId, MarkAsNew, MarkAsNewStartDateTimeUtc, MarkAsNewEndDateTimeUtc," +
                "HasTierPrices, HasDiscountsApplied, Weight, Length, Width, Height, AvailableStartDateTimeUtc, AvailableEndDateTimeUtc," +
                "DisplayOrder, Published, Deleted, CreatedOnUtc, UpdatedOnUtc) VALUES (5, 0, 1, '"+ lastprdname + "', NULL, NULL, NULL, 1, 0, 0, NULL, NULL, NULL," +
                "1, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, 0, NULL, 0, NULL, 0, 0, 0, 1, 10, NULL, 0, 0, 0, 0, NULL, 0, 100, 0, 10, 0, 1, 0, 1, 0, 0, 0.0000, 0, 0, 0, 0, 1," +
                " 0, 0, 0, 10000,0, 0, 0, 0, 1, 0, 0, 1, 10000, NULL, 0, 0, 0, 0, 0, NULL, 0, 0.0000, 0.0000, 0.0000, 0, 0.0000, 1000.0000, " +
                "0, 0.0000, 1, 0, 1, 0, NULL, NULL, 0, 0, 0.0000, 0.0000, 0.0000, 0.0000, NULL, NULL, 0, 1, 0, '"+ DateTime.UtcNow + "', '"+ DateTime.UtcNow +"')";

            SqlCommand Mainproductqrycmd = new SqlCommand(Mainproductqry, con);
            Mainproductqrycmd.ExecuteNonQuery();

            SqlCommand lastidcmd = new SqlCommand("SELECT max(Id) AS newid FROM Product", con);
            SqlDataReader lastidsdr = lastidcmd.ExecuteReader();
            lastidsdr.Read();
            var lastprdid = lastidsdr["newid"];

            var flagid = HttpContext.Session.GetString("prdidflag");
            var flagshaftid = HttpContext.Session.GetString("prdidflagshaft");
            var qdid = HttpContext.Session.GetString("prdidqd");
            var mbid = HttpContext.Session.GetString("prdidmb");
            var wlid =  HttpContext.Session.GetString("prdidwl");

            String flagqry = "INSERT INTO RelatedProduct (ProductId1, ProductId2, DisplayOrder) VALUES ('" + lastprdid + "', '" + flagid + "', 1)";
            SqlCommand flagqrycmd = new SqlCommand(flagqry, con);
            flagqrycmd.ExecuteNonQuery();

            String flagshaftqry = "INSERT INTO RelatedProduct (ProductId1, ProductId2, DisplayOrder) VALUES ('" + lastprdid + "', '" + flagshaftid + "', 2)";
            SqlCommand flagshaftqrycmd = new SqlCommand(flagshaftqry, con);
            flagshaftqrycmd.ExecuteNonQuery();

            String qdqry = "INSERT INTO RelatedProduct (ProductId1, ProductId2, DisplayOrder) VALUES ('" + lastprdid + "', '" + qdid + "', 3)";
            SqlCommand qdqrycmd = new SqlCommand(qdqry, con);
            qdqrycmd.ExecuteNonQuery();

            String mbqry = "INSERT INTO RelatedProduct (ProductId1, ProductId2, DisplayOrder) VALUES ('" + lastprdid + "', '" + mbid + "', 4)";
            SqlCommand mbqrycmd = new SqlCommand(mbqry, con);
            mbqrycmd.ExecuteNonQuery();

            String wlqry = "INSERT INTO RelatedProduct (ProductId1, ProductId2, DisplayOrder) VALUES ('" + lastprdid + "', '" + wlid + "', 5)";
            SqlCommand wlqrycmd = new SqlCommand(wlqry, con);
            wlqrycmd.ExecuteNonQuery();

            String categoryqry = "INSERT INTO Product_Category_Mapping(ProductId, CategoryId, IsFeaturedProduct, DisplayOrder) VALUES ('"+ lastprdid +"', 59, 0, 1)";
            SqlCommand categoryqrycmd = new SqlCommand(categoryqry, con);
            categoryqrycmd.ExecuteNonQuery();

            String urlrecordqry = "INSERT INTO UrlRecord(EntityId, EntityName, Slug, IsActive, LanguageId) VALUES ('"+ lastprdid +"', 'Product', '"+ lastprdname+"', 1, 0)";
            SqlCommand urlrecordqrycmd = new SqlCommand(urlrecordqry, con);
            urlrecordqrycmd.ExecuteNonQuery();

            /* Test builder empty message display and same things for that two other buttons */

            return RedirectToAction("ClearBuilderAdminProduct", "CustomBuilder");
        }

        /* Action to make builder clear once product added in admin side */

        public IActionResult ClearBuilderAdminProduct()
        {
            if (HttpContext.Session.GetString("prdidflag") != null)
            {
                HttpContext.Session.Remove("prdidflag");
                HttpContext.Session.Remove("prdimgflag");
                HttpContext.Session.Remove("prdnameflag");
                HttpContext.Session.Remove("prdskuflag");
                HttpContext.Session.SetString("prdpriceflag", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidflagshaft") != null)
            {
                HttpContext.Session.Remove("prdidflagshaft");
                HttpContext.Session.Remove("prdimgflagshaft");
                HttpContext.Session.Remove("prdnameflagshaft");
                HttpContext.Session.Remove("prdskuflagshaft");
                HttpContext.Session.SetString("prdpriceflagshaft", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidqd") != null)
            {
                HttpContext.Session.Remove("prdidqd");
                HttpContext.Session.Remove("prdimgqd");
                HttpContext.Session.Remove("prdnameqd");
                HttpContext.Session.Remove("prdskuqd");
                HttpContext.Session.SetString("prdpriceqd", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidmb") != null)
            {
                HttpContext.Session.Remove("prdidmb");
                HttpContext.Session.Remove("prdimgmb");
                HttpContext.Session.Remove("prdnamemb");
                HttpContext.Session.Remove("prdskumb");
                HttpContext.Session.SetString("prdpricemb", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidwl") != null)
            {
                HttpContext.Session.Remove("prdidwl");
                HttpContext.Session.Remove("prdimgwl");
                HttpContext.Session.Remove("prdnamewl");
                HttpContext.Session.Remove("prdskuwl");
                HttpContext.Session.SetString("prdpricewl", "$0.00");
            }

            return RedirectToAction("Index", "CustomBuilder", new { usern = "admin" });
        }

        public IActionResult ClearBuilder()
        {
            if (HttpContext.Session.GetString("prdidflag") != null)
            {
                HttpContext.Session.Remove("prdidflag");
                HttpContext.Session.Remove("prdimgflag");
                HttpContext.Session.Remove("prdnameflag");
                HttpContext.Session.Remove("prdskuflag");
                HttpContext.Session.SetString("prdpriceflag", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidflagshaft") != null)
            {
                HttpContext.Session.Remove("prdidflagshaft");
                HttpContext.Session.Remove("prdimgflagshaft");
                HttpContext.Session.Remove("prdnameflagshaft");
                HttpContext.Session.Remove("prdskuflagshaft");
                HttpContext.Session.SetString("prdpriceflagshaft", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidqd") != null)
            {
                HttpContext.Session.Remove("prdidqd");
                HttpContext.Session.Remove("prdimgqd");
                HttpContext.Session.Remove("prdnameqd");
                HttpContext.Session.Remove("prdskuqd");
                HttpContext.Session.SetString("prdpriceqd", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidmb") != null)
            {
                HttpContext.Session.Remove("prdidmb");
                HttpContext.Session.Remove("prdimgmb");
                HttpContext.Session.Remove("prdnamemb");
                HttpContext.Session.Remove("prdskumb");
                HttpContext.Session.SetString("prdpricemb", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidwl") != null)
            {
                HttpContext.Session.Remove("prdidwl");
                HttpContext.Session.Remove("prdimgwl");
                HttpContext.Session.Remove("prdnamewl");
                HttpContext.Session.Remove("prdskuwl");
                HttpContext.Session.SetString("prdpricewl", "$0.00");
            }

            return RedirectToAction("Index", "CustomBuilder");
        }

        /* Action to remove data from session when user add products to cart */

        public IActionResult ClearBuilderCart()
        {
            if (HttpContext.Session.GetString("prdidflag") != null)
            {
                HttpContext.Session.Remove("prdidflag");
                HttpContext.Session.Remove("prdimgflag");
                HttpContext.Session.Remove("prdnameflag");
                HttpContext.Session.Remove("prdskuflag");
                HttpContext.Session.SetString("prdpriceflag", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidflagshaft") != null)
            {
                HttpContext.Session.Remove("prdidflagshaft");
                HttpContext.Session.Remove("prdimgflagshaft");
                HttpContext.Session.Remove("prdnameflagshaft");
                HttpContext.Session.Remove("prdskuflagshaft");
                HttpContext.Session.SetString("prdpriceflagshaft", "$0.00");
            }

            if (HttpContext.Session.GetString("prdidqd") != null)
            {
                HttpContext.Session.Remove("prdidqd");
                HttpContext.Session.Remove("prdimgqd");
                HttpContext.Session.Remove("prdnameqd");
                HttpContext.Session.Remove("prdskuqd");
                HttpContext.Session.SetString("prdpriceqd", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidmb") != null)
            {
                HttpContext.Session.Remove("prdidmb");
                HttpContext.Session.Remove("prdimgmb");
                HttpContext.Session.Remove("prdnamemb");
                HttpContext.Session.Remove("prdskumb");
                HttpContext.Session.SetString("prdpricemb", "$0.00");
            }
            if (HttpContext.Session.GetString("prdidwl") != null)
            {
                HttpContext.Session.Remove("prdidwl");
                HttpContext.Session.Remove("prdimgwl");
                HttpContext.Session.Remove("prdnamewl");
                HttpContext.Session.Remove("prdskuwl");
                HttpContext.Session.SetString("prdpricewl", "$0.00");
            }

            //return RedirectToAction("", "cart");
            //return Url.RouteUrl("ShoppingCart");
            return RedirectToRoute("ShoppingCart");
        }
    }
}