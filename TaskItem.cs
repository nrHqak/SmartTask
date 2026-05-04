namespace SmartTaskDistributor
{
    public class TaskItem
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Difficulty { get; set; }
        public string Priority { get; set; }

        public TaskItem(string name, int duration, int difficulty, string priority)
        {
            Name = name;
            Duration = duration;
            Difficulty = difficulty;
            Priority = priority;
        }

        public int CalculateWeight()
        {
            int priorityScore;
            if (Priority == "Высокий")
            {
                priorityScore = 3;
            }
            else if (Priority == "Средний")
            {
                priorityScore = 2;
            }
            else
            {
                priorityScore = 1;
            }

            return priorityScore * 100 + Difficulty * 10 - Duration / 10;
        }

        public override string ToString()
        {
            return string.Format("{0} | {1} мин | Сложность: {2} | Приоритет: {3}", Name, Duration, Difficulty, Priority);
        }
    }
}
