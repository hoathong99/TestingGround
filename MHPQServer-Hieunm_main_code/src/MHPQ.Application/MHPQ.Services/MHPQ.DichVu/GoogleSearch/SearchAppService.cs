using Abp.Application.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerpApi;
using MHPQ.Common.DataResult;

namespace MHPQ.MHPQ.Services.MHPQ.DichVu.GoogleSearch
{
    public class GoogleSearch : SerpApiSearch
    {
        public GoogleSearch(Hashtable parameter, String apiKey) : base(parameter, apiKey, SerpApiSearch.GOOGLE_ENGINE) { }
        public GoogleSearch(String apiKey) : base(new Hashtable(), apiKey, SerpApiSearch.GOOGLE_ENGINE) { }
        /*
         * Get list of location using Location API
         */
        public JArray GetLocation(string location, int limit)
        {
            string buffer = getRawResult("/locations.json", "location=" + location + "&limit=" + limit, false);
            return JArray.Parse(buffer);
        }
    }
    public interface ISearchAppService : IApplicationService
    {
        Task<DataResult> GetSearchResult(string place, string input);
    }
    public class SearchAppService : MHPQAppServiceBase, ISearchAppService
    {
        public SearchAppService()
        {
        }
        public async Task<DataResult> GetSearchResult(string place, string input)
        {
            String apiKey = "";
            if (apiKey == null)
            {
                return DataResult.ResultError(null, "Invalid ApiKey");
            }

            // Localized search for Coffee shop in Austin Texas
            Hashtable ht = new Hashtable();
            ht.Add("q", input);
            ht.Add("hl", "vi");
            ht.Add("gl", "vn");
            ht.Add("google_domain", "google.com");

            GoogleSearch search = new GoogleSearch(ht, apiKey);
            try
            {
                //Get location matching: input;
                JArray locations = search.GetLocation(place, 3);
                // set location
                search.parameterContext.Add("location", (string)locations[0]["canonical_name"]);

                JObject data = search.GetJson();
                JObject resultShops = (JObject)data["local_results"];
                // Close socket
                search.Close();
                //string id = (string)((JObject)data["search_metadata"])["id"];
                //Console.WriteLine("Search from the archive: " + id + ". [0 credit]");
                //JObject archivedSearch = search.GetSearchArchiveJson(id);
                
                //organic result coffee shop;
                return DataResult.ResultSucces( resultShops, "Success");
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
