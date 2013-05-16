using System;
using System.Xml;

namespace MonsterGolfOnline
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class GolfHole
    {
        private int m_holenumber = -1;
        private int m_teeid = -1;
        private int m_holepar = -1;
        private int m_holehcp = -1;

        public GolfHole(int number, int teeid, int par, int hcp)
        {
            m_holenumber = number;
            m_teeid = teeid;
            m_holepar = par;
            m_holehcp = hcp;
        }
        public int Number
        {
            get { return m_holenumber; }
            set { m_holenumber = value; }
        }
        public int TeeID
        {
            get { return m_teeid; }
            set { m_teeid = value; }
        }
        public int Par
        {
            get { return m_holepar; }
            set { m_holepar = value; }
        }
        public int Handicap
        {
            get { return m_holehcp; }
            set { m_holehcp = value; }
        }
    }
}
