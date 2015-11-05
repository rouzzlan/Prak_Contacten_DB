using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
  public interface IRepository
  {
    IEnumerable<Category> ReadAllCatergories();
    IEnumerable<Contact> ReadAllContacts(int? categoryId = null);
    Contact ReadContact(int id);
    void CreateContact(Contact contactToInsert);
    void UpdateContact(Contact contactToUpdate);
    void DeleteContact(int id);
  }
}
