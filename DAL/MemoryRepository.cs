using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace DAL
{
  public class MemoryRepository : IRepository
  {
    private List<Contact> contacts;
    private List<Category> categories;
    //de werking verifieren;
    public void CreateContact(Contact contactToInsert)
    {
      contacts.Add(contactToInsert);
    }

    public void DeleteContact(int id)
    {
      bool found = false;
      foreach (Contact matchingContact in contacts)
      {
        if (matchingContact.ContactId == id)
        {
          contacts.Remove(matchingContact);
          found = true;
          break;
        }
      }
      if (!found)
      {
        throw new InvalidOperationException(string.Format("Contact met id {0} is niet gevonden", id));
      }
    }

    public IEnumerable<Category> ReadAllCatergories()
    {
      return categories;
    }

    public IEnumerable<Contact> ReadAllContacts(int? categoryId = default(int?))
    {
      List<Contact> contactsTemp = new List<Contact>();
      if (categoryId != null)
      {
        foreach (Contact matchingContact in contacts)
        {
          foreach (Category cat in matchingContact.Categories)
          {
            if (cat.CategoryId == categoryId)
            {
              contactsTemp.Add(matchingContact);
            }
          }
        }
        return contactsTemp;
      }
      return contacts;
    }

    public Contact ReadContact(int id)
    {
      foreach (Contact matchingContact in contacts)
      {
        if (matchingContact.ContactId == id)
        {
          return matchingContact;
        }
      }
      return null;
    }

    public void UpdateContact(Contact contactToUpdate)
    {
      // Do nothing! All data lives in memory, so everything references the same objects!!
    }
    public MemoryRepository()
    {
      contacts = new List<Contact>();
      categories = new List<Category>();
      SeedCategories();
      SeedContacts();

    }
    private void SeedCategories()
    {
      categories.Add(new Category { CategoryId = 1, Description = "Family" });
      categories.Add(new Category { CategoryId = 2, Description = "Friends" });
      categories.Add(new Category { CategoryId = 3, Description = "School" });
      categories.Add(new Category { CategoryId = 4, Description = "Sport" });
    }
    private void SeedContacts()
    {

      CreateContact(new Contact()
      {
        ContactId = 1,
        Name = "Verstraeten Micheline",
        Adress = new Address() { Zipcode = 2000, City = "Antwerpen", StreetAndNumber = "Antwerpsestraat", AddressId = 1 },
        Gender = Gender.Female,
        Blocked = false,
        Birthday = new DateTime(1978, 8, 30),
        Phone = "03/123.45.67",
        Mobile = "0495/11.22.33",
        Categories = new List<Category> { categories.ElementAt(0) }
      });
      CreateContact(new Contact()
      {
        ContactId = 2,
        Name = "Bogaerts Sven",
        Adress = new Address() { Zipcode = 500, City = "Brussel", StreetAndNumber = "Bruselsestraat 10 ", AddressId = 2 },
        Gender = Gender.Male,
        Blocked = false,
        Birthday = new DateTime(1975, 4, 12),
        Mobile = "0478/12.34.56",
        Categories = new List<Category> { categories.ElementAt(0), categories.ElementAt(2), categories.ElementAt(3) }
      });
      CreateContact(new Contact()
      {
        ContactId = 3,
        Name = "Vlaerminckx Dieter",
        Adress = new Address() { Zipcode = 9000, City = "Gent", StreetAndNumber = "Gentsestraat 95", AddressId = 3 },
        Gender = Gender.Female,
        Blocked = true,
        Birthday = new DateTime(1981, 12, 8),
        Categories = new List<Category> { categories.ElementAt(0), categories.ElementAt(3) }
      });
    }
  }
}
