using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using API.Infrastructure.Errors;
using Core.DTO;
using Entities;
using EventBus.Messages.Events;
using MassTransit;
using Npgsql;

namespace Core.Repositories
{
    public class UserRegistrationRepository:IUserRegistrationRepository
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public UserRegistrationRepository(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        private const string DatabaseConnection = "Host=localhost;Port=5432;Username=postgres;Password=admin;Database=user_registration;";

        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();  
            using (NpgsqlConnection connection = new NpgsqlConnection(DatabaseConnection))  
            {  
                NpgsqlDataAdapter dataAdapter = new  NpgsqlDataAdapter();
                DataSet dataSet = new DataSet();
                NpgsqlCommand command = new NpgsqlCommand("getAll", connection);  
                command.CommandType = CommandType.StoredProcedure;  
                
                dataAdapter.SelectCommand = command;
                
                connection.Open();

                dataAdapter.Fill(dataSet,"Users");
                
                foreach (DataRow row in dataSet.Tables["Users"].Rows)
                {
                    User user = new User();  
                    user.Id = (Guid)row["Id"];  
                    user.FirstName = row["FirstName"].ToString();  
                    user.LastName = row["LastName"].ToString();  
                    user.Email = row["Email"].ToString();
                    users.Add(user);
                }
         
                connection.Close();  
            }  
            return users;
        }

        public bool Delete(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(DatabaseConnection))  
            {
                const string commandText = "Delete From \"Users\" Where \"Id\" = @ID;";
                NpgsqlCommand command = new NpgsqlCommand(commandText, connection);  
                command.Parameters.AddWithValue("@Id", id);  
                connection.Open();
              
                int affectedRaws = command.ExecuteNonQuery();
                
                connection.Close();

                if(affectedRaws != 1)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "There is no user whit this id." });
            }

            UserRegistratedEvent userRegistratedEvent = new UserRegistratedEvent {Message = "User is deleted successfully."};
            _publishEndpoint.Publish(userRegistratedEvent);
            return true;
        }

        public bool Create(UserDto user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(DatabaseConnection))
            {
             
                // const string commandText = "Insert Into Users (FirstName,LastName,Email) VALUES ()";
                const string commandText = "Insert Into \"Users\" (\"Id\",\"FirstName\", \"LastName\", \"Email\") VALUES (@Id,@FirstName, @LastName, @Email)"; 
                NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection);  
                // cmd.CommandType = CommandType.StoredProcedure;  
  
                cmd.Parameters.AddWithValue("@Id",Guid.NewGuid());  
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);  
                cmd.Parameters.AddWithValue("@LastName", user.LastName);  
                cmd.Parameters.AddWithValue("@Email", user.Email);  

                connection.Open();  
                
                NpgsqlCommand checkUserDublicateEmail = new NpgsqlCommand("SELECT COUNT(*) FROM \"Users\" WHERE \"Email\" = @Email" , connection);
                checkUserDublicateEmail.Parameters.AddWithValue("@Email", user.Email);
                
                var userExist = (long)checkUserDublicateEmail.ExecuteScalar();

                if (userExist > 0)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "User with this email is already exist." });
                }
               
                cmd.ExecuteNonQuery();  
                
                connection.Close();  
            } 
            
            UserRegistratedEvent userRegistratedEvent = new UserRegistratedEvent {Message = @"User is created successfully"};
            _publishEndpoint.Publish(userRegistratedEvent);
            return true;
        }
    }
}