/* File: FleetLoader.cs
 * Author: Jackson Carder
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CIS598_Senior_Project.FleetObjects;
using CIS598_Senior_Project.FleetObjects.ShipObjects;
using CIS598_Senior_Project.FleetObjects.SquadronObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;

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
                    if (squads[1] > 0) sw.WriteLine(">>>,B-Wing Squadron," + squads[1]);
                    if (squads[2] > 0) sw.WriteLine(">>>,X-Wing Squadron," + squads[2]);
                    if (squads[3] > 0) sw.WriteLine(">>>,Y-Wing Squadron," + squads[3]);
                }
                else
                {
                    if (squads[0] > 0) sw.WriteLine(">>>,TIE Fighter Squadron," + squads[0]);
                    if (squads[1] > 0) sw.WriteLine(">>>,TIE Advanced Squadron," + squads[1]);
                    if (squads[2] > 0) sw.WriteLine(">>>,TIE Interceptor Squadron," + squads[2]);
                    if (squads[3] > 0) sw.WriteLine(">>>,TIE Bomber Squadron," + squads[3]);
                }
                sw.Close();

                List<string> list = AvailableFleets();

                using StreamWriter sw2 = new StreamWriter("Fleets.txt", true);
                bool record = recordName(fleet.Name + "," + fleet.IsRebelFleet, sw2, list);
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
        public static Fleet LoadFleet(string fileName, ContentManager content)
        {
            Fleet fleet = new Fleet(fileName);
            string[] split;
            Ship selectedShip = null;
            int id = 0;
            try
            {
                using StreamReader sr = new StreamReader(fileName + ".txt");
                
                string line = sr.ReadLine();
                fleet.Name = line;

                line = sr.ReadLine();
                if (line.Equals("Rebel")) fleet.IsRebelFleet = true;
                else if (line.Equals("Imperial")) fleet.IsRebelFleet = false;

                line = sr.ReadLine();

                while (line != null)
                {
                    split = line.Split(',');

                    switch(split[0])
                    {
                        case ">": //ship
                            if (selectedShip != null) fleet.Ships.Add(selectedShip); selectedShip = null;

                            switch(split[1])
                            {
                                case "Assault Frigate Mark II":
                                    selectedShip = new AssaultFrigateMarkII(id, content);
                                    break;
                                case "CR90 Corvette":
                                    selectedShip = new CR90Corvette(id, content);
                                    break;
                                case "Nebulon-B Frigate":
                                    selectedShip = new NebulonBFrigate(id, content);
                                    break;
                                case "Gladiator Star Destroyer":
                                    selectedShip = new GladiatorStarDestroyer(id, content);
                                    break;
                                case "Victory Star Destroyer":
                                    selectedShip = new VictoryStarDestroyer(id, content);
                                    break;
                            }
                            if (split[2].Equals("True")) selectedShip.ShipTypeA = true;
                            else if (split[2].Equals("False")) selectedShip.ShipTypeA = false;
                            break;
                        case ">>": //upgrade
                            switch(split[1])
                            {
                                case "Advanced Projectors":
                                    selectedShip.Upgrades[7] = new AdvancedProjectors(content);
                                    break;
                                case "Assault Concussion Missiles":
                                    selectedShip.Upgrades[3] = new AssaultConcussionMissiles(content);
                                    break;
                                case "Admiral Motti":
                                    selectedShip.Commander = new CommanderAdmiralMotti(content);
                                    break;
                                case "Admiral Screed":
                                    selectedShip.Commander = new CommanderAdmiralScreed(content);
                                    break;
                                case "Garm Bel Iblis":
                                    selectedShip.Commander = new CommanderGarmBelIblis(content);
                                    break;
                                case "General Dodonna":
                                    selectedShip.Commander = new CommanderGeneralDodonna(content);
                                    break;
                                case "Grand Moff Tarkin":
                                    selectedShip.Commander = new CommanderGrandMoffTarkin(content);
                                    break;
                                case "Mon Mothma":
                                    selectedShip.Commander = new CommanderMonMothma(content);
                                    break;
                                case "Electronic Countermeasures":
                                    selectedShip.Upgrades[7] = new ElectronicCountermeasures(content);
                                    break;
                                case "Engineering Team":
                                    selectedShip.Upgrades[1] = new EngineeringTeam(content);
                                    break;
                                case "Engine Techs":
                                    selectedShip.Upgrades[1] = new EngineTechs(content);
                                    break;
                                case "Enhanced Armament":
                                    selectedShip.Upgrades[5] = new EnhancedArmament(content);
                                    break;
                                case "Expanded Hangar Bays":
                                    selectedShip.Upgrades[4] = new ExpandedHangarBay(content);
                                    break;
                                case "Expanded Launchers":
                                    selectedShip.Upgrades[3] = new ExpandedLaunchers(content);
                                    break;
                                case "Flight Controllers":
                                    selectedShip.Upgrades[2] = new FlightControllers(content);
                                    break;
                                case "Gunnery Team":
                                    selectedShip.Upgrades[2] = new GunneryTeam(content);
                                    break;
                                case "H9 Turbolasers":
                                    selectedShip.Upgrades[5] = new H9Turbolasers(content);
                                    break;
                                case "Ion Cannon Batteries":
                                    selectedShip.Upgrades[6] = new IonCannonBatteries(content);
                                    break;
                                case "Leading Shots":
                                    selectedShip.Upgrades[6] = new LeadingShots(content);
                                    break;
                                case "Nav Team":
                                    selectedShip.Upgrades[1] = new NavTeam(content);
                                    break;
                                case "Adar Tallon":
                                    selectedShip.Upgrades[0] = new OfficerAdarTallon(content);
                                    break;
                                case "Admiral Chiraneau":
                                    selectedShip.Upgrades[0] = new OfficerAdmiralChiraneau(content);
                                    break;
                                case "Defense Liaison":
                                    selectedShip.Upgrades[0] = new OfficerDefenseLiaison(content);
                                    break;
                                case "Director Isard":
                                    selectedShip.Upgrades[0] = new OfficerDirectorIsard(content);
                                    break;
                                case "Intel Officer":
                                    selectedShip.Upgrades[0] = new OfficerIntelOfficer(content);
                                    break;
                                case "Leia Organa":
                                    selectedShip.Upgrades[0] = new OfficerLeiaOrgana(content);
                                    break;
                                case "Raymus Antilles":
                                    selectedShip.Upgrades[0] = new OfficerRaymusAntilles(content);
                                    break;
                                case "Veteran Captain":
                                    selectedShip.Upgrades[0] = new OfficerVeteranCaptain(content);
                                    break;
                                case "Weapons Liaison":
                                    selectedShip.Upgrades[0] = new OfficerWeaponsLiaison(content);
                                    break;
                                case "Wullf Yularen":
                                    selectedShip.Upgrades[0] = new OfficerWullfYularen(content);
                                    break;
                                case "Overload Pulse":
                                    selectedShip.Upgrades[6] = new OverloadPulse(content);
                                    break;
                                case "Point-Defense Reroute":
                                    selectedShip.Upgrades[4] = new PointDefenseReroute(content);
                                    break;
                                case "Sensor Team":
                                    selectedShip.Upgrades[2] = new SensorTeam(content);
                                    break;
                                case "Corrupter":
                                    selectedShip.Title = new TitleCorrupter(content);
                                    break;
                                case "Demolisher":
                                    selectedShip.Title = new TitleDemolisher(content);
                                    break;
                                case "Dodonna's Pride":
                                    selectedShip.Title = new TitleDodonnasPride(content);
                                    break;
                                case "Dominator":
                                    selectedShip.Title = new TitleDominator(content);
                                    break;
                                case "Gallant Haven":
                                    selectedShip.Title = new TitleGallantHaven(content);
                                    break;
                                case "Insidious":
                                    selectedShip.Title = new TitleInsidious(content);
                                    break;
                                case "Jaina's Light":
                                    selectedShip.Title = new TitleJainasLight(content);
                                    break;
                                case "Paragon":
                                    selectedShip.Title = new TitleParagon(content);
                                    break;
                                case "Redemption":
                                    selectedShip.Title = new TitleRedemption(content);
                                    break;
                                case "Salvation":
                                    selectedShip.Title = new TitleSalvation(content);
                                    break;
                                case "Tantive IV":
                                    selectedShip.Title = new TitleTantiveIV(content);
                                    break;
                                case "Warlord":
                                    selectedShip.Title = new TitleWarlord(content);
                                    break;
                                case "Yavaris":
                                    selectedShip.Title = new TitleYavaris(content);
                                    break;
                                case "XI7 Turbolasers":
                                    selectedShip.Upgrades[5] = new XI7Turbolasers(content);
                                    break;
                                case "XX-9 Turbolasers":
                                    selectedShip.Upgrades[5] = new XX9Turbolasers(content);
                                    break;
                            }
                            break;
                        case ">>>": //squadron
                            if (selectedShip != null) fleet.Ships.Add(selectedShip); selectedShip = null;

                            switch (split[1])
                            {
                                case "A-Wing Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "B-Wing Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "X-Wing Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "Y-Wing Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "TIE Fighter Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "TIE Advanced Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "TIE Interceptor Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                                case "TIE Bomber Squadron":
                                    addSquadrons(split[1], int.Parse(split[2]), id, content, fleet);
                                    break;
                            }
                            break;
                    }


                    line = sr.ReadLine();
                    id++;
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
        /// <returns>true if successful, false otherwise</returns>
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

        /// <summary>
        /// Adds the indicated squadrons to the fleet
        /// </summary>
        /// <param name="squad">The name of the squadron to add</param>
        /// <param name="num">The number of squadrons to add</param>
        /// <param name="id">The starting id</param>
        /// <param name="content">The Content Manager</param>
        /// <param name="fleet">The fleet to add things to</param>
        private static void addSquadrons(string squad, int num, int id, ContentManager content, Fleet fleet)
        {
            switch (squad)
            {
                case "A-Wing Squadron":
                    for(int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new AWingSquadron(id, content));
                        id++;
                    }
                    break;
                case "B-Wing Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new BWingSquadron(id, content));
                        id++;
                    }
                    break;
                case "X-Wing Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new XWingSquadron(id, content));
                        id++;
                    }
                    break;
                case "Y-Wing Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new YWingSquadron(id, content));
                        id++;
                    }
                    break;
                case "TIE Fighter Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new TIEFighterSquadron(id, content));
                        id++;
                    }
                    break;
                case "TIE Advanced Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new TIEAdvancedSquadron(id, content));
                        id++;
                    }
                    break;
                case "TIE Interceptor Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new TIEInterceptorSquadron(id, content));
                        id++;
                    }
                    break;
                case "TIE Bomber Squadron":
                    for (int i = 0; i < num; i++)
                    {
                        fleet.Squadrons.Add(new TIEBomberSquadron(id, content));
                        id++;
                    }
                    break;
            }
        }

        /// <summary>
        /// Checks if a fleet name is already in use in the file system
        /// </summary>
        /// <param name="name">The name to check for with bool attached</param>
        /// <param name="sw">The stream writer that writes to the files</param>
        /// <returns>true if already exists, false if new</returns>
        private static bool recordName(string name, StreamWriter sw, List<string> names)
        {
            foreach(var s in names)
            {
                if(s.Equals(name))
                {
                    return true;
                }
            }

            sw.WriteLine(name);
            return false;
        }
    }
}
