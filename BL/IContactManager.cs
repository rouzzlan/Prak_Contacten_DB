using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
  public interface IContactManager
  {
    IEnumerable<Category> GetAllCategories();
    IEnumerable<Contact> GetAllContacts(OrderByFieldName sortBy);
    IEnumerable<Contact> GetContactsForACategory(int categoryId);
    Contact ReadContact(int id);
    void AddContact(string name, string streetAndNumber, short zipCode, string city, Gender gender, DateTime birthDay, string phone, string mobile);
    void ChangeContact(Contact contactToChange);
    void RemoveContact(int id);
  }
}
