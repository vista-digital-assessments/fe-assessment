using BowlingGame.Core.Interfaces;
using BowlingGame.Web.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingGame.Web.Models
{
    public class BowlingContestant : IContestant
    {
        public bool IsInstanceComplete { get; set; }
        public string ContestantName { get; set; }
        public List<IScoreRecord> ScoringData { get; set; }

        public BowlingContestant()
        {
            ScoringData = new List<IScoreRecord>();
        }

        public int GetScore()
        {
            int totalScore = 0;
            //calculate score for frames 1-9
            foreach (var group in ScoringData.Where(x => x.ScoreFrame != 10).GroupBy(x => x.ScoreFrame))
            {
                int frameScore = 0;
                int frameNumber = group.Key;
                IEnumerable<IScoreRecord> currentFrame = group.AsEnumerable();

                //its a strike
                if (currentFrame.Count() == 1 && currentFrame.Sum(x => x.Score) == 10)
                {
                    IScoreRecord frame = currentFrame.Last();
                    int index = ScoringData.LastIndexOf(frame);
                    try
                    {
                        frameScore = 10 + ScoringData[index + 1].Score + ScoringData[index + 2].Score;
                    }
                    catch
                    {
                        try
                        {
                            frameScore = 10 + ScoringData[index + 1].Score;
                        }
                        catch
                        {
                            frameScore = 10;
                        }

                    }

                }
                else if (currentFrame.Count() == 2 && currentFrame.Sum(x => x.Score) == 10)
                {
                    IScoreRecord frame = currentFrame.Last();
                    int index = ScoringData.LastIndexOf(frame);
                    try
                    {
                        frameScore = 10 + ScoringData[index + 1].Score;
                    }
                    catch
                    {
                        frameScore = 10;
                    }

                }
                else
                {
                    frameScore = currentFrame.Sum(x => x.Score);
                }

                totalScore += frameScore;
            }

            //add 10th frame
            totalScore += ScoringData.Where(x => x.ScoreFrame == 10).Sum(x => x.Score);

            return totalScore;
        }
    }
}
