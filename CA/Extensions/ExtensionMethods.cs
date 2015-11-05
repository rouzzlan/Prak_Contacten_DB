using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Extensions
{
  internal static class ExtensionMethods
  {
    internal static string PrintSummary(this Contact contact)
    {
      return String.Format("ID: {0} {1} - {2} ({3})",
        contact.ContactId,
        contact.Blocked ? "B" : " ",
        contact.Name,
        contact.Birthday);
    }
    internal static string PrintCategory(this Category category)
    {
      return string.Format("{0} ({1})", category.Description, category.CategoryId);
    }

    internal static string PrintLongInfo(this Contact contact)
    {
      return string.Format("{0} - {1} {2} woonachtig te {3}, {4} {5}",
        contact.ContactId,
        contact.Name,
        contact.Mobile == null ? "" : $"({contact.Mobile})",
        contact.Adress.StreetAndNumber,
        contact.Adress.Zipcode,
        contact.Adress.City);
    }
  }
}
