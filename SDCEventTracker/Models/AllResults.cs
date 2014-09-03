using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDCEventTracker.Models
{
    public class AllResults
    {
        public IEnumerable<Result> MorningHuntData { get; set; }
        public IEnumerable<Result> EveningHuntData { get; set; }
        public IEnumerable<Result> BarkingContestData { get; set; }
        public IEnumerable<Result> BenchShowData { get; set; }
    }

}