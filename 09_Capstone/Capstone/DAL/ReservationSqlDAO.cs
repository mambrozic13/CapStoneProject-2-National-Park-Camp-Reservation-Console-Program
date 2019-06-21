﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationSqlDAO
    {
        private string connectionString;
        int reservationID;

        // Create a new sql-based reservation dao.
        public ReservationSqlDAO(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }
        // Create a list to hold the reservation information.
        public static IList<Reservation> reservationList = new List<Reservation>();

        public IList<Reservation> GetAllReservationInformation()
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM reservation", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation.Reservation_ID = Convert.ToInt32(reader["park_id"]);
                        reservation.Site_ID = Convert.ToInt32(reader["site_id"]);
                        reservation.Name = Convert.ToString(reader["name"]);
                        reservation.From_Date = Convert.ToInt32(reader["from_date"]);
                        reservation.To_Date = Convert.ToInt32(reader["area"]);
                        reservation.Create_Date = Convert.ToInt32(reader["create_date"]);
                        reservationList.Add(reservation);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"There was an error: {e.Message}.");
            }
            return reservationList;
        }

        public IList<Reservation> GetReservationsForCampground(Campground campground)
        {
            // Create a list to hold our campgrounds
            IList<Reservation> reservationList = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM reservation AS r INNER JOIN site AS s ON r.site_id = s.id INNER JOIN campground AS c ON s.campground_id = c.campground_id WHERE c.name = @name";
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();
                        reservation.Reservation_ID = Convert.ToInt32(reader["park_id"]);
                        reservation.Site_ID = Convert.ToInt32(reader["site_id"]);
                        reservation.Name = Convert.ToString(reader["name"]);
                        reservation.From_Date = Convert.ToInt32(reader["from_date"]);
                        reservation.To_Date = Convert.ToInt32(reader["area"]);
                        reservation.Create_Date = Convert.ToInt32(reader["create_date"]);
                        reservationList.Add(reservation);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"There was an error: {ex.Message}.");
            }
            return reservationList;
        }


        public int CreateNewReservation(Site site, string name, DateTime arrivalDate, DateTime departureDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO reservation (site_id, name, from_date, to_date) VALUES (@siteID, @name, @arrivalDate, @departureDate)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteID", site.Site_ID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);
                    cmd.ExecuteNonQuery();

                    string sql2 = "SELECT reservation_id FROM reservation WHERE name = @name";
                    cmd = new SqlCommand(sql2, conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reservationID = Convert.ToInt32(reader["reservation_id"]);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"There was an error: {e.Message}.");
            }
            return reservationID;
        }
    }
}