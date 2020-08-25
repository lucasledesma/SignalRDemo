using System;
using System.Collections.Generic;
using SignalRDemo.Models;

namespace SignalRDemo.Helpers
{
    public class StateChecker
    {
        private readonly Random random;

        private int itemNo = 0;

        private readonly string[] Status =
            {"State 1", "State 2", "State 3", "Done!"};

        private readonly Dictionary<int, int> StatusTracker = new Dictionary<int, int>();


        public StateChecker(Random random)
        {
            this.random = random;
        }

        private int GetNextStatusIndex(int itemNo)
        {
            if (!StatusTracker.ContainsKey(itemNo))
                StatusTracker.Add(itemNo, -1);

            StatusTracker[itemNo]++;

            return StatusTracker[itemNo];
        }

        public int GetNewItem()
        {
            return ++itemNo;
        }

        public UpdateInfo GetUpdate(Item item)
        {
            if (random.Next(1, 5) != 4) return new UpdateInfo { New = false };

            var index = GetNextStatusIndex(item.Id);

            if (Status.Length <= index) index = Status.Length - 1;

            var result = new UpdateInfo
            {
                ItemId = item.Id,
                New = true,
                Update = Status[index],
                Finished = Status.Length - 1 == index
            };
            return result;

        }
    }
}