using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Message
    {
        public double sentanceAVG { get; set; }
        public double wordAVG { get; set; }
        public double tokenRaitio { get; set; }
        public DateTime timeDate { get; set; }
        public double subjectWordCount { get; set; }
        public double sentanceCount { get; set; }
        public int SpellMistakeCount { get; set; }

        public Message(double sa, double wa, double tr, DateTime td, double swc, double sc,int smc)
        {
            this.sentanceAVG = sa;
            this.wordAVG = wa;
            this.tokenRaitio = tr;
            this.timeDate = td;
            this.subjectWordCount = swc;
            this.sentanceCount = sc;
            this.SpellMistakeCount = smc;
        }

    }
}
