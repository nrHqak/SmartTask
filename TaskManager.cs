using System.Collections.Generic;
using System.Linq;

namespace SmartTaskDistributor;

public class TaskManager
{
    private readonly List<TaskItem> _tasks = [];

    public IReadOnlyList<TaskItem> Tasks => _tasks;

    public void AddTask(TaskItem task) => _tasks.Add(task);

    public void RemoveTask(TaskItem task) => _tasks.Remove(task);

    public void ClearTasks() => _tasks.Clear();

    public void SortTasks()
    {
        // Алгоритм сортировки:
        // 1) Получить список задач.
        // 2) Для каждой задачи рассчитать вес через CalculateWeight().
        // 3) Отсортировать: сначала по приоритету, затем по сложности, затем по времени.
        // 4) Вывести отсортированный результат в интерфейс.
        _tasks.Sort((a, b) =>
        {
            int byPriority = PriorityRank(b.Priority).CompareTo(PriorityRank(a.Priority));
            if (byPriority != 0) return byPriority;

            int byDifficulty = b.Difficulty.CompareTo(a.Difficulty);
            if (byDifficulty != 0) return byDifficulty;

            int byDuration = a.Duration.CompareTo(b.Duration);
            if (byDuration != 0) return byDuration;

            return b.CalculateWeight().CompareTo(a.CalculateWeight());
        });
    }

    public int GetTotalTime() => _tasks.Sum(t => t.Duration);

    private static int PriorityRank(string priority) => priority switch
    {
        "Высокий" => 3,
        "Средний" => 2,
        _ => 1
    };
}
