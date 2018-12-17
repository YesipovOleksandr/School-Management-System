﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskDomKofe.Model.Entities;

namespace TestTaskDomKofe.Model
{
    public class StudentManager
    {
        public int InsertSubjects(Students students)
        {
            string sqlQuery = String.Format("Insert into Students (FIO,YearOfStudy,Address,Phone,Class_id) Values('{0}','{1}','{2}','{3}','{4}');"
           + "Select @@Identity", students.FIO, students.YearOfStudy, students.Address, students.Phone, students.Class_id);
            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString);
            connection.Open();
            //Create a Command object
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            //Execute the command to SQL Server and return the newly created ID
            int newclasseID = Convert.ToInt32((decimal)command.ExecuteScalar());
            //Close and dispose
            command.Dispose();
            connection.Close();
            connection.Dispose();
            // Set return value
            return newclasseID;
        }
        public List<Students> GetStudents()
        {

            List<Students> result = new List<Students>();

            //Create the SQL Query for returning all the articles
            string sqlQuery = String.Format("select * from Students");

            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(sqlQuery, connection);

            //Create DataReader for storing the returning table into server memory
            SqlDataReader dataReader = command.ExecuteReader();

            Students article = null;

            //load into the result object the returned row from the database
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    article = new Students();
                    article.Id = Convert.ToInt32(dataReader["Id"]);
                    article.FIO = dataReader["FIO"].ToString();
                    article.YearOfStudy = Convert.ToDateTime(dataReader["YearOfStudy"].ToString());
                    article.Address = dataReader["Address"].ToString();
                    article.Phone = dataReader["Phone"].ToString();
                    article.Class_id = Convert.ToInt32(dataReader["Class_id"]);
                    result.Add(article);
                }
            }

            return result;

        }
        public bool DeleteStudents(int studentsID)
        {
            bool result = false;

            //Create the SQL Query for deleting an article
            string sqlQuery = String.Format("delete from Students where Id = {0}", studentsID);

            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString);
            connection.Open();

            //Create a Command object
            SqlCommand command = new SqlCommand(sqlQuery, connection);

            // Execute the command
            int rowsDeletedCount = command.ExecuteNonQuery();
            if (rowsDeletedCount != 0)
                result = true;
            // Close and dispose
            command.Dispose();
            connection.Close();
            connection.Dispose();


            return result;
        }
        public int UpdateStudents(Students students)
        {

            //Create the SQL Query for inserting an article

            string createQuery = String.Format("Insert into Students (FIO,YearOfStudy,Address,Phone,Class_id) Values('{0}','{1}','{2}','{3}','{4}');"
       + "Select @@Identity", students.FIO, students.YearOfStudy, students.Address, students.Phone, students.Class_id);

            string updateQuery = String.Format("Update Students SET FIO='{0}',YearOfStudy='{1}',Address='{2}',Phone='{3}',Class_id='{4}'  Where Id = {5};",
               students.FIO, students.YearOfStudy, students.Address, students.Phone, students.Class_id, students.Id);

            //Create and open a connection to SQL Server 
            SqlConnection connection = new SqlConnection(DatabaseHelper.ConnectionString);
            connection.Open();

            //Create a Command object
            SqlCommand command = null;

            if (students.Id != 0)
                command = new SqlCommand(updateQuery, connection);
            else
                command = new SqlCommand(createQuery, connection);

            int savedArticleID = 0;
            try
            {
                //Execute the command to SQL Server and return the newly created ID
                var commandResult = command.ExecuteScalar();
                if (commandResult != null)
                {
                    savedArticleID = Convert.ToInt32(commandResult);
                }
                else
                {
                    //the update SQL query will not return the primary key but if doesn't throw exception 
                    //then we will take it from the already provided data
                    savedArticleID = students.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //there was a problem executing the script
            }

            //Close and dispose
            command.Dispose();
            connection.Close();
            connection.Dispose();

            return savedArticleID;
        }
    }

}