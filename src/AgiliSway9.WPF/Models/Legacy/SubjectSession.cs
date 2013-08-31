using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway9.WPF.Models.Legacy
{
    public class SubjectSession
    {
        public int Age;
        public Range WeightLb = new Range();
        public Range WeightKg = new Range();
        public Guid Id = Guid.NewGuid();
        public int K;

        public List<SubjectStep> Steps = new List<SubjectStep>();
    }
}
