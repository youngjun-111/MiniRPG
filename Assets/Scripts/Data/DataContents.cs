using System;
using System.Collections.Generic;

namespace Data
{
    #region Stat
    //메모리에서 들고 있는 거를 파일로 변환 할 수 있는 공식
    [Serializable]
    public class Stat
    {
        public int level;
        public int hp;
        public int attack;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();
        //변환 하는 작업을 여기서 할려고
        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
            {
                dict.Add(stat.level, stat);
            }

            return dict;
        }
    }
}
#endregion
