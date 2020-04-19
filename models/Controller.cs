using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SalesApp.OdooRpc;
using static SalesApp.models.CRMModel;
using Xamarin.Forms;
using SalesApp.Persistance;
using SalesApp.DBModel;
using static SalesApp.DBModel.SalesOrderDB;

namespace SalesApp.models
{
    class Controller
    {
        private static Controller instance = null;

        private static readonly object padlock = new object();
        private OdooRPC odooConnector;

        private Controller()
        {
        }

        public string[] getDatabases(string url)
        {
            try
            {
                OdooRPC con = OdooRPC.InstanceCreation(url);
                return con.getDatabases();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            String[] st = new string[0];
            return st;
        }

        public String login(String url, String database, String username, String password)
        {
            try
            {
                odooConnector = OdooRPC.InstanceCreation(url);
                App.userid = odooConnector.login(database, username, password);
                //object[] domain = new object[] { "id", "=", App.userid };
                //JArray userData = odooConnector.odooSearchReadCall<JArray>("res.users", domain, new string[] { "name", "email", "partner_id","groups_id","image_medium" }, false);

                //string partnerName="", partnerRole="", partnerImage="";
                //int partnerId=0;
                //foreach (JObject obj in userData)
                //{                        
                //    partnerId = obj["partner_id"][0].ToObject<int>();
                //    partnerName = obj["name"].ToString();
                //    partnerRole = "Manager";
                //    partnerImage = obj["image_medium"].ToString();                    
                //}
                //Settings.PrefKeyIsLocked = "True";
                //UserAccount data = new UserAccount(url, database, App.userid,partnerId,partnerName,password,partnerImage,partnerRole);                
                //Settings.PrefKeyUserDetails = JsonConvert.SerializeObject(data);    

                if (App.userid == -1)
                {
                    return "false";
                }

                else
                {
                    return "true";
                }
               
            }
            catch (Exception ea)
            {
              //  return "server";   
                return "false";
                //System.Diagnostics.Debug.WriteLine("SYSTEMRES???????????????" + ea.Message);
            }
        }


        public String SaleOrderConfirm(String modelname, string methodname, int saleorderid)
        {
            String res = odooConnector.odooMethodSaleOrderConfirm(modelname,methodname,saleorderid);

            return res;
        }

        public String SetToConfirm(String modelname, string methodname, int saleorderid)
        {
            String res = odooConnector.odooMethodSetToConfirm(modelname, methodname, saleorderid);

            return res;
        }

        public List<CRMOpportunities> nextActivity()
        {
            List<CRMOpportunities> tarList = new List<CRMOpportunities>();
            JArray result = odooConnector.odooMethodCall_salestarget<JArray>("sale.crm", "get_next_activities");
            tarList = result.ToObject<List<CRMOpportunities>>();
            return tarList;
        }

        public List<Attachments> getfileAtachment(int saleorder_id)
        {
            List<Attachments> tarList = new List<Attachments>();
            JArray result = odooConnector.odooMethodCall_attachment<JArray>("sale.order", "get_attachments",saleorder_id);
            tarList = result.ToObject<List<Attachments>>();
            return tarList;
        }

        public List<CRMLead> crmLeadData()
        {
            String[] colourcodes = new String[] { "#3498db", "#e67e22", "#c0392b", "#2ecc71", "#d35400", "#27ae60", " #e74c3c", "#2980b9" };
            try
            {

                //JObject res = odooConnector.odooCrmLeadDataCall("sale.crm", "get_your_pipelines_all_orders");

                App.filterdict["range"] = "False";
                App.filterdict["days"] = "False";
                App.filterdict["month"] = "True";

             //   odooConnector = OdooRPC.InstanceCreation(Settings.UserUrlName);
                JObject res = odooConnector.odooFilterDataCall("sale.crm", "get_your_pipelines_all_orders");

                //List<OrderLine> test = new List<OrderLine>();

                App.statedict = res["state_list"].ToObject<Dictionary<int, string>>();
                App.countrydict = res["country_list"].ToObject<Dictionary<int, string>>();


                App.cus_address = res["Customers_Address"].ToObject<Dictionary<dynamic, dynamic>>();

                App.product_PriceList = res["Product_PriceList"].ToObject<List<Product_PriceList>>();

                App.all_delivery_method = res["all_delivery_method"].ToObject<List<all_delivery_method>>();

                App.partner_name = res["user_name"].ToString();
                App.partner_image = res["user_image_medium"].ToString();
                App.partner_id = res["partner_id"].ToObject<int>();
                App.partner_email = res["user_email"].ToString();
                    
                App.taxList = res["taxes"].ToObject<List<taxes>>();
                App.paytermList = res["payment_terms"].ToObject<List<paytermList>>();

               App.commisiongroupList = res["commission_group"].ToObject<List<commisiongroupList>>();

                App.salesteam = res["sales_team"].ToObject<Dictionary<int, string>>();
                App.salespersons = res["sales_persons"].ToObject<Dictionary<int, string>>();

                App.productList = res["Products"].ToObject<List<ProductsList>>();

                App.user_location_string = res["location"].ToString();

                App.branchList = res["branch"].ToObject<List<branch>>();
                App.warehousList = res["warehouse"].ToObject<List<warehouse>>();
                App.analayticList = res["analytic_account"].ToObject<List<analytic>>();

                App.cusList = res["Customers_List"].ToObject<List<Customers>>();

 // App User DB Starts Here ********************//

                //App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                //App._connection.CreateTable<UserModelDB>();

                //if (App.UserListDb.Count == 0)
                //{

                    //var json_state = Newtonsoft.Json.JsonConvert.SerializeObject(res["state_list"]);

                    //var json_country = Newtonsoft.Json.JsonConvert.SerializeObject(res["country_list"]);

                    //var json_tags = Newtonsoft.Json.JsonConvert.SerializeObject(res["crm_lead_tags"]);
                    //var json_salesteam = Newtonsoft.Json.JsonConvert.SerializeObject(res["sales_team"]);
                    //var json_salespersons = Newtonsoft.Json.JsonConvert.SerializeObject(res["sales_persons"]);

                    //var json_customers_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["Customers"]);
                    //var  json_next_activity = Newtonsoft.Json.JsonConvert.SerializeObject(res["next_activity"]);
                    //var json_stages = Newtonsoft.Json.JsonConvert.SerializeObject(res["stages"]);

                    //var json_payment_terms =  Newtonsoft.Json.JsonConvert.SerializeObject(res["payment_terms"]);

                    //var json_commission_groups = Newtonsoft.Json.JsonConvert.SerializeObject(res["commission_group"]);

                    //var jso_tags_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["taxes"]);

                    //var jso_products_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["Products"]);

                   
                    //var sample = new UserModelDB
                    //{
                    //    userid = App.userid,
                    //    partnerid = App.partner_id,
                    //    user_image_medium = App.partner_image,
                    //    user_email= App.partner_email,
                    //    user_name = App.partner_name,
                    //    state_list = json_state,
                    //    country_list = json_country,
                    //    tagsPicker = json_tags,
                    //    sales_team = json_salesteam,
                    //    sales_persons = json_salespersons,
                    //    customers_list = json_customers_list,
                    //    next_activity = json_next_activity,
                    //    stages = json_stages,
                    //    payment_terms = json_payment_terms,
                    //    commission_group = json_commission_groups,
                    //    tax_list = jso_tags_list,
                    //    products = jso_products_list
                    //};
                    //App._connection.Insert(sample);

                    //try
                    //{

                    //    var details = (from y in App._connection.Table<UserModelDB>() select y).ToList();

                    //    App.UserListDb = details;

                    //    List<ProductsList> productslistdb = new List<ProductsList>();
                    //    Dictionary<int, string> cusdictdb = new Dictionary<int, string>();
                    //    foreach (var item in App.UserListDb)
                    //    {
                    //        productslistdb = JsonConvert.DeserializeObject<List<ProductsList>>(item.products);

                    //        cusdictdb = JsonConvert.DeserializeObject<Dictionary<int, string>>(item.customers_list);

                    //        App.ProductListDb = productslistdb;
                    //        App.cusdictDb = cusdictdb;
                    //        App.userid_db = item.userid;
                    //    }
                    //}

                                       
                    //catch
                    //{
                    //    int te = 0;
                    //}
                     

               // }

            // DB Ends



                App.nextActivityList = res["next_activity"].ToObject<List<next_activity>>();

                App.crmList = res["crm_leads"].ToObject<List<CRMLead>>();


// Lead DB Starts Here ********************//

                //App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                //App._connection.CreateTable<CRMLeadDB>();

                //var crmlead_details = App._connection.Query<CRMLeadDB>("SELECT * from CRMLeadDB where yellowimg_string = ?", "yellowcircle.png");
               
                //if (crmlead_details.Count() == 0)
                //{
                //    App._connection.Query<SalesQuotationDB>("DELETE from CRMLeadDB");
                   
                //    foreach (var item in App.crmList)
                //    {
                //        var sample = new CRMLeadDB
                //        {
                //            id=item.id,
                //            customer = item.customer,
                //            next_activity = item.next_activity,
                //            name = item.name,
                //            probability = item.probability,
                //            phone = item.phone,
                //            title_action = item.title_action,
                //            expected_revenue = item.expected_revenue,

                //            team_name = item.team_name,
                //            priority = item.priority,
                //            state = item.state,
                //            street = item.street,
                //            street2 = item.street2,
                //            city = item.city,
                //            country = item.country,
                //            contact_name = item.contact_name,
                //            mobile = item.mobile,
                //            email = item.email,
                //            pipe_date = item.pipe_date,
                //            pipe_date1 = item.pipe_date1,
                //            conDate = item.conDate,
                //            conDate1 = item.conDate1,
                //            DateOrder = item.DateOrder,
                //            FullState = item.FullState,

                //            state_colour = item.state_colour,

                //            //order_line = item.order_line[0].ToString()
                //        };
                //        App._connection.Insert(sample);


                //        //App._samp.Add(sample);
                //    }

                //    try
                //    {

                //        var details = (from y in App._connection.Table<CRMLeadDB>() select y).ToList();

                //        App.crmListDb = details;

                //    }

                //    catch
                //    {
                //        int te = 0;
                //    }
                //}


// Lead DB Ends

                App.crmOpprList = res["crm_quotations"].ToObject<List<CRMOpportunities>>();



 // Oppo DB Starts Here ********************//

                //App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                //App._connection.CreateTable<CRMOpportunitiesDB>();

                //var crmoppo_details = App._connection.Query<CRMOpportunitiesDB>("SELECT * from CRMOpportunitiesDB where yellowimg_string = ?", "yellowcircle.png");

                //if(crmoppo_details.Count == 0 )
                //{
                //    App._connection.Query<SalesQuotationDB>("DELETE from CRMOpportunitiesDB");
                //    foreach(var item in App.crmOpprList)
                //    {
                //        var sample = new CRMOpportunitiesDB
                //        {
                //            customer = item.customer,
                //            next_activity = item.next_activity,
                //            name = item.name,
                //            probability = item.probability,
                //            phone = item.phone,
                //            title_action = item.title_action,
                //            expected_revenue = item.expected_revenue,
                //            team_name = item.team_name,
                //            priority = item.priority,
                //            state = item.state,
                //            street = item.street,
                //            street2 = item.street2,
                //            city = item.city,
                //            country = item.country,
                //            contact_name = item.contact_name,
                //            mobile = item.mobile,
                //            email = item.email,
                //            FullState = item.FullState,
                //            state_colour = item.state_colour,
                //            pipe_date = item.pipe_date,
                //            //pipe_datetime = item.pipe_datetime,
                //            //pipe_datetime1 = item.pipe_datetime1,
                //            pipe_datetime = item.pipe_datetime.ToString(),
                //            pipe_datetime1 = item.pipe_datetime1.ToString(),
                //            DateOrder = item.DateOrder,
                //            newpipe_date = item.newpipe_date,
                //        };
                //        App._connection.Insert(sample);
                //    }

                //    try
                //    {

                //        var details = (from y in App._connection.Table<CRMOpportunitiesDB>() select y).ToList();

                //        App.CRMOpportunitiesListDb = details;

                //    }
                //    catch
                //    {
                //        int te = 0;
                //    }
                //}

                App.salesQuotList = res["sale_quotations"].ToObject<List<SalesQuotation>>();

                App.draftQuotList = res["draft_quotations"].ToObject<List<SalesQuotation>>();


 // Quot DB Starts Here ********************//

                //App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                //App._connection.CreateTable<SalesQuotationDB>();

                //var sq_details = App._connection.Query<SalesQuotationDB>("SELECT * from SalesQuotationDB where yellowimg_string = ?", "yellowcircle.png");

    
                //if (sq_details.Count() == 0)
                //{
                //  App._connection.Query<SalesQuotationDB>("DELETE from SalesQuotationDB");
                    
                //    foreach (var item in App.salesQuotList)
                //    {
                //        var json_orderline = Newtonsoft.Json.JsonConvert.SerializeObject(item.order_line);
                //        var sample = new SalesQuotationDB
                //        {
                //            customer = item.customer,
                //        //    next_activity = item.next_activity,
                //            name = item.name,
                //          //  probability = item.probability,
                //           // phone = item.phone,
                //          //  title_action = item.title_action,
                //          //  expected_revenue = item.expected_revenue,
                //            payment_term =  item.payment_term,
                //           // team_name = item.team_name,
                //            priority = item.priority,
                //            state = item.state,
                //            street = item.street,
                //            street2 = item.street2,
                //            city = item.city,
                //            country = item.country,
                //           // contact_name = item.contact_name,
                //          //  mobile = item.mobile,
                //          //  email = item.email,
                //           // state_colour = item.state_colour,
                //            sales_person = item.sales_person,
                //            sales_team = item.sales_team,
                //            customer_reference = item.customer_reference,
                //            fiscal_position = item.fiscal_position,
                //            FullState = item.FullState,
                //            state_colour = item.state_colour,
                //            DateOrder = item.DateOrder,
                //            order_line = json_orderline
                //        };
                //        App._connection.Insert(sample);
                //        //App._samp.Add(sample);
                //    }

                //    try
                //    {
                //        var details = (from y in App._connection.Table<SalesQuotationDB>() select y).ToList();
                //        App.SalesQuotationListDb = details;
                //    }

                //    catch
                //    {
                //        int te = 0;
                //    }
                //}

      // DB Ends


                App.salesOrderList = res["sale_orders"].ToObject<List<SalesOrder>>();


// Sale Order DB Starts Here ********************//

                //App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                //App._connection.CreateTable<SalesOrderDB>();

                //if (App.SalesOrderListDb.Count == 0)
                //{
                                 
                //    foreach (var item in App.salesOrderList)
                //    {

                //        var json_orderline = Newtonsoft.Json.JsonConvert.SerializeObject(item.order_line);

                //        var sample = new SalesOrderDB
                //        {
                //            customer = item.customer,
                //            next_activity = item.next_activity,
                //            name = item.name,
                //            probability = item.probability,
                //            phone = item.phone,
                //            title_action = item.title_action,
                //            expected_revenue = item.expected_revenue,
                //            payment_term = item.payment_term,
                //            team_name = item.team_name,
                //            priority = item.priority,
                //            state = item.state,
                //            street = item.street,
                //            street2 = item.street2,
                //            city = item.city,
                //            country = item.country,
                //            contact_name = item.contact_name,
                //            mobile = item.mobile,
                //            email = item.email,
                //            state_colour = item.state_colour,
                //            sales_person = item.sales_person,
                //            sales_team = item.sales_team,
                //            customer_reference = item.customer_reference,
                //            fiscal_position = item.fiscal_position,
                //            FullState = item.FullState,

                //          //  ColorCode = item.ColorCode,

                //            DateOrder = item.DateOrder,

                //            order_line = json_orderline,

                                                                              
                //        };
                //        App._connection.Insert(sample);
                //        //App._samp.Add(sample);
                //    }

                //    try
                //    {

                //        var details = (from y in App._connection.Table<SalesOrderDB>() select y).ToList();
                //        App.SalesOrderListDb = details;
                                           
                //    }
                //    catch
                //    {
                //        int te = 0;
                //    }

                //}

                               
                App.stageList = res["stages"].ToObject<List<stages>>();

                String colorCodeData = "";

                int cnt = 0;
                foreach (stages stateObj in res["stages"].ToObject<List<stages>>())
                {
                    try
                    {
                        colorCodeData = colorCodeData + "," + stateObj.name + "^" + colourcodes[cnt];
                    }
                    catch
                    {
                        cnt = 0;
                        continue;
                    }
                    cnt++;
                }

                Settings.StageColourCode = colorCodeData;
                App.cusdict = res["Customers"].ToObject<Dictionary<int, string>>();

                App.reasondict = res["lost_reason"].ToObject<Dictionary<int, string>>();

                App.crmleadtags = res["crm_lead_tags"].ToObject<Dictionary<int, string>>();

                App.user_gps_enabled = res["user_gps_enabled"].ToObject<Boolean>();
                App.user_gps_time = res["user_gps_time"].ToObject<int>();

                App.locationsList = res["Location"].ToObject<List<LocationsList>>();

                App.cus_address = res["Customers_Address"].ToObject<Dictionary<dynamic, dynamic>>();

                return App.crmList;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);

                if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
                {
                    App.NetAvailable = false;
                }

                else if (ea.Message.Contains("(503) Service Unavailable"))
                {
                    App.responseState = false;
                }
                return App.crmList;
            }
        }


        public List<CRMLead> crmFilterData()
        {
            String[] colourcodes = new String[] { "#3498db", "#e67e22", "#c0392b", "#2ecc71", "#d35400", "#27ae60", " #e74c3c", "#2980b9" };
            try
            {

                JObject res = odooConnector.odooFilterDataCall("sale.crm", "get_your_pipelines_all_orders");
                //   List<OrderLine> test = new List<OrderLine>();

                              
                App.taxList = res["taxes"].ToObject<List<taxes>>();
                App.paytermList = res["payment_terms"].ToObject<List<paytermList>>();
               App.commisiongroupList = res["commission_group"].ToObject<List<commisiongroupList>>();

                App.nextActivityList = res["next_activity"].ToObject<List<next_activity>>();

                App.crmList = res["crm_leads"].ToObject<List<CRMLead>>();

                App.crmOpprList = res["crm_quotations"].ToObject<List<CRMOpportunities>>();

                App.draftQuotList = res["draft_quotations"].ToObject<List<SalesQuotation>>();

                App.salesQuotList = res["sale_quotations"].ToObject<List<SalesQuotation>>();

                App.salesOrderList = res["sale_orders"].ToObject<List<SalesOrder>>();


                App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                App._connection.CreateTable<SalesOrderDB>();
                                            
                App.stageList = res["stages"].ToObject<List<stages>>();

                String colorCodeData = "";

                int cnt = 0;
                foreach (stages stateObj in res["stages"].ToObject<List<stages>>())
                {
                    try
                    {
                        colorCodeData = colorCodeData + "," + stateObj.name + "^" + colourcodes[cnt];
                    }
                    catch
                    {
                        cnt = 0;
                        continue;
                    }
                    cnt++;
                }

                Settings.StageColourCode = colorCodeData;

                App.cusdict = res["Customers"].ToObject<Dictionary<int, string>>();

                App.cusList = res["Customers_List"].ToObject<List<Customers>>();

                App.reasondict = res["lost_reason"].ToObject<Dictionary<int, string>>();

                App.salesteam = res["sales_team"].ToObject<Dictionary<int, string>>();
                App.salespersons = res["sales_persons"].ToObject<Dictionary<int, string>>();

                App.partner_name = res["user_name"].ToString();
                App.partner_image = res["user_image_medium"].ToString();
                App.partner_id = res["partner_id"].ToObject<int>();
                App.partner_email = res["user_email"].ToString();

                App.user_gps_enabled = res["user_gps_enabled"].ToObject<Boolean>();
                App.user_gps_time = res["user_gps_time"].ToObject<int>();

                App.productList = res["Products"].ToObject<List<ProductsList>>();

                 App.locationsList = res["Location"].ToObject<List<LocationsList>>();

                App.branchList = res["branch"].ToObject<List<branch>>();
                App.warehousList = res["warehouse"].ToObject<List<warehouse>>();
                App.analayticList = res["analytic_account"].ToObject<List<analytic>>();

                return App.crmList;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);


                if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
                {
                    App.NetAvailable = false;
                }

                else if (ea.Message.Contains("(503) Service Unavailable"))
                {
                    App.responseState = false;
                }

                return App.crmList;
            }
        }

        public List<SalesModel> salesOrderData(string getState)
        {
            List<SalesModel> quotationList = new List<SalesModel>();
            try
            {
                JObject jsonObject = JObject.Parse(Settings.PrefKeyUserDetails.ToString());
                int saleUserId = jsonObject["userId"].ToObject<int>();

                object[] domain = new object[2];
                domain[0] = new object[] { "user_id", "=", saleUserId };
                domain[1] = new object[] { "state", "=", getState };

                quotationList = odooConnector.odooSearchReadCall1<dynamic>("sale.order", domain, new string[] { "name", "partner_id", "date_order", "state" }, true);

                return quotationList;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);
                return quotationList;
            }
        }


        //public List<CustomersModel> GetCustomerData()
        //{
        //    List<CustomersModel> data = new List<CustomersModel>();

        //    object[] domain = new object[] { };

        //    data = odooConnector.odooSearchReadCall3<dynamic>("res.partner", domain, new string[] { "name", "street", "email", "image_small", "city", "country_id" }, true);

        //    return data;
        //}

        public List<CustomersModel> GetCustomerData()
        {
            List<CustomersModel> data = new List<CustomersModel>();

            JArray dt = odooConnector.odooCustomerDataCall<dynamic>("res.partner", "get_customer_list");
            data = dt.ToObject<List<CustomersModel>>();
            return data;

            //JArray dt = odooConnector.odooCustomerDataCall<dynamic>("res.partner", "get_customers_app");
            //data = dt.ToObject<List<CustomersModel>>();
            //return data;
        }

        public List<SalesQuotation> GetDraftQuotationData()
        {
            List<SalesQuotation> data = new List<SalesQuotation>();
            JObject dq = odooConnector.odooDraftDataCall<dynamic>("sale.crm", "get_before_draft_state_quotations");
           
            data = dq["sale_quotations"].ToObject<List<SalesQuotation>>();
          //  data = dt.ToObject<List<SalesQuotation>>();
            return data;
        }

        public JObject GetCustomerCreationData()
        {
            JObject dt = odooConnector.odooCustomerDataCall<dynamic>("res.partner", "get_create_customers_data");
            return dt;
        }


        public List<all_events> GetCalenderData(int month, int year)
        {
            List<all_events> calresList = new List<all_events>();

            JObject result = odooConnector.odooMethodCall_cal<JObject>("calendar.event", "get_calendar_event_app", App.userid, month, year);

            calresList = result["all_events"].ToObject<List<all_events>>();

            App.tagsDict = result["all_tags"].ToObject<Dictionary<int, string>>();

            return calresList;

         }

        public List<StockWareHouseList> GetStockProductData(int id)
        {
            List<StockWareHouseList> calresList = new List<StockWareHouseList>();
         
             calresList = odooConnector.odooMethodCall_stockcount<dynamic>("stock.inventory", "search_by_product", id);
            return calresList;
        }


        public List<StockWareHouseList> GetStockWareHouseData(int id)
        {
            List<StockWareHouseList> calresList = new List<StockWareHouseList>();

            calresList = odooConnector.odooMethodCall_stockcount<JObject>("stock.inventory", "search_by_warehoue", id);
            return calresList;
        }

        public List<StockWareHouseList> GetStockAllData(int prod_id,int loc_id)
        {
            List<StockWareHouseList> calresList = new List<StockWareHouseList>();

            calresList = odooConnector.odooMethodCall_allcountdata<JObject>("stock.inventory", "search_product_with_location", prod_id, loc_id);
            return calresList;
        }

        public List<all_events> GetMeetingsData(int partnerid)
        {
            List<all_events> meetingList = new List<all_events>();

            JObject result = odooConnector.odooMethodCall_meeting<JObject>("calendar.event", "get_customers_meetings", partnerid);

            meetingList = result["all_events"].ToObject<List<all_events>>();

            //App.tagsDict = result["all_tags"].ToObject<Dictionary<int, string>>();

            return meetingList;

        }

        public List<CRMOpportunities> GetOppssData(int partnerid)
        {
            List<CRMOpportunities> oppoList = new List<CRMOpportunities>();
            JArray result = odooConnector.odooMethodCall_meeting<JArray>("res.partner", "get_customers_oppurtunities", partnerid);
            oppoList = result.ToObject<List<CRMOpportunities>>();
            return oppoList;
        }


        public List<SalesOrder> GetSalesData(int partnerid)
        {
            List<SalesOrder> oppoList = new List<SalesOrder>();
            JArray result = odooConnector.odooMethodCall_meeting<JArray>("res.partner", "get_customers_quotation", partnerid);
            oppoList = result.ToObject<List<SalesOrder>>();
            return oppoList;
        }

        public List<SalesModel> salesOrderData1()
        {
            List<SalesModel> data = new List<SalesModel>();
            try
            {
                object[] domain = new object[] { };
                data = odooConnector.odooSearchReadCall1<dynamic>("sale.order", domain, new string[] { "name", "partner_id", "state", "date_order" }, false);

                return data;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);
                return data;
            }

        }

        public List<NewSalesTarget> newsalesTargetData()
        {
            List<NewSalesTarget> data = new List<NewSalesTarget>();

            try
            {

              //  JObject  dt = odooConnector.odooCustomerDataCall<dynamic>("crm.team", "get_sale_target_groups");

                JArray dt = odooConnector.odooCustomerDataCall<dynamic>("crm.team", "get_sale_target_groups");
               data = dt.ToObject<List<NewSalesTarget>>();
                return data;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);
                return data;
            }

        }

        public List<SalesTarget> salesTargetData()
        {
            List<SalesTarget> data = new List<SalesTarget>();
            
            try
            {
              
                JArray dt = odooConnector.odooCustomerDataCall<dynamic>("target.group", "get_sale_commission_target");
                data = dt.ToObject<List<SalesTarget>>();
                return data;
            }
            catch (Exception ea)
            {
                System.Diagnostics.Debug.WriteLine("::::: CRM Warning Message ::::  " + ea.Message);
                return data;
            }

        }

        public string UpdateCRMOpporData(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odooUpdatecrmOppMeeting(modelName, methodName, vals);
            return flag;
        }

        public float getsubtotal(string modelName, string methodName, float unit_price, float quantity, List<int> taxids, float disount)
        {
            float flag = odooConnector.odoogetSubtotal(modelName, methodName, unit_price,quantity,taxids,disount);
            return flag;
        }


        public bool UpdateSaleOrder(string modelName, string methodName, int sale_id, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odooUpdatesaleorder(modelName, methodName, sale_id, vals);
            return flag;
        }

        public bool UpdateCustomerData(string modelName, string methodName, int customer_id, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odooUpdatecustomer(modelName, methodName, customer_id, vals);
            return flag;
        }

        public bool UpdateContactlist(string modelName, string methodName, int contact_id, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odooUpdatecustomer(modelName, methodName, contact_id, vals);
            return flag;
        }

        public bool CreateContactlist(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odooCreatecustomer(modelName, methodName, vals);
            return flag;
        }


        public bool UpdatePassword(string modelName, string methodName, int sale_id, string pass)
        {
            bool flag = odooConnector.odooUpdatepassword(modelName, methodName, sale_id, pass);
            return flag;
        }

        public string UpdateGPSData(string modelName, string methodName, float latitude, float longitude, int id, string time)
        {
            string flag = odooConnector.odooUpdateGpsData(modelName, methodName, latitude, longitude, id, time);
            return flag;
        }

        public bool UpdateGPSData1(string modelName, string methodName, float latitude, float longitude)
        {

            try
            {
                odooConnector.odooUpdateGpsData1(modelName, methodName, latitude,longitude);
            }
            catch
            {
                return false;
            }
            return true;

            //string flag = odooConnector.odooUpdateGpsData1(modelName, methodName, latitude, longitude);
            //return flag;
        }

        public string clearGPSData(string modelName, string methodName, int id)
        {
            string flag = odooConnector.clearGpsData(modelName, methodName, id);
            return flag;
        }


        public string UpdateLeadCreationData(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odooUpdatecrmLeadCreation(modelName, methodName, vals);
            return flag;
        }


        public string UpdateLeadCreationConvertData(string modelName, string methodName, int id, Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odooConvertrmLeadCreation(modelName, methodName, id, vals);
            return flag;
        }

        public string UpdateCRMOpporData1(string modelName, string methodName, int updateId, Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odooUpdatecrmOppMeeting1(modelName, methodName, updateId, vals);
            return flag;
        }

        public List<SalesTarget> GetTargetData()
        {
            List<SalesTarget> tarList = new List<SalesTarget>();
            JArray result = odooConnector.odooMethodCall_salestarget<JArray>("sale.crm", "get_sales_target");
            tarList = result.ToObject<List<SalesTarget>>();
            return tarList;
        }

        public bool UpdateMarkWon(int modelId)
        {
            try
            {
                odooConnector.odooUpdateUserData("crm.lead", "action_set_won", modelId);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UpdateLost(int modelId, int lostId)
        {
            try
            {
                odooConnector.odooLostData("sale.crm", "mark_lost_app", modelId, lostId);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public JObject GetCustomerDetailData(int cus_id)
        {
            JObject dt = odooConnector.odooMethodCall_promotions<dynamic>("res.partner", "get_customer_detail", cus_id);
            return dt;
        }

        //public List<ActivitiesModel> getActivitiesData()
        //{
        //    List<ActivitiesModel> activitiesList = new List<ActivitiesModel>


        //    {
        //        new ActivitiesModel(1,"Nestle","Henry","01/01/2018"),
        //          new ActivitiesModel(1,"Health Building","Agrolait","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),
        //          new ActivitiesModel(1,"Sample","Sample","02/01/2018"),

        //    };

        //    return activitiesList;
        //}

        public static Controller InstanceCreation()
        {
            
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Controller();
                }
                return instance;
            }
        }
    }
}
