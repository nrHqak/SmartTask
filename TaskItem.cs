namespace SmartTaskDistributor;

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
        int priorityScore = Priority switch
        {
            "Высокий" => 3,
            "Средний" => 2,
            _ => 1
        };

        return priorityScore * 100 + Difficulty * 10 - Duration / 10;
    }

    public override string ToString()
    {
        return $"{Name} | {Duration} мин | Сложность: {Difficulty} | Приоритет: {Priority}";
    }
}
