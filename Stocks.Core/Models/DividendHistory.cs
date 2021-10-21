using Newtonsoft.Json;
using System;

namespace Stocks.Core.Models
{
    public class DividendHistory
    {
        public DateTime ExDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime DeclarationDate { get; set; }
        [JsonIgnore]
        public DateTime WhenToBuy
        {
            get
            {
                try
                {
                    return ExDate.AddDays(-1);
                }
                catch (Exception ex)
                {

                    throw new Exception(ExDate.ToShortDateString(), ex);
                }
            }
        }
        public string Type { get; set; }
        public double Amount { get; set; }

        [JsonIgnore]
        public bool HasSpecial { get => Type == "Special"; } 

        public override string ToString()
        => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
