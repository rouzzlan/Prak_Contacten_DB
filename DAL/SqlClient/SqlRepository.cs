using System;
using System.Collections.Generic;
using Domain;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL.SqlClient
{
  public class SqlRepository : IRepository
  {
    public void CreateContact(Contact contactToInsert)
    {
      // stap 1: de querry implementeren.
      string adresQuery = "INSERT INTO Addresses (StreetAndNumber, ZipCode, City) VALUES (@streetAndNumber, @zipCode, @city);";
      string contacQuerry = "INSERT INTO Contacts (Name, AddressId, Gender, Blocked, BirthDay, Phone, Mobile) VALUES (@name, @addressId, @gender, @blocked, @birthDay, @phone, @mobile)";
      string indentityQuery = "SELECT SCOPE_IDENTITY()";
      // een connectie starten
      using (SqlConnection connectie = GetConnection())
      {
        connectie.Open();
        //sql command definieren
        //SqlCommand commando = new SqlCommand(contacQuerry, connectie);
        //paramenters aanpassen
        SqlCommand commando = new SqlCommand(adresQuery+indentityQuery, connectie);
        commando.Parameters.AddWithValue("@streetAndNumber", contactToInsert.Adress.StreetAndNumber);
        commando.Parameters.AddWithValue("@zipCode", contactToInsert.Adress.Zipcode);
        commando.Parameters.AddWithValue("@city", contactToInsert.Adress.City);
        //commando.ExecuteNonQuery();

        int id = Convert.ToInt32(commando.ExecuteScalar());


        commando = new SqlCommand(contacQuerry, connectie);
        commando.Parameters.AddWithValue("@name", contactToInsert.Name);
        commando.Parameters.AddWithValue("@addressId", id);//dummy waarde
        commando.Parameters.AddWithValue("@gender", contactToInsert.Gender);
        commando.Parameters.AddWithValue("@blocked", contactToInsert.Blocked);
        commando.Parameters.AddWithValue("@birthDay", contactToInsert.Birthday);
        commando.Parameters.AddWithValue("@phone", contactToInsert.Phone);
        commando.Parameters.AddWithValue("@mobile", contactToInsert.Mobile);
        commando.ExecuteNonQuery();

        connectie.Close();
      }

    }

    public void DeleteContact(int id)
    {
      using(var connection = GetConnection())
      {
        var command = new SqlCommand("sp_DeleteContact", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@contactId ", id);
        connection.Open();
        command.ExecuteReader();
      }

      
    }

    public IEnumerable<Category> ReadAllCatergories()
    {
      string query = "SELECT Id, Description FROM Categories";
      List<Category> categories = new List<Category>();
      using (SqlConnection conn = GetConnection())
      {
        //Toewijzen van zowel query alsook de connectie aan het commando object
        SqlCommand cmd = new SqlCommand(query, conn);
        conn.Open();
        //Commando uitvoeren op de DB en de resultatentabel uitlezen via onze
        //DataReader cursor
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
          Category loadingCategory = new Category();
          loadingCategory.CategoryId = reader.GetInt32(0);
          loadingCategory.Description = reader.GetString(1);
          categories.Add(loadingCategory);
        }
        reader.Close();
        conn.Close();
      }
      return categories;
    }

    public IEnumerable<Contact> ReadAllContacts(int? categoryId = default(int?))
    {
      string query;
      //lopt hier mis bij optie 2 keuze met categorie id
      query = "SELECT c.Id, Name, AddressId, a.StreetAndNumber, a.ZipCode, a.City, Gender, Blocked, BirthDay, Phone, Mobile FROM Contacts c INNER JOIN Addresses a ON a.Id = AddressId";
      if (categoryId != null)
      {
        query += " INNER JOIN Contacts_Categories cc ON cc.ContactId = c.Id WHERE cc.CategoryId = @categoryId";
      }
      List<Contact> contactsTemp = new List<Contact>();
      using (SqlConnection conn = GetConnection())
      {
        SqlCommand cmd = new SqlCommand(query, conn);
        if(categoryId != null)
        {
          cmd.Parameters.AddWithValue("@categoryId", categoryId);
        }
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
          Contact loadingContact = new Contact();
          loadingContact.ContactId = reader.GetInt32(0);
          loadingContact.Name = reader.GetString(1);
          Address adres = new Address();
          adres.AddressId = reader.GetInt32(2);
          adres.StreetAndNumber = reader.GetString(3);
          adres.Zipcode = (short)reader.GetInt16(4); //kan fout geven
          adres.City = reader.GetString(5);
          loadingContact.Adress = adres;
          byte genderInt = reader.GetByte(6);// blijkbaar met eem byte
          if (genderInt == 1)
            loadingContact.Gender = Gender.Male;
          else
            loadingContact.Gender = Gender.Female;
          loadingContact.Blocked = reader.GetBoolean(7);
          loadingContact.Birthday = reader.GetDateTime(8);
          loadingContact.Phone = reader.IsDBNull(9) ? null : reader.GetString(9);
          loadingContact.Mobile = reader.IsDBNull(10) ? null : reader.GetString(10);

          contactsTemp.Add(loadingContact);
        }
      }
      return contactsTemp;
    }

    public Contact ReadContact(int id)
    {
      string query = "SELECT c.Id, Name, AddressId, a.StreetAndNumber, a.ZipCode, a.City, Gender, Blocked, BirthDay, Phone, Mobile FROM Contacts c INNER JOIN Addresses a ON a.Id = AddressId WHERE c.Id = @contactId";
      Contact loadingContact = new Contact();
      using (SqlConnection conn = GetConnection())
      {
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@contactId", id);
        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        
        loadingContact.ContactId = reader.GetInt32(0);
        loadingContact.Name = reader.GetString(1);
        Address adres = new Address();
        adres.AddressId = reader.GetInt32(2);
        adres.StreetAndNumber = reader.GetString(3);
        adres.Zipcode = (short)reader.GetInt16(4); //kan fout geven
        adres.City = reader.GetString(5);
        loadingContact.Adress = adres;
        byte genderInt = reader.GetByte(6);// blijkbaar met eem byte
        if (genderInt == 1)
          loadingContact.Gender = Gender.Male;
        else
          loadingContact.Gender = Gender.Female;
        loadingContact.Blocked = reader.GetBoolean(7);
        loadingContact.Birthday = reader.GetDateTime(8);
        loadingContact.Phone = reader.IsDBNull(9) ? null : reader.GetString(9);
        loadingContact.Mobile = reader.IsDBNull(10) ? null : reader.GetString(10);
        
      }
      return loadingContact;
    }

    public void UpdateContact(Contact contactToUpdate)
    {
      string query = "UPDATE Contacts SET Name = @name, Gender = @gender, Blocked = @blocked, BirthDay = @birthDay, Phone = @phone, Mobile = @mobile WHERE Id = @contactId";
      using(SqlConnection conn = GetConnection())
      {
        conn.Open();
        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@name", contactToUpdate.Name);
        if(contactToUpdate.Gender == Gender.Male)
        {
          cmd.Parameters.AddWithValue("@gender", 1);
        }
        else
        {
          cmd.Parameters.AddWithValue("@gender", 2);
        }
        cmd.Parameters.AddWithValue("@blocked", contactToUpdate.Blocked);
        cmd.Parameters.AddWithValue("@birthDay", contactToUpdate.Birthday);
        cmd.Parameters.AddWithValue("@phone", contactToUpdate.Phone);
        cmd.Parameters.AddWithValue("@mobile", contactToUpdate.Mobile);
        cmd.Parameters.AddWithValue("@contactId", contactToUpdate.ContactId);
        cmd.ExecuteNonQuery();
        conn.Close();
      }
    }
    //string connString=ConfigurationManager.ConnectionStrings["<naamConnectionString>"].ConnectionString;
    private SqlConnection GetConnection()
    {
      var connStr = ConfigurationManager.ConnectionStrings["ContactsDB-ADO"].ConnectionString;
      //add reference niet vergeten
      return new SqlConnection(connStr);
    }
  }
}
