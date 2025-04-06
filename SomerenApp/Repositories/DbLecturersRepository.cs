﻿using Microsoft.Data.SqlClient;
using SomerenApp.Models;
using System.Data;
using System.Diagnostics;

namespace SomerenApp.Repositories
{
    public class DbLecturersRepository : ILecturersRepository
    {
        private readonly string? _connectionString;
        public DbLecturersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SomerenDatabase");
        }
        public List<Lecturer> GetAllLecturers()
        {
            List<Lecturer> lecturers = new List<Lecturer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT lecturerNumber, firstName, lastName, age, phoneNumber FROM lecturers ORDER BY lastName";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    lecturers.Add(lecturer);
                }
            }
            return lecturers;
        }
        private Lecturer ReadLecturer(SqlDataReader reader)
        {
            int lecturerNumber = (int)reader["lecturerNumber"];
            string firstName = (string)reader["firstName"];
            string lastName = (string)reader["lastName"];
            byte age = (byte)reader["age"];
            string phoneNumber = (string)reader["phoneNumber"];
            return new Lecturer(lecturerNumber, firstName, lastName,  age, phoneNumber);
        }
        public Lecturer? GetLecturerByID(int lecturerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT lastName, firstName, age, phoneNumber, lecturerNumber, roomId FROM lecturers WHERE lecturerNumber = @LecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LecturerNumber", lecturerNumber);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    return lecturer;
                }
                else
                {
                    throw new Exception("No lecturer was found by given lecturerNumber.");
                }
            }
        }

        public void AddLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO lecturers (firstName, lastName, age, phoneNumber)" +
                    "VALUES(@FirstName, @LastName, @Age, @PhoneNumber); " +
                    "SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Connection.Open();
                lecturer.LecturerNumber = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void EditLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE lecturers SET firstName = @FirstName, lastName = @LastName, " +
                    "age = @Age, phoneNumber = @PhoneNumber WHERE lecturerNumber = @LecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                command.Parameters.AddWithValue("@Age", lecturer.Age);
                command.Parameters.AddWithValue("@PhoneNumber", lecturer.PhoneNumber);
                command.Parameters.AddWithValue("@LecturerNumber", lecturer.LecturerNumber);
                command.Connection.Open();

                int nrofRowsAffected = command.ExecuteNonQuery();
                if (nrofRowsAffected == 0)
                {
                    throw new Exception("No records updated!");
                }
            }
        }

        public void DeleteLecturer(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM lecturers WHERE lecturerNumber = @LecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LecturerNumber", lecturer.LecturerNumber);
                command.Connection.Open();
                int nrofRowsAffected = command.ExecuteNonQuery();
                if (nrofRowsAffected == 0)
                {
                    throw new Exception("No records updated!");
                }
            }
        }
        public List<Lecturer> GetSupervisors(int activityNumber)
        {
            return AddLecturersToActivityList("SELECT * FROM lecturers WHERE lecturerNumber NOT IN  (SELECT lecturerNumber FROM accompaniments WHERE activityNumber = @ActivityNumber;);", activityNumber);
        }
        public List<Lecturer> GetNonSupervisors(int activityNumber)
        {
            return AddLecturersToActivityList("SELECT lecturerNumber FROM accompaniments WHERE @ActivityNumber = @ActivityNumber;", activityNumber);
        }
        public List<Lecturer> AddLecturersToActivityList(string query, int activityNumber)
        {
            List<Lecturer> lecturers = new List<Lecturer>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activityNumber);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    lecturers.Add(lecturer);
                }
            }
            return lecturers;
        }

        public void RemoveSuperVisor(int activityNumber, int lecturerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM accompaniments WHERE activityNumber = @ActivityNumber AND lecturerNumber = @LecturerNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activityNumber);
                command.Parameters.AddWithValue("@LecturerNumber", lecturerNumber);
                command.Connection.Open();
                int nrofRowsAffected = command.ExecuteNonQuery();
                if (nrofRowsAffected == 0)
                {
                    throw new Exception("No records updated!");
                }
            }
        }
        public void AddSuperVisor(int activityNumber, int lecturerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"INSERT INTO accompaniments (activityNumber, lecturerNumber)" +
                    "VALUES(@ActivityNumber, @LecturerNumber);";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActivityNumber", activityNumber);
                command.Parameters.AddWithValue("@LecturerNumber", lecturerNumber);
                command.Connection.Open();
            }
        }
    }
}
