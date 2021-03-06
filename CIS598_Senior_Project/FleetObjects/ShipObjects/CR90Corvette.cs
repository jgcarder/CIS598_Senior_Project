/* File: FiringArc.cs
 * Author: Jackson Carder
 */

using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;
using CIS598_Senior_Project.Collisions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    /// <summary>
    /// A class for the CR90's
    /// </summary>
    public class CR90Corvette : Ship
    {
        private int _hull;
        private int _command;
        private int _squadron;
        private int _engineering;
        private int _points;
        private bool _shipTypeA;
        private bool _engToken;
        private bool _navToken;
        private bool _sqdToken;
        private bool _firToken;
        private List<RedDie> _redAS;
        private List<BlueDie> _blueAS;
        private List<BlackDie> _blackAS;
        private List<DefenseToken> _defenseTokens;

        /// <summary>
        /// Ship's id
        /// </summary>
        public override int Id { get; }

        /// <summary>
        /// The total point cost for the ship including upgrades
        /// </summary>
        public override int PointCost
        {
            get
            {
                int x = _points;
                foreach (var upgd in Upgrades)
                {
                    if (upgd != null)
                    {
                        x += upgd.PointCost;
                    }
                }
                if (Title != null) x += Title.PointCost;
                if (Commander != null) x += Commander.PointCost;
                return x;
            }
        }

        /// <summary>
        /// The command value for the ship, ie the number of command dials that have to be set each round
        /// </summary>
        public override int Hull { get { return _hull; } set { _hull = value; } }

        /// <summary>
        /// The maximum hull for the ship
        /// </summary>
        public override int MaxHull { get { return 4; } }

        /// <summary>
        /// The command value for the ship, ie the number of command dials that have to be set each round
        /// </summary>
        public override int Command { get { return _command; } }

        /// <summary>
        /// The squadron value for the ship
        /// </summary>
        public override int Squadron { get { return _squadron; } }

        /// <summary>
        /// The ship's engineering value
        /// </summary>
        public override int Engineering { get { return _engineering; } }

        /// <summary>
        /// The ship's current speed
        /// </summary>
        public override int Speed { get; set; } = 1;

        /// <summary>
        /// The number of tokens the ship has in stock
        /// </summary>
        public override int TokenCount
        {
            get
            {
                int x = 0;
                if (HasConcentrateFireToken) x++;
                if (HasEngineeringToken) x++;
                if (HasNavigationToken) x++;
                if (HasSquadronToken) x++;

                return x;
            }
        }

        /// <summary>
        /// The movement table for the ship
        /// </summary>
        public override int[,] Movement
        {
            get
            {
                return new int[,]
                {
                    { -1, -1, -1, -1},
                    { 2, -1, -1, -1},
                    { 1, 2, -1, -1},
                    { 0, 1, 2, -1},
                    { 0, 1, 1, 2}
                };
            }
        }

        /// <summary>
        /// The types of upgrades available for the ship
        /// </summary>
        public override int[] UpgradeTypes
        {
            get
            {
                if(ShipTypeA)
                {
                    return new int[] { 1, 1, 0, 0, 0, 1, 0, 1 };
                }
                else
                {
                    return new int[] { 1, 1, 0, 0, 0, 0, 1, 1 };
                }
            }
        }

        /// <summary>
        /// If the ship has the fleet commander aboard
        /// </summary>
        public override bool HasCommander { get; set; } = false;

        /// <summary>
        /// The bool in charge of what ship varient this is
        /// </summary>
        public override bool ShipTypeA
        {
            get { return _shipTypeA; }
            set
            {
                _shipTypeA = value;
                if (_shipTypeA) _points = 44;
                else _points = 39;
                setHullDice(_shipTypeA);
            }
        }

        /// <summary>
        /// If the ship has an engineering token
        /// </summary>
        public override bool HasEngineeringToken
        {
            get
            {
                return _engToken;
            }
            set
            {
                if (TokenCount < Command) _engToken = value;
            }
        }

        /// <summary>
        /// if the ship has a squadron token
        /// </summary>
        public override bool HasSquadronToken
        {
            get
            {
                return _sqdToken;
            }
            set
            {
                if (TokenCount < Command) _sqdToken = value;
            }
        }

        /// <summary>
        /// if the ship has a nav token
        /// </summary>
        public override bool HasNavigationToken
        {
            get
            {
                return _navToken;
            }
            set
            {
                if (TokenCount < Command) _navToken = value;
            }
        }

        /// <summary>
        /// if the ship has a concentrate fire token
        /// </summary>
        public override bool HasConcentrateFireToken
        {
            get
            {
                return _firToken;
            }
            set
            {
                if (TokenCount < Command) _firToken = value;
            }
        }

        /// <summary>
        /// if it has been activated
        /// </summary>
        public override bool BeenActivated { get; set; } = false;

        /// <summary>
        /// The ship's name
        /// </summary>
        public override string Name { get { return "CR90 Corvette"; } }

        /// <summary>
        /// The queue of command dials for the ship
        /// </summary>
        public override Queue<CommandDialEnum> CommandDials { get; set; }

        /// <summary>
        /// The ship's title card
        /// </summary>
        public override UpgradeCard Title { get; set; }

        /// <summary>
        /// The commander of the fleet
        /// </summary>
        public override UpgradeCard Commander { get; set; }

        /// <summary>
        /// The ship's image
        /// </summary>
        public override Texture2D Image { get; } //to be set

        /// <summary>
        /// The potential source for images
        /// </summary>
        public override Rectangle Source { get; } //to be set

        /// <summary>
        /// The ships rotation, careful its backwards
        /// </summary>
        public override float Rotation { get; set; } = 0; //to be set

        /// <summary>
        /// The ship's position on the board
        /// </summary>
        public override Vector2 Position { get; set; } //to be set

        /// <summary>
        /// The ship's origin for rotation and placement
        /// </summary>
        public override Vector2 Origin { get { return new Vector2(51, 93); } }

        /// <summary>
        /// The ships center bounds
        /// </summary>
        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 36); } }

        /// <summary>
        /// The ships center bounds
        /// </summary>
        public override BoundingCircle BowBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 91, Rotation - MathHelper.PiOver2), 36); } }

        /// <summary>
        /// The ship's port bounds
        /// </summary>
        public override BoundingCircle PortBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation - MathHelper.Pi), 36); } }

        /// <summary>
        /// The ship's starboard bounds
        /// </summary>
        public override BoundingCircle StarboardBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation), 36); } }

        /// <summary>
        /// The ship'd aft bounds
        /// </summary>
        public override BoundingCircle AftBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 61, Rotation - (3 * MathHelper.PiOver2)), 36); } }

        /// <summary>
        /// The list of firing arcs
        /// </summary>
        public override FiringArc[] Arcs { get; set; }


        /// <summary>
        /// The list of upgrades
        /// </summary>
        public override UpgradeCard[] Upgrades { get; set; }

        /// <summary>
        /// The ship's defenses
        /// </summary>
        public override List<DefenseToken> DefenseTokens { get { return _defenseTokens; } }

        /// <summary>
        /// Red anti squadron dice
        /// </summary>
        public override List<RedDie> RedAS
        {
            get { return _redAS; }
        }

        /// <summary>
        /// blue anti squadron dice
        /// </summary>
        public override List<BlueDie> BlueAS
        {
            get { return _blueAS; }
        }

        /// <summary>
        /// black anti squadron dice
        /// </summary>
        public override List<BlackDie> BlackAS
        {
            get { return _blackAS; }
        }

        /// <summary>
        /// The ship's constructor
        /// </summary>
        /// <param name="id">The ship's id</param>
        /// <param name="content">The content manager for loading things</param>
        public CR90Corvette(int id, ContentManager content)
        {
            Upgrades = new UpgradeCard[8];

            Arcs = new FiringArc[4];
            Arcs[0] = new FiringArc(2, (Math.PI / 180) * 85);
            Arcs[1] = new FiringArc(2, (Math.PI / 180) * 95);
            Arcs[2] = new FiringArc(2, (Math.PI / 180) * 95);
            Arcs[3] = new FiringArc(1, (Math.PI / 180) * 85);

            _redAS = new List<RedDie>();
            _blueAS = new List<BlueDie>();
            _blackAS = new List<BlackDie>();

            ShipTypeA = true;

            Image = content.Load<Texture2D>("CR90CorvetteToken");

            _hull = 4;
            _command = 1;
            _squadron = 1;
            _engineering = 2;

            _navToken = false;
            _engToken = false;
            _firToken = false;
            _sqdToken = false;

            Id = id;
       
            _defenseTokens = new List<DefenseToken>();

            _blueAS.Add(new BlueDie(DieTypeEnum.Blue));

            _defenseTokens.Add(new RedirectDefenseToken(0));
            _defenseTokens.Add(new EvadeDefenseToken(0));
            _defenseTokens.Add(new EvadeDefenseToken(1));

            CommandDials = new Queue<CommandDialEnum>();
        }

        /// <summary>
        /// Refreshes the defense tokens
        /// </summary>
        public override void RefreshDefense()
        {
            foreach (var token in _defenseTokens) token.Reset();
        }

        /// <summary>
        /// Setting the hull dice
        /// </summary>
        /// <param name="shipA">For which type of ship it is</param>
        public override void setHullDice(bool shipA)
        {
            foreach (var arc in Arcs) arc.ClearDice();

            _redAS.Clear();
            _blueAS.Clear();
            _blackAS.Clear();

            _blueAS.Add(new BlueDie(DieTypeEnum.Blue));

            if (ShipTypeA)
            {
                Arcs[0].AddDice(2, DieTypeEnum.Red);
                Arcs[0].AddDice(1, DieTypeEnum.Blue);
                Arcs[1].AddDice(1, DieTypeEnum.Red);
                Arcs[1].AddDice(1, DieTypeEnum.Blue);
                Arcs[2].AddDice(1, DieTypeEnum.Red);
                Arcs[2].AddDice(1, DieTypeEnum.Blue);
                Arcs[3].AddDice(1, DieTypeEnum.Red);
            }
            else
            {
                Arcs[0].AddDice(3, DieTypeEnum.Blue);
                Arcs[1].AddDice(2, DieTypeEnum.Blue);
                Arcs[2].AddDice(2, DieTypeEnum.Blue);
                Arcs[3].AddDice(1, DieTypeEnum.Blue);
            }
        }

        /// <summary>
        /// Refreshes the ship's upgrades
        /// </summary>
        public override void refreshShip()
        {
            foreach (var upgrade in Upgrades)
            {
                if (upgrade != null)
                {
                    switch (upgrade.Name)
                    {
                        case "Enhanced Armament":
                            Arcs[1].AddDice(1, DieTypeEnum.Red);
                            Arcs[2].AddDice(1, DieTypeEnum.Red);
                            break;
                        case "Expanded Launchers":
                            Arcs[0].AddDice(2, DieTypeEnum.Black);
                            break;
                        case "Expanded Hangar Bay":
                            _squadron += 1;
                            break;
                        case "Engineering Team":
                            _engineering += 1;
                            break;
                    }
                }
            }
        }
    }
}
