using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public class Address
  {
    public int AddressId { get; set; }
    public string StreetAndNumber { get; set; }
    public short Zipcode { get; set; }
    public string City { get; set; }
    public Contact Contact { get; set; }

  }
}
