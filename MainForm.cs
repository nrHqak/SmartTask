using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartTaskDistributor;

public class MainForm : Form
{
    private readonly TaskManager _taskManager = new();

    private readonly TextBox _txtTaskName = new();
    private readonly NumericUpDown _numDuration = new();
    private readonly NumericUpDown _numDifficulty = new();
    private readonly ComboBox _cmbPriority = new();
    private readonly ListBox _lstTasks = new();
    private readonly Label _lblSummary = new();

    public MainForm()
    {
        InitializeUi();
    }

    private void InitializeUi()
    {
        Text = "Smart Task Distributor";
        Size = new Size(900, 620);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = ColorTranslator.FromHtml("#1E1E2F");
        Font = new Font("Segoe UI", 10f);

        Label title = new()
        {
            Text = "Smart Task Distributor",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 20f),
            Location = new Point(25, 20),
            AutoSize = true
        };

        _txtTaskName.SetBounds(25, 90, 280, 36);
        _txtTaskName.PlaceholderText = "Название задачи";

        _numDuration.SetBounds(320, 90, 130, 36);
        _numDuration.Minimum = 1;
        _numDuration.Maximum = 480;
        _numDuration.Value = 30;

        _numDifficulty.SetBounds(465, 90, 130, 36);
        _numDifficulty.Minimum = 1;
        _numDifficulty.Maximum = 5;
        _numDifficulty.Value = 3;

        _cmbPriority.SetBounds(610, 90, 140, 36);
        _cmbPriority.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbPriority.Items.AddRange(["Низкий", "Средний", "Высокий"]);
        _cmbPriority.SelectedIndex = 1;

        var btnAdd = CreateButton("Добавить", 765, 90, OnAddClick, Color.FromArgb(76, 175, 80));
        var btnRemove = CreateButton("Удалить", 25, 145, OnRemoveClick, Color.FromArgb(0, 194, 255));
        var btnOptimize = CreateButton("Оптимизировать", 170, 145, OnOptimizeClick, Color.FromArgb(76, 175, 80));
        var btnClear = CreateButton("Очистить", 360, 145, OnClearClick, Color.FromArgb(225, 87, 89));

        _lstTasks.SetBounds(25, 205, 835, 320);
        _lstTasks.BackColor = Color.FromArgb(35, 35, 50);
        _lstTasks.ForeColor = Color.White;
        _lstTasks.BorderStyle = BorderStyle.None;

        _lblSummary.SetBounds(25, 540, 835, 30);
        _lblSummary.ForeColor = Color.FromArgb(200, 200, 220);
        _lblSummary.Text = "Задач: 0 | Общее время: 0 мин | Загруженность: Низкая";

        Controls.AddRange([title, _txtTaskName, _numDuration, _numDifficulty, _cmbPriority,
            btnAdd, btnRemove, btnOptimize, btnClear, _lstTasks, _lblSummary]);
    }

    private RoundedButton CreateButton(string text, int x, int y, EventHandler onClick, Color color)
    {
        RoundedButton button = new()
        {
            Text = text,
            NormalBackColor = color,
            HoverBackColor = ControlPaint.Light(color, .2f),
            BackColor = color
        };
        button.SetBounds(x, y, 130, 40);
        button.Click += onClick;
        return button;
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        try
        {
            string name = _txtTaskName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name) || _cmbPriority.SelectedItem is null)
            {
                MessageBox.Show("Введите название задачи и выберите приоритет.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Требование использования Convert.ToInt32
            int duration = Convert.ToInt32(_numDuration.Value);
            int difficulty = Convert.ToInt32(_numDifficulty.Value);
            string priority = _cmbPriority.SelectedItem.ToString() ?? "Низкий";

            // Проверка с логическими операторами
            if (duration <= 0 || (difficulty < 1 || difficulty > 5))
            {
                MessageBox.Show("Проверьте длительность и сложность задачи.", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _taskManager.AddTask(new TaskItem(name, duration, difficulty, priority));
            _txtTaskName.Clear();
            RefreshTaskList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось добавить задачу: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnRemoveClick(object? sender, EventArgs e)
    {
        if (_lstTasks.SelectedItem is not TaskItem selectedTask)
        {
            MessageBox.Show("Выберите задачу для удаления.", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _taskManager.RemoveTask(selectedTask);
        RefreshTaskList();
    }

    private void OnOptimizeClick(object? sender, EventArgs e)
    {
        _taskManager.SortTasks();
        RefreshTaskList();
    }

    private void OnClearClick(object? sender, EventArgs e)
    {
        _taskManager.ClearTasks();
        RefreshTaskList();
    }

    private void RefreshTaskList()
    {
        _lstTasks.Items.Clear();
        foreach (TaskItem task in _taskManager.Tasks)
        {
            _lstTasks.Items.Add(task);
        }

        int total = _taskManager.GetTotalTime();
        string loadLevel = total switch
        {
            <= 120 => "Низкая",
            <= 300 => "Средняя",
            _ => "Высокая"
        };

        _lblSummary.Text = $"Задач: {_taskManager.Tasks.Count} | Общее время: {total.ToString()} мин | Загруженность: {loadLevel}";
    }
}
