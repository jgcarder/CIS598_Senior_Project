using System;
using System.Collections.Generic;
using System.Text;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.UpgradeObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    public class AssaultFrigateMarkII : Ship
    {
        private int _hull;
        private bool _shipTypeA;
        private bool _engToken;
        private bool _navToken;
        private bool _sqdToken;
        private bool _firToken;
        private List<DefenseToken> _defenseTokens;

        public override int Id { get; }

        public override int PointCost 
        { 
            get 
            {
                if(ShipTypeA)
                {
                    int x = 81;
                    foreach(var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
                else
                {
                    int x = 72;
                    foreach (var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
            } 
        }

        public override int Hull { get { return _hull; } set { _hull = value; } }

        public override int Command { get { return 3; } }

        public override int Squadron
        {
            get
            {
                if(ShipTypeA)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
        }

        public override int Engineering { get { return 4; } }

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
                    { -1, -1, -1, -1},
                    { 1, -1, -1, -1},
                    { 1, 1, -1, -1},
                    { 0, 1, 1, -1},
                    { -1, -1, -1, -1}
                };
            }
        }
        
                                                    //{Officer, Support, Weapons, Ordinance, Offensive, Turbolasers, Ion, Defensive}
        public override int[] UpgradeTypes { get { return new int[]{1, 0, 1, 0, 1, 1, 0, 1}; } }

        public override bool HasCommander { get; set; } = false;

        public override bool ShipTypeA
        {
            get { return _shipTypeA; }
            set
            {
                _shipTypeA = value;
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

        public override string Name { get { return "Assault Frigate Mark II"; } }

        public override Queue<CommandDialEnum> CommandDials { get; set; }

        public override UpgradeCard Title { get; set; }

        public override UpgradeCard Commander { get; set; }

        public override Texture2D Image { get; } //to be set

        public override Rectangle Source { get; } //to be set

        public override double Rotation { get; set; } = 0; //to be set

        public override Vector2 Position { get; set; } //to be set

        public override FiringArc[] Arcs { get; }

        public override List<UpgradeCard> Upgrades { get; set; }

        public override List<DefenseToken> DefenseTokens
        {
            get
            {
                return _defenseTokens;
            }
        }

        public override List<RedDie> RedAS { get; }

        public override List<BlueDie> BlueAS { get; }

        public override List<BlackDie> BlackAS { get; }

        public AssaultFrigateMarkII(int id, ContentManager content)
        {
            _hull = 6;

            _navToken = false;
            _engToken = false;
            _firToken = false;
            _sqdToken = false;

            Id = id;
            RedAS = new List<RedDie>();
            BlueAS = new List<BlueDie>();
            BlackAS = new List<BlackDie>();

            _defenseTokens = new List<DefenseToken>();

            Arcs = new FiringArc[4];
            Arcs[0] = new FiringArc(4, (Math.PI / 180) * 67.5);
            Arcs[1] = new FiringArc(3, (Math.PI / 180) * 110);
            Arcs[2] = new FiringArc(3, (Math.PI / 180) * 110);
            Arcs[3] = new FiringArc(2, (Math.PI / 180) * 72.5);

            ShipTypeA = true;

            _defenseTokens.Add(new RedirectDefenseToken(0));
            _defenseTokens.Add(new BraceDefenseToken(0));
            _defenseTokens.Add(new EvadeDefenseToken(0));

            CommandDials = new Queue<CommandDialEnum>();
        }

        public override void setHullDice(bool shipA)
        {
            foreach (var arc in Arcs) arc.ClearDice();

            RedAS.Clear();
            BlueAS.Clear();
            BlackAS.Clear();

            if (ShipTypeA)
            {
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

                Arcs[0].AddDice(2, DieTypeEnum.Red);
                Arcs[0].AddDice(1, DieTypeEnum.Blue);
                Arcs[1].AddDice(3, DieTypeEnum.Red);
                Arcs[1].AddDice(1, DieTypeEnum.Blue);
                Arcs[2].AddDice(3, DieTypeEnum.Red);
                Arcs[2].AddDice(1, DieTypeEnum.Blue);
                Arcs[3].AddDice(2, DieTypeEnum.Red);
                Arcs[3].AddDice(1, DieTypeEnum.Blue);
            }
            else
            {
                BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

                Arcs[0].AddDice(2, DieTypeEnum.Red);
                Arcs[1].AddDice(3, DieTypeEnum.Red);
                Arcs[1].AddDice(1, DieTypeEnum.Blue);
                Arcs[2].AddDice(3, DieTypeEnum.Red);
                Arcs[2].AddDice(1, DieTypeEnum.Blue);
                Arcs[3].AddDice(2, DieTypeEnum.Red);
            }
        }

        public override void RefreshDefense()
        {
            foreach (var token in _defenseTokens) token.Reset();
        }
    }
}
