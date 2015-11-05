using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using DAL;
using DAL.SqlClient;

namespace BL
{
  public class ContactManager : IContactManager 
  {
    private readonly IRepository contactRepository;
    public void AddContact(string name, string streetAndNumber, short zipCode, string city, Gender gender, DateTime birthDay, string phone, string mobile)
    {
      
      Address adres = new Address
      {
        StreetAndNumber = streetAndNumber,
        Zipcode = zipCode,
        City = city,
      };
      Contact contact = new Contact
      {
        ContactId = getHighestID() + 1,
        Name = name,
        Gender = gender,
        Birthday = birthDay,
        Phone = phone,
        Mobile = mobile,
        Adress = adres
      };
      contactRepository.CreateContact(contact);
      
    }

    public ContactManager()
    {
      //contactRepository = new MemoryRepository();
      contactRepository = new SqlRepository();
    }

    public void ChangeContact(Contact contactToChange)
    {
      contactRepository.UpdateContact(contactToChange);
    }

    public IEnumerable<Category> GetAllCategories()
    {
      return contactRepository.ReadAllCatergories();
    }

    public IEnumerable<Contact> GetAllContacts(OrderByFieldName sortBy = OrderByFieldName.ID)
    {
      //
      return contactRepository.ReadAllContacts();
    }

    public IEnumerable<Contact> GetContactsForACategory(int categoryId)
    {
      //List<Contact> contacts = new List<Contact>();
      return contactRepository.ReadAllContacts(categoryId);
    }

    public Contact ReadContact(int id)
    {
      return contactRepository.ReadContact(id);
    }

    public void RemoveContact(int id)
    {
      contactRepository.DeleteContact(id);
    }

    private int getHighestID()
    {
      List<int> IDs = new List<int>();
      foreach (Contact contact in GetAllContacts())
      {
        IDs.Add(contact.ContactId);
      }
      return IDs.Max();
    }
  }
}
