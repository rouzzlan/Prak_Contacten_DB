using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public class Contact
  {
    public int ContactId { get; set; }
    public string Name { get; set; }
    public Address Adress { get; set; }
    public Gender Gender { get; set; }
    public DateTime Birthday { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public bool Blocked { get; set; }
    public List<Category> Categories { get; set; }
  }
}
