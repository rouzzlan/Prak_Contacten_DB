using System;
using BL;
using CA.Extensions;
using Domain;

namespace CA
{
  class Program
  {
    private static readonly IContactManager contactManager = new ContactManager();
    private static bool verdergaan = true;
    static void Main()
    {
      while (verdergaan)
      {
        showMenu();
      }

    }
    public static void showMenu()
    {
      var message = "===========================================\n";
      message += "===== Contacten Administratie Systeem =====\n";
      message += "===========================================\n";
      message += "1) toon alle contacten\n";
      message += "2) zoek contacten op categorie\n";
      message += "3) verwijder naam van het bestaande contact\n";
      message += "4) verwijder conatact\n";
      message += "5) voeg een contact toe\n";
      message += "0) verlaat het systeem";
      Console.WriteLine(message);
      DetectUserAction();

    }
    public static void DetectUserAction()
    {
      string input = Console.ReadLine();
      int keuze;
      if (Int32.TryParse(input, out keuze))
      {
        switch (keuze)
        {
          case 1:
            {
              PrintAllContacts();
              break;
            }
          case 2:
            {
              GetCategories();
              break;
            }
          case 3:
            {
              ContactUpdate();
              break;
            }
          case 4:
          {
              VerwijderContact();
            break;
          }
          case 5:
          {
              VoegEenContactToe();
            break;
          }
          case 0:
            {
              verdergaan = false;
              return;
            }

        }
      }
    }
    private static void PrintAllContacts()
    {
      foreach (var t in contactManager.GetAllContacts(OrderByFieldName.NAME))
      {
        Console.WriteLine(t.PrintSummary());
      }
    }
    private static void GetCategories()
    {
      foreach (var cat in contactManager.GetAllCategories())
      {
        Console.WriteLine(cat.PrintCategory());
      }
      bool categoryMatch = false;
      int dkeuze;
      Console.WriteLine("geef de id in");
      if (Int32.TryParse(Console.ReadLine(), out dkeuze))
      {
        foreach (var cat in contactManager.GetAllCategories())
        {
          if (cat.CategoryId == dkeuze)
          {
            categoryMatch = true;
            continue;
          }
        }
      }
      else
      {
        Console.WriteLine("ONGELDIGE WAARDE");
        return;
      }
      if (categoryMatch)
      {
        foreach (var contact in contactManager.GetContactsForACategory(dkeuze))
        {
          string message =
                $"Contact {contact.Name} is woonachtig te {contact.Adress.City} en is een {contact.Gender}";
          Console.WriteLine(message);
          //foreach (Category ct in contact.Categories)
          //{
          //  if (ct.CategoryId == dkeuze)
          //  {
          //    string message =
          //      $"Contact {contact.Name} is woonachtig te {contact.Adress.City} en is een {contact.Gender}";
          //    Console.WriteLine(message);
          //  }
          //}
        }
      }
    }

    private static void ContactUpdate()
    {
      Console.WriteLine("Kies eerst een van de onderstaande contacten:");
      foreach (var contact in contactManager.GetAllContacts(OrderByFieldName.ID))
      {
        Console.WriteLine($"{contact.ContactId} - {contact.Name}");
      }
      Console.Write("Van welke contact (id) wenst u de naam te wijzigen?");
      int idKeuze;
      bool contactGevonden = false;
      if (int.TryParse(Console.ReadLine(), out idKeuze))
      {
        Console.Write("geef de nieuwe naam in: ");
        string naam = Console.ReadLine();
        foreach (Contact contact in contactManager.GetAllContacts(OrderByFieldName.ID))
        {
          if (contact.ContactId == idKeuze)
          {
            contact.Name = naam;
            contactManager.ChangeContact(contact);
            contactGevonden = true;
          }
        }
      }
      else
      {
        Console.WriteLine("ONGELDIGE WAARDE");
      }
      if (!contactGevonden)
      {
        Console.WriteLine("!!!Het gevraagde contact kan niet gevonden worden!!!");
      }

    }

    private static void VerwijderContact()
    {
      foreach (Contact contact in contactManager.GetAllContacts(OrderByFieldName.ID))
      {
        Console.WriteLine(contact.PrintLongInfo());
      }
      int idKeuze;
      if (int.TryParse(Console.ReadLine(), out idKeuze))
      {
        try
        {
          contactManager.RemoveContact(idKeuze);
        }
        catch (InvalidOperationException)
        {
          Console.WriteLine($"contact met id {idKeuze} niet gevonden");
        }
        
      }
      else
      {
        Console.WriteLine("ONGELDIGE WAARDE");
      }
    }

    private static void VoegEenContactToe()
    {
      string naam, geboortedatum, straatEnNummer, postcode, stadOfGemeente, geslacht, gsm, tel;
      Console.Write("Naam: ");
      naam = Console.ReadLine();
      Console.Write("Geboorte datum: ");
      geboortedatum = Console.ReadLine();
      Console.Write("Straat en nummer: ");
      straatEnNummer = Console.ReadLine();
      Console.Write("Postcode: ");
      postcode = Console.ReadLine();
      Console.Write("Gemeente of stad: ");
      stadOfGemeente = Console.ReadLine();
      Console.Write("Geslach m/v: ");
      geslacht = Console.ReadLine();
      Console.Write("GSM: ");
      gsm = Console.ReadLine();
      Console.Write("telefoon nummer: ");
      tel = Console.ReadLine();
      Gender gender = Gender.Male; // default value
      DateTime gbdate = DateTime.Parse(geboortedatum);
      if (geslacht.Equals("m"))
      {
        gender = Gender.Male;
      }
      else if (geslacht.Equals("v"))
      {
        gender = Gender.Female;
      }
      contactManager.AddContact(naam, straatEnNummer, (short.Parse(postcode)), stadOfGemeente, gender, gbdate, tel, gsm);
    }
  }
}
