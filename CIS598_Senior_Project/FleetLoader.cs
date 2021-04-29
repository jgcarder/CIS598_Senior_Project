using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.FleetObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;

namespace CIS598_Senior_Project
{
    public static class FleetLoader
    {

        /// <summary>
        /// Saves an entire fleet to a local file.
        /// </summary>
        /// <param name="fleet">The fleet to save</param>
        /// <returns>True if the save was successful and false otherwise</returns>
        public static bool SaveFleet(Fleet fleet)
        {
            try
            {
                using StreamWriter sw = new StreamWriter(fleet.Name + ".txt");

                sw.WriteLine(fleet.Name);
                if (fleet.IsRebelFleet) sw.WriteLine("Rebel");
                else sw.WriteLine("Imperial");

                foreach(var ship in fleet.Ships)
                {
                    switch(ship.Name)
                    {
                        case "Assault Frigate Mark II":
                            sw.WriteLine(">," + ship.Name + "," + ship.ShipTypeA);
                            writeUpgrades(sw, ship);
                            break;
                        case "CR90 Corvette":
                            sw.WriteLine(">," + ship.Name + "," + ship.ShipTypeA);
                            writeUpgrades(sw, ship);
                            break;
                        case "Nebulon-B Frigate":
                            sw.WriteLine(">," + ship.Name + "," + ship.ShipTypeA);
                            writeUpgrades(sw, ship);
                            break;
                        case "Gladiator Star Destroyer":
                            sw.WriteLine(">," + ship.Name + "," + ship.ShipTypeA);
                            writeUpgrades(sw, ship);
                            break;
                        case "Victory Star Destroyer":
                            sw.WriteLine(">," + ship.Name + "," + ship.ShipTypeA);
                            writeUpgrades(sw, ship);
                            break;
                    }
                }

                int[] squads = returnSquads(fleet);
                if(fleet.IsRebelFleet)
                {
                    if (squads[0] > 0) sw.WriteLine(">>>,A-Wing Squadron," + squads[0]);
                    if (squads[1] > 0) sw.WriteLine(">>>,B-Wing Squadron" + squads[1]);
                    if (squads[2] > 0) sw.WriteLine(">>>,X-Wing Squadron" + squads[2]);
                    if (squads[3] > 0) sw.WriteLine(">>>,Y-Wing Squadron" + squads[3]);
                }
                else
                {
                    if (squads[0] > 0) sw.WriteLine(">>>,TIE Fighter Squadron," + squads[0]);
                    if (squads[1] > 0) sw.WriteLine(">>>,TIE Advanced Squadron" + squads[1]);
                    if (squads[2] > 0) sw.WriteLine(">>>,TIE Interceptor Squadron" + squads[2]);
                    if (squads[3] > 0) sw.WriteLine(">>>,TIE Bomber Squadron" + squads[3]);
                }
                sw.Close();

                using StreamWriter sw2 = new StreamWriter("Fleets.txt", true);
                sw2.WriteLine(fleet.Name + "," + fleet.IsRebelFleet);
                sw2.Close();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Loads the requested fleet
        /// </summary>
        /// <param name="fileName">The name of the fleet to be deleted without attached IsRebelFleet bool by comma</param>
        /// <returns></returns>
        public static Fleet LoadFleet(string fileName)
        {
            Fleet fleet = new Fleet(fileName);
            try
            {
                using StreamReader sr = new StreamReader(fileName + ".txt");
                string line = sr.ReadLine();
                while(line != null)
                {
                    //Filter and add things to fleet

                    line = sr.ReadLine();
                }
                return fleet;
            }
            catch(Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Deletes the fleet from the save files
        /// </summary>
        /// <param name="fleetName">The name of the fleet to be deleted with attached IsRebelFleet bool by comma</param>
        /// <returns></returns>
        public static bool DeleteFleet(string fleetName)
        {
            try
            {
                string name = fleetName.Split(',')[0];
                File.Delete(name + ".txt");

                List<string> fleets = AvailableFleets();
                File.Create("Fleets.txt").Close();
                using StreamWriter sw = new StreamWriter("Fleets.txt");
                foreach(var s in fleets)
                {
                    if(!s.Split(',')[0].Equals(name))
                    {
                        sw.WriteLine(s);
                    }
                }
                sw.Close();

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns the list of available fleets to load
        /// </summary>
        /// <returns></returns>
        public static List<string> AvailableFleets()
        {
            List<string> fleets = new List<string>();
            try
            {
                using StreamReader sr = new StreamReader("Fleets.txt");
                string line = sr.ReadLine();

                while(line != null)
                {
                    fleets.Add(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                return fleets;
            }
            catch(Exception e)
            {
                MessageBox.Show("Exception: " + e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Writes the upgrades of a ship into the designated file
        /// </summary>
        /// <param name="sw">The streamWriter to the file</param>
        /// <param name="ship">The ship whose upgrades need to be written</param>
        private static void writeUpgrades(StreamWriter sw, Ship ship)
        {
            if (ship.Commander != null) sw.WriteLine(">>," + ship.Commander.Name);
            if (ship.Title != null) sw.WriteLine(">>," + ship.Title.Name);
            for(int i = 0; i < ship.Upgrades.Length; i++)
            {
                if (ship.Upgrades[i] != null) sw.WriteLine(">>," + ship.Upgrades[i].Name);
            }
        }

        /// <summary>
        /// gets the number of squads of each type
        /// </summary>
        /// <returns>the list of number of squads by type</returns>
        private static int[] returnSquads(Fleet fleet)
        {
            int[] result = new int[4];
            if (fleet.IsRebelFleet)
            {
                result[0] += fleet.Squadrons.FindAll(x => x.Name == "A-Wing Squadron").Count;
                result[1] += fleet.Squadrons.FindAll(x => x.Name == "B-Wing Squadron").Count;
                result[2] += fleet.Squadrons.FindAll(x => x.Name == "X-Wing Squadron").Count;
                result[3] += fleet.Squadrons.FindAll(x => x.Name == "Y-Wing Squadron").Count;
            }
            else
            {
                result[0] += fleet.Squadrons.FindAll(x => x.Name == "TIE Fighter Squadron").Count;
                result[1] += fleet.Squadrons.FindAll(x => x.Name == "TIE Advanced Squadron").Count;
                result[2] += fleet.Squadrons.FindAll(x => x.Name == "TIE Interceptor Squadron").Count;
                result[3] += fleet.Squadrons.FindAll(x => x.Name == "TIE Bomber Squadron").Count;
            }

            return result;
        }
    }
}
