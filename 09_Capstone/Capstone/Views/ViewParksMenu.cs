﻿using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Views
{
    public class ViewParksMenu : CLIMenu
    {
        IList<Park> parkList;

        public ViewParksMenu()
        {
            IParkSqlDAO parkDAO = new ParkSqlDAO(ConnectionString);
            try
            {
                this.Title = "***Select A Park For Further Details***";
                parkList = parkDAO.GetAllParks();
                foreach(Park park in parkList)
                {
                    this.menuOptions.Add($"{park.Park_ID}", $"{park.Name}");
                }
                this.menuOptions.Add("Q", "Quit");
            }
            catch(Exception e)
            {
                Console.WriteLine($"There was an error: {e.Message}.");
            }
        }
        protected override bool ExecuteSelection(string choice)
        {
            
            switch (choice)
            {
                case "Q":
                    break;
                 default:
                    int index = int.Parse(choice) - 1;
                    Park park = parkList.ElementAt(index);
                    DisplayInfoForPark(park);
                    ParkInfoMenu menu = new ParkInfoMenu(parkDAO, park);
                    menu.Run();
                  break;
            }

            return true;
        }
        public void DisplayInfoForPark(Park park)
        {
            Console.Clear();
            Console.WriteLine("Park Information Screen");
            Console.WriteLine($"{park.Name} National Park");
            Console.WriteLine($"Location: {park.Location}");
            Console.WriteLine($"Established: {park.Establish_date}");
            Console.WriteLine($"Area: {park.Area} sq km");
            Console.WriteLine($"Annual Visitors: {park.Visitors}");
            Console.WriteLine("");
            Console.WriteLine($"{park.Description}");

            Console.ReadKey();
        }
    }
}
