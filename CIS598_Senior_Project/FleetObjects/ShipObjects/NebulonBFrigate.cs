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
    public class NebulonBFrigate : Ship
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

        public override int Id { get; }

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

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int Command { get { return _command; } }

        public override int Squadron { get { return _squadron; } }

        public override int Engineering { get { return _engineering; } }

        public override int Speed { get; set; } = 1;

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

        public override int[,] Movement
        {
            get
            {
                return new int[,]
                {
                    {-1, -1, -1, -1},
                    {1, -1, -1, -1},
                    {1, 1, -1, -1},
                    {0, 1, 2, -1},
                    {-1, -1, -1, -1}
                };
            }
        }

        public override int[] UpgradeTypes { get { return new int[] { 1, 1, 0, 0, 0, 1, 0, 0 }; } }

        public override bool HasCommander { get; set; } = false;

        public override bool ShipTypeA
        {
            get { return _shipTypeA; }
            set
            {
                _shipTypeA = value;
                if (_shipTypeA) _points = 57;
                else _points = 51;
                setHullDice(_shipTypeA);
            }
        }

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

        public override bool BeenActivated { get; set; } = false;

        public override string Name { get { return "Nebulon-B Frigate"; } }

        public override Queue<CommandDialEnum> CommandDials { get; set; }

        public override UpgradeCard Title { get; set; }

        public override UpgradeCard Commander { get; set; }

        public override Texture2D Image { get; } //to be set

        public override Rectangle Source { get; } //to be set

        public override float Rotation { get; set; } = 0; //to be set

        public override Vector2 Position { get; set; } //to be set

        public override Vector2 Origin { get { return new Vector2(51, 72); } }

        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 36); } }

        public override BoundingCircle BowBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 71, Rotation + MathHelper.PiOver2), 36); } }

        public override BoundingCircle PortBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation + MathHelper.Pi), 36); } }

        public override BoundingCircle StarboardBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation), 36); } }

        public override BoundingCircle AftBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 81, Rotation + 3 * MathHelper.PiOver2), 36); } }

        public override FiringArc[] Arcs { get; }

        public override UpgradeCard[] Upgrades { get; set; }

        public override List<DefenseToken> DefenseTokens { get { return _defenseTokens; } }

        public override List<RedDie> RedAS
        {
            get { return _redAS; }
        }

        public override List<BlueDie> BlueAS
        {
            get { return _blueAS; }
        }

        public override List<BlackDie> BlackAS
        {
            get { return _blackAS; }
        }

        public NebulonBFrigate(int id, ContentManager content)
        {
            Upgrades = new UpgradeCard[8];

            Arcs = new FiringArc[4];
            Arcs[0] = new FiringArc(3, (Math.PI / 180) * 55);
            Arcs[1] = new FiringArc(1, (Math.PI / 180) * 125);
            Arcs[2] = new FiringArc(1, (Math.PI / 180) * 125);
            Arcs[3] = new FiringArc(2, (Math.PI / 180) * 55);

            _redAS = new List<RedDie>();
            _blueAS = new List<BlueDie>();
            _blackAS = new List<BlackDie>();

            ShipTypeA = true;

            Image = content.Load<Texture2D>("NebulonBToken");

            _hull = 5;
            _command = 2;
            if (ShipTypeA) _squadron = 2;
            else _squadron = 1;
            _engineering = 3;

            _navToken = false;
            _engToken = false;
            _firToken = false;
            _sqdToken = false;

            Id = id;

            _defenseTokens = new List<DefenseToken>();

            _blueAS.Add(new BlueDie(DieTypeEnum.Blue));
            _blueAS.Add(new BlueDie(DieTypeEnum.Blue));

            _defenseTokens.Add(new BraceDefenseToken(0));
            _defenseTokens.Add(new BraceDefenseToken(1));
            _defenseTokens.Add(new EvadeDefenseToken(0));

            CommandDials = new Queue<CommandDialEnum>();
        }

        public override void RefreshDefense()
        {
            foreach (var token in _defenseTokens) token.Reset();
        }

        public override void setHullDice(bool shipA)
        {
            foreach (var arc in Arcs) arc.ClearDice();

            _redAS.Clear();
            _blueAS.Clear();
            _blackAS.Clear();

            Arcs[0].AddDice(3, DieTypeEnum.Red);
            Arcs[1].AddDice(1, DieTypeEnum.Red);
            Arcs[1].AddDice(1, DieTypeEnum.Blue);
            Arcs[2].AddDice(1, DieTypeEnum.Red);
            Arcs[2].AddDice(1, DieTypeEnum.Blue);
            Arcs[3].AddDice(2, DieTypeEnum.Red);

            if (ShipTypeA)
            {
                _blueAS.Add(new BlueDie(DieTypeEnum.Blue));
                _blueAS.Add(new BlueDie(DieTypeEnum.Blue));
            }
            else
            {
                _blueAS.Add(new BlueDie(DieTypeEnum.Blue));
            }
        }

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
