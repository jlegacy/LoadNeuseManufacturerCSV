using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using LINQtoCSV;

namespace LoadNeuseCSV
{
    internal class ProductCSV
    {
        [CsvColumn(Name = "brandName", FieldIndex = 1)]
        public string BrandName { get; set; }

        [CsvColumn(Name = "modelName", FieldIndex = 2)]
        public string ModelName { get; set; }

        [CsvColumn(Name = "modelYear", FieldIndex = 3)]
        public string ModelYear { get; set; }

        [CsvColumn(Name = "modelSKU", FieldIndex = 4)]
        public string ModelSku { get; set; }

        [CsvColumn(Name = "modelDescription", FieldIndex = 5)]
        public string ModelDescription { get; set; }

        [CsvColumn(Name = "modelImage", FieldIndex = 6)]
        public string NodelImage { get; set; }

        [CsvColumn(Name = "imageCaption", FieldIndex = 7)]
        public string ImageCaption { get; set; }

        [CsvColumn(Name = "gender", FieldIndex = 8)]
        public string Gender { get; set; }

        [CsvColumn(Name = "categoryID", FieldIndex = 9)]
        public int CategoryID { get; set; }

        [CsvColumn(Name = "sku", FieldIndex = 10)]
        public string Sku { get; set; }

        [CsvColumn(Name = "mpn", FieldIndex = 11)]
        public string mpn { get; set; }

        [CsvColumn(Name = "gtin1", FieldIndex = 12)]
        public string Gtin1 { get; set; }

        [CsvColumn(Name = "gtin2", FieldIndex = 13)]
        public string Gtin2 { get; set; }

        [CsvColumn(Name = "msrp", FieldIndex = 14)]
        public double Msrp { get; set; }

        [CsvColumn(Name = "dealerCost", FieldIndex = 15)]
        public double DealerCost { get; set; }

        [CsvColumn(Name = "specialCost", FieldIndex = 16)]
        public string SpecialCost { get; set; }

        [CsvColumn(Name = "lowMsrp", FieldIndex = 17)]
        public double LowMsrp { get; set; }

        [CsvColumn(Name = "length", FieldIndex = 18)]
        public string Length { get; set; }

        [CsvColumn(Name = "width", FieldIndex = 19)]
        public string Width { get; set; }

        [CsvColumn(Name = "height", FieldIndex = 20)]
        public string Height { get; set; }

        [CsvColumn(Name = "weight", FieldIndex = 21)]
        public double Weight { get; set; }

        [CsvColumn(Name = "image", FieldIndex = 22)]
        public string Image { get; set; }

        [CsvColumn(Name = "unit", FieldIndex = 23)]
        public string Unit { get; set; }

        [CsvColumn(Name = "hazmatCode", FieldIndex = 24)]
        public string HazmatCode { get; set; }

        [CsvColumn(Name = "taxable", FieldIndex = 25)]
        public string Taxable { get; set; }

        [CsvColumn(Name = "shippable", FieldIndex = 26)]
        public string Shippable { get; set; }

        [CsvColumn(Name = "shipGround", FieldIndex = 27)]
        public string ShipGround { get; set; }

        [CsvColumn(Name = "shipAir", FieldIndex = 28)]
        public string ShipAir { get; set; }

        [CsvColumn(Name = "ormd", FieldIndex = 29)]
        public string Ormd { get; set; }

        [CsvColumn(Name = "FFLrequired", FieldIndex = 30)]
        public string FfLrequired { get; set; }

        [CsvColumn(Name = "NFArequired", FieldIndex = 31)]
        public string NfArequired { get; set; }

        [CsvColumn(Name = "variHash", FieldIndex = 32)]
        public string VariHash { get; set; }

        [CsvColumn(Name = "name", FieldIndex = 33)]
        public string Name { get; set; }

        [CsvColumn(Name = "id", FieldIndex = 34)]
        public string Id { get; set; }

        [CsvColumn(Name = "text", FieldIndex = 35)]
        public string Text { get; set; }
    }

    internal class Program
    {
        public static int scIdStart = 190; 

        private static void Main(string[] args)
        {
            
            var fd = new CsvFileDescription();
            fd.SeparatorChar = ',';
            fd.FirstLineHasColumnNames = true;
            fd.TextEncoding = Encoding.Default;

            var cc = new CsvContext();

            IEnumerable<ProductCSV> products =
                cc.Read<ProductCSV>("C:\\Users\\jlegacy\\Desktop\\NeuseCSV.csv", fd);

// Data is now available via variable products.

            IEnumerable<ProductCSV> productsByName =
                from p in products
                select p;
// or ...
            foreach (ProductCSV item in productsByName)
            {
              
                    //Data maping object to our database
                var text = new searchCriteriaDataContext();
                var mySearchCriteria = new searchcriteria();
                var dc = new searchCriteriaDataContext();
                    
                    IQueryable<searchcriteria> q =
                        from a in dc.GetTable<searchcriteria>()
                        where ((a.scWorkingName.ToLower()).CompareTo(item.BrandName.ToLower()) == 0)
                        orderby a.scWorkingName
                        select a;
                    if (q.Any())
                    {
                        foreach (searchcriteria x in q)
                        {
                            buildData(x, item);
                            dc.SubmitChanges();
                           
                        }
                    }
                    else
                    {
                        text.searchcriterias.InsertOnSubmit(mySearchCriteria);
                        buildData(mySearchCriteria, item);
                   //     text.searchcriterias.Context.ExecuteCommand("SET IDENTITY_INSERT searchcriteria ON");
                        text.SubmitChanges();
                    }
                  
                }

            
            }
        
        private static void buildData(searchcriteria mySearchcriteria, ProductCSV item)
        {
         //   ++scIdStart;
         //   mySearchcriteria.scID = scIdStart;
            mySearchcriteria.scWorkingName = item.BrandName;
            mySearchcriteria.scName = item.BrandName;
            mySearchcriteria.scName2 = item.BrandName;
            mySearchcriteria.scName3 = item.BrandName;
            mySearchcriteria.scLogo = "";
            mySearchcriteria.scURL = "";
            mySearchcriteria.scURL2 = "";
            mySearchcriteria.scURL3 = "";
            mySearchcriteria.scEmail = "";
            mySearchcriteria.scGroup = 0;
        }

        public static string HtmlEncode(string text)
        {
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            var result = new StringBuilder(text.Length + (int) (text.Length*0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

    }
}