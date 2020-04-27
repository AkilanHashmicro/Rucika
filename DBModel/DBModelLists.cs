using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SalesApp.models;
using SQLite;
using Xamarin.Forms;

namespace SalesApp.DBModel
{
    public class DBModelLists
    {

    }

    [Table("UserModelDB")]
    public class UserModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public int userid { get; set; }
        public int partnerid { get; set; }
        public string user_name { get; set; }
        public string user_image_medium { get; set; }
        public string customers_list { get; set; }
        public string state_list { get; set; }
        public string country_list { get; set; }
        public string stages { get; set; }
        public string sales_team { get; set; }
        public string sales_persons { get; set; }
        public string products { get; set; }
        public string user_email { get; set; }
        public string tagsPicker { get; set; }
        public string next_activity { get; set; }

        public string tax_list { get; set; }

        public string payment_terms { get; set; }

        public string commission_group { get; set; }

        public string warehouse_list { get; set; }

    }


    [Table("CRMLeadDB")]
    public class CRMLeadDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        string tempColours = Settings.StageColourCode;
        Dictionary<string, string> stageColours = new Dictionary<string, string>();
        public int id { get; set; }
        public string customer { get; set; }
        public string next_activity { get; set; }
        public string name { get; set; }
        public int probability { get; set; }
        public string phone { get; set; }
        //public object[] tags { get; set; }
        public string title_action { get; set; }
        public string expected_revenue { get; set; }
        public string expected_closing { get; set; }
        public string team_name { get; set; }
        public int priority { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contact_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }

        public string pipe_date { get; set; }

        public string pipe_date1 { get; set; }

        public DateTime conDate1 { get; set; }

        public DateTime conDate { get; set; }

        public string DateOrder { get; set; }

        public string FullState { get; set; }

        public string state_colour { get; set; }


        // starts for crm lead rpc

        public string function { get; set; }
        public int user_id { get; set; }
        public int team_id { get; set; }
        public int state_id { get; set; }
        public int country_id { get; set; }
        public int nextact_id { get; set; }
        public string description { get; set; }
        public string type { get; set; }

        public string serverupdate_string { get; set; }
        public string zip { get; set; }
        public string yellowimg_string { get; set; }


        public string server_color { get; set; }

    }


    [Table("SalesOrderDB")]
    public class SalesOrderDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public string customer { get; set; }
        public string next_activity { get; set; }
        public string name { get; set; }
        public int probability { get; set; }
        public string phone { get; set; }

        public string title_action { get; set; }
        public string expected_revenue { get; set; }

        public string payment_term { get; set; }

        public string team_name { get; set; }
        public int priority { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contact_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string state_colour { get; set; }
        public string sales_person { get; set; }
        public string sales_team { get; set; }
        public string customer_reference { get; set; }
        public string fiscal_position { get; set; }
        //public string sales_team { get; set; }

        //public string _state1;
        public string FullState { get; set; }


        //[JsonProperty("date_order")]
        public string DateOrder { get; set; }


        public string customer_lead { get; set; }
        public string price_unit { get; set; }
        public string product_uom_qty { get; set; }
        public string price_subtotal { get; set; }
        public String taxes { get; set; }
        public string product_name { get; set; }

        public string order_line { get; set; }

       public string ColorCode { get; set; }

        //public List<OrderLineDB> order_line { get; set; }



    }




    [Table("SalesQuotationDB")]
    public class SalesQuotationDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public int id { get; set; }
        public string customer { get; set; }
        public string next_activity { get; set; }
        public string name { get; set; }
        public int probability { get; set; }
        public string phone { get; set; }

        public string title_action { get; set; }
        public string expected_revenue { get; set; }

        public string payment_term { get; set; }
        public string commission_group { get; set; }
        public int payment_term_id { get; set; }

        public int commission_group_id { get; set; }

        public string team_name { get; set; }
        public int priority { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contact_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string state_colour { get; set; }
        public string sales_person { get; set; }
        public string sales_team { get; set; }
        public string customer_reference { get; set; }
        public string fiscal_position { get; set; }
        // public string sales_team { get; set; }

        //public string _state1;
        public string FullState { get; set; }

        //[JsonProperty("date_order")]

        public string date_Order { get; set; }

          public string DateOrder { get; set; }


        //public string order_line { get; set; }

        public string order_line { get; set; }

        public string order_date { get; set; }
        public string expiration_date { get; set; }
        public int payment_terms { get; set; }
        public int user_id { get; set; }
        public int customer_id { get; set; }

        public string yellowimg_string { get; set; }
        //   public string ColorCode { get; set; }

        //public string DateOrder
        //{
        //    get
        //    {
        //        DateTime dt = Convert.ToDateTime(date_Order.ToString());
        //        // DateTime dt = DateTime.ParseExact(dateOrder, "yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture);
        //        date_Order = dt.ToString("d MMM");
        //        return date_Order;
        //    }

        //    set
        //    {
        //        date_Order = value;
        //    }
        //}


    }



    [Table("CRMOpportunitiesDB")]
    public class CRMOpportunitiesDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }
        public string customer { get; set; }
        public string next_activity { get; set; }
        public string name { get; set; }
        public int probability { get; set; }
        public string phone { get; set; }
        //public object[] tags { get; set; }
        public string title_action { get; set; }
        public string expected_revenue { get; set; }
        public string expected_closing { get; set; }
        public string team_name { get; set; }
        public string priority { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string contact_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }

        //public string _state1;
        public string FullState { get; set; }


        public string state_colour { get; set; }

        public string pipe_date { get; set; }

        //public DateTime pipe_datetime1 { get; set; }

        //public DateTime pipe_datetime { get; set; }


        public string pipe_datetime1 { get; set; }

        public string pipe_datetime { get; set; }


        public string newpipe_date { get; set; }

        public string DateOrder { get; set; }

        public int cusid { get; set; }
        public string next_activity_date { get; set; }
        public string rating { get; set; }
        public string stage { get; set; }
        public string internal_notes { get; set; }
        public string meetings { get; set; }

        //starts for opportunities

        public string oppurtunity_title { get; set; }
        public string next_activity_summary { get; set; }
        public string yellowimg_string { get; set; }
        public int nextact_id { get; set; }
    }


    [Table("CustomersModelDB")]
    public class CustomersModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public string name { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public int crm_count { get; set; }
        public int sales_count { get; set; }
        public int meeting_count { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string zip { get; set; }

        public int id { get; set; }

        //    public string image_medium { get; set; }
        public string image_small { get; set; }

        public string website { get; set; }

        public Dictionary<int, string> tags { get; set; }

        public ImageSource imagetest { get; set; }

        public string contacts { get; set; }

        //   public List<ContactsList> contacts { get; set; }

        public ImageSource CustomerImg
        {
            get
            {
                if (image_small != "")
                {
                    byte[] imageAsBytes = Encoding.UTF8.GetBytes(image_small);
                    byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                    imagetest = ImageSource.FromStream(() => new MemoryStream(decodedByteArray));
                    // imagetest = ImageSource.FromStream(() => stream);
                    return imagetest;
                }
                else
                {
                    imagetest = "unknown.png";
                    return imagetest;
                }

            }
            set
            {
                // image = value;
            }
        }

    }



    //public class OrderLineDB
    //{
    //    // [JsonProperty("customer_lead")]
    //    public string customer_lead { get; set; }

    //    // [JsonProperty("price_unit")]
    //    public string price_unit { get; set; }

    //    // [JsonProperty("product_uom_qty")]
    //    public string product_uom_qty { get; set; }

    //    // [JsonProperty("price_subtotal")]
    //    public string price_subtotal { get; set; }

    //    // [JsonProperty("taxes")]
    //    public object[] taxes { get; set; }

    //    //[JsonProperty("product_name")]
    //    public string product_name { get; set; }


    //    public string description { get; set; }

    //}



}