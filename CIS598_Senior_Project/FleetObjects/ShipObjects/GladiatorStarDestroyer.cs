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
    public class GladiatorStarDestroyer : Ship
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

        public override int Speed { get; set; } = 0;

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
                    {2, -1, -1, -1},
                    {1, 1, -1, -1},
                    {0, 1, 1, -1},
                    {-1, -1, -1, -1}
                };
            }
        }

        public override int[] UpgradeTypes { get { return new int[] { 1, 1, 1, 1, 0, 0, 0, 0 }; } }

        public override bool HasCommander { get; set; } = false;

        public override bool ShipTypeA
        {
            get { return _shipTypeA; }
            set
            {
                _shipTypeA = value;
                if (_shipTypeA) _points = 56;
                else _points = 62;
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

        public override string Name { get { return "Gladiator Star Destroyer"; } }

        public override Queue<CommandDialEnum> CommandDials { get; set; }

        public override UpgradeCard Title { get; set; }

        public override UpgradeCard Commander { get; set; }

        public override Texture2D Image { get; } //to be set

        public override Rectangle Source { get; } //to be set

        public override float Rotation { get; set; } = 0; //to be set

        public override Vector2 Position { get; set; } //to be set

        public override Vector2 Origin { get { return new Vector2(51, 75); } }

        public override BoundingCircle Bounds { get { return new BoundingCircle(new Vector2(Position.X, Position.Y), 36); } }

        public override BoundingCircle BowBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 75, Rotation + MathHelper.PiOver2), 36); } }

        public override BoundingCircle PortBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation + MathHelper.Pi), 36); } }

        public override BoundingCircle StarboardBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 50, Rotation), 36); } }

        public override BoundingCircle AftBounds { get { return new BoundingCircle(CollisionHelper.GetNewCoords(Bounds.Center, 75, Rotation + 3 * MathHelper.PiOver2), 36); } }

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

        public GladiatorStarDestroyer(int id, ContentManager content)
        {
            Upgrades = new UpgradeCard[8];

            Arcs = new FiringArc[4];
            Arcs[0] = new FiringArc(3, (Math.PI / 180) * 72.5);
            Arcs[1] = new FiringArc(2, (Math.PI / 180) * 110);
            Arcs[2] = new FiringArc(2, (Math.PI / 180) * 110);
            Arcs[3] = new FiringArc(1, (Math.PI / 180) * 67.5);

            _redAS = new List<RedDie>();
            _blueAS = new List<BlueDie>();
            _blackAS = new List<BlackDie>();

            ShipTypeA = true;

            Image = content.Load<Texture2D>("GladiatorSDToken103x153");

            _hull = 5;
            _command = 2;
            _squadron = 2;
            _engineering = 3;

            Id = id;
            
            _navToken = false;
            _engToken = false;
            _firToken = false;
            _sqdToken = false;

            _defenseTokens = new List<DefenseToken>();

            _defenseTokens.Add(new RedirectDefenseToken(0));
            _defenseTokens.Add(new EvadeDefenseToken(0));
            _defenseTokens.Add(new BraceDefenseToken(0));

            CommandDials = new Queue<CommandDialEnum>();
        }

        public override void RefreshDefense()
        {
            foreach (var token in _defenseTokens) token.Reset();
        }

        public override void setHullDice(bool shipA)
        {
            foreach (var arc in Arcs) arc.ClearDice();

            RedAS.Clear();
            BlueAS.Clear();
            BlackAS.Clear();

            Arcs[0].AddDice(2, DieTypeEnum.Red);
            Arcs[0].AddDice(2, DieTypeEnum.Black);
            Arcs[0].AddDice(2, DieTypeEnum.Red);
            Arcs[0].AddDice(2, DieTypeEnum.Black);

            if (ShipTypeA)
            {
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

                Arcs[1].AddDice(4, DieTypeEnum.Black);
                Arcs[2].AddDice(4, DieTypeEnum.Black);
            }
            else
            {
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

                Arcs[1].AddDice(3, DieTypeEnum.Black);
                Arcs[1].AddDice(1, DieTypeEnum.Red);
                Arcs[2].AddDice(3, DieTypeEnum.Black);
                Arcs[2].AddDice(1, DieTypeEnum.Red);
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
