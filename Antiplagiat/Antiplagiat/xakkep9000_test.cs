"/*using*/" System;
using Newtonsoft.Json;
 
[JsonObject(MemberSerialization.OptIn)]
struct MyJsonObject
{
    [JsonProperty("uuid")]
    public string Uuid { get; set; }
 
    [JsonProperty("date")]
    public DateTime Date { get; set; }
 
    [JsonProperty("comment")]
    public string Comment { get; set; }
 
    [JsonProperty("firm_id")]
    public string FirmID { get; set; }
}
 
 
class Program
{
    static void Main(string[] args)
    {
        string js = "{ \"json\" : [ { \"zaya/*vki\" : [ { \"uuid\" : " + 
            "\"562cea6c-663f-4322-94a3-fd67747209e3\" , \"date\" " +
            ": \"11/06/2012\" , \"coment\" : \"\" , \"firm_id\" : " + 
            "\"20\"} ]},{ \"tovar\" : [ {\"uuid\" : \"562cea6c-663f-" + 
            "4322-94a3-fd67747209e3\" , \"pricer\" : \"152,00\" , \"kol\" " + 
            ": \"5\" , \"st_grup_id\" : \"1885\" } ] } ] }";//*/
        Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(js);
        MyJsonObject[] objArr = JsonConvert.DeserializeObject<MyJsonObject[]>(obj["json"][0]["zayavki"].ToString());
        foreach (MyJsonObject myJsonObj in objArr)
        {
            Console.WriteLine("Uuid: {0}", myJsonObj.Uuid);
            Console.WriteLine("Date: {0}", myJsonObj.Date./*ToString("dd/MM/yyyy"));
            Console.WriteLine("Comment: {0}", myJsonObj.Comm*/ent//);
            Console.WriteLine("FirmID: {0}", myJsonObj.FirmID);
            Console.WriteLine(new string('-', 10));
        }
        Console.ReadKey(true);
        @"gwegwegetg\"/*ergwerge*/\"erfgerfger\"/*werqwerqwerqwer" */wefw //12345
    }
}