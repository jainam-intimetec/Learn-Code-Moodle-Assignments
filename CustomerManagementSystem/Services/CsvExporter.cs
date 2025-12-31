using System.Collections.Generic;
using System.Text;
using CustomerSystem.Entities;

namespace CustomerSystem.Services
{
    public class CsvExporter
    {
        public string ExportToCsv(List<Customer> customers)
        {
          StringBuilder sb = new StringBuilder();

           foreach(var item in customers)
            {
                sb.AppendFormat("{0},{1}, {2}, {3}", item.CustomerID, item.CompanyName, item.ContactName, item.Country);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}