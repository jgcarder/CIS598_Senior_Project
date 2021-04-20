using CIS598_Senior_Project.FleetObjects.DefenseTokenObjects;
using CIS598_Senior_Project.FleetObjects.DiceObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIS598_Senior_Project.FleetObjects.ShipObjects
{
    public class VictoryStarDestroyer : Ship
    {
        private int _hull;
        private bool _shipTypeA;
        private List<DefenseToken> _defenseTokens;

        public override int Id { get; }

        public override int PointCost
        {
            get
            {
                if (ShipTypeA)
                {
                    int x = 73;
                    foreach (var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
                else
                {
                    int x = 85;
                    foreach (var upgd in Upgrades)
                    {
                        x += upgd.PointCost;
                    }
                    return x;
                }
            }
        }
        public override int Hull { get { return _hull; } }

        public override int Command { get { return 3; } }

        public override int Squadron { get { return 3; } }

        public override int Engineering { get { return 4; } }

        public override int Speed { get; set; } = 0;

        public override int[,] Movement
        {
            get
            {
                return new int[,]
                {
                    {-1, -1, -1, -1},
                    {1, -1, -1, -1},
                    {0, 1, -1, -1},
                    {-1, -1, -1, -1},
                    {-1, -1, -1, -1}
                };
            }
        }

        public override int[] UpgradeTypes
        {
            get
            {
                if (ShipTypeA)
                {
                    return new int[] { 1, 0, 1, 1, 1, 0, 1, 0 };
                }
                else
                {
                    return new int[] { 1, 0, 1, 0, 1, 1, 1, 0 };
                }
            }
        }

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

        public override string Name { get { return "Victory Star Destroyer"; } }

        public override UpgradeCard Title { get; set; }

        public override UpgradeCard Commander { get; set; }

        public override FiringArc[] Arcs { get; }

        public override List<UpgradeCard> Upgrades { get; }

        public override List<DefenseToken> DefenseTokens { get { return _defenseTokens; } }

        public override List<RedDie> RedAS { get; }

        public override List<BlueDie> BlueAS { get; }

        public override List<BlackDie> BlackAS { get; }

        public VictoryStarDestroyer(int id)
        {
            _hull = 8;

            Id = id;
            RedAS = new List<RedDie>();
            BlueAS = new List<BlueDie>();
            BlackAS = new List<BlackDie>();

            _defenseTokens = new List<DefenseToken>();

            BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

            Arcs = new FiringArc[4];
            Arcs[0] = new FiringArc(3, (Math.PI / 180) * 105);
            Arcs[1] = new FiringArc(3, (Math.PI / 180) * 85);
            Arcs[2] = new FiringArc(3, (Math.PI / 180) * 85);
            Arcs[3] = new FiringArc(1, (Math.PI / 180) * 85);

            ShipTypeA = true;

            _defenseTokens.Add(new RedirectDefenseToken(0));
            _defenseTokens.Add(new RedirectDefenseToken(1));
            _defenseTokens.Add(new BraceDefenseToken(0));
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

            BlueAS.Add(new BlueDie(DieTypeEnum.Blue));

            if (ShipTypeA)
            {
                Arcs[0].AddDice(3, DieTypeEnum.Red);
                Arcs[0].AddDice(3, DieTypeEnum.Black);
                Arcs[1].AddDice(2, DieTypeEnum.Red);
                Arcs[1].AddDice(1, DieTypeEnum.Black);
                Arcs[2].AddDice(2, DieTypeEnum.Red);
                Arcs[2].AddDice(1, DieTypeEnum.Black);
                Arcs[3].AddDice(2, DieTypeEnum.Red);
            }
            else
            {
                Arcs[0].AddDice(3, DieTypeEnum.Red);
                Arcs[0].AddDice(3, DieTypeEnum.Blue);
                Arcs[1].AddDice(2, DieTypeEnum.Red);
                Arcs[1].AddDice(1, DieTypeEnum.Blue);
                Arcs[2].AddDice(2, DieTypeEnum.Red);
                Arcs[2].AddDice(1, DieTypeEnum.Blue);
                Arcs[3].AddDice(2, DieTypeEnum.Red);
            }
        }
    }
}
