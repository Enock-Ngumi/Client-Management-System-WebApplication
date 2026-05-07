using Client_Management_System.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Client_Management_System.Services
{
    public class PersonService
    {
        private readonly string _connection;

        public PersonService(IConfiguration config)
        {
            _connection = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }
        public List<Persons> GetAll()
        {
            List<Persons> list = new List<Persons>();

            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = "SELECT * FROM persons";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Persons
                {
                    Id = Convert.ToInt32(reader["id"]),
                    FirstName = Convert.ToString(reader["firstname"]) ?? "",
                    LastName = Convert.ToString(reader["lastname"]) ?? "",
                    Email = Convert.ToString(reader["email"]) ?? "",
                    Phone = Convert.ToString(reader["phonenumber"]) ?? "",
                    Dob = reader["dateofbirth"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["dateofbirth"])
                });
            }

            return list;
        }
        public void Add(Persons p)
        {
            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = @"INSERT INTO persons 
                    (firstname, lastname, email, phone, dateofbirth)
                    VALUES(@fn, @ln, @em, @ph, @dob)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@fn", p.FirstName);
            cmd.Parameters.AddWithValue("@ln", p.LastName);
            cmd.Parameters.AddWithValue("@em", p.Email);
            cmd.Parameters.AddWithValue("@ph", p.Phone);
            cmd.Parameters.AddWithValue("@dob", p.Dob);

            cmd.ExecuteNonQuery();
        }
        public Persons? GetById(int id)
        {
            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = "SELECT * FROM persons WHERE id=@id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Persons
                {
                    Id = Convert.ToInt32(reader["id"]),
                    FirstName = Convert.ToString(reader["firstname"]) ?? "",
                    LastName = Convert.ToString(reader["lastname"]) ?? "",
                    Email = Convert.ToString(reader["email"]) ?? "",
                    Phone = Convert.ToString(reader["phone"]) ?? "",
                    Dob = reader["dateofbirth"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["dateofbirth"])
                };
            }

            return null;
        }
        public void Update(Persons p)
        {
            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = @"UPDATE persons SET firstname=@fn, lastname=@ln, email=@em, phone=@ph, dateofbirth=@dob WHERE id=@id";


            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.Parameters.AddWithValue("@fn", p.FirstName);
            cmd.Parameters.AddWithValue("@ln", p.LastName);
            cmd.Parameters.AddWithValue("@em", p.Email);
            cmd.Parameters.AddWithValue("@ph", p.Phone);
            cmd.Parameters.AddWithValue("@dob", p.Dob);


            cmd.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            using SqlConnection con = new SqlConnection(_connection);
            con.Open();

            string query = "DELETE FROM persons WHERE id=@id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}